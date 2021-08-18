import com.tortuga.model.PlatformType
import com.tortuga.model.PlatformType
import com.tortuga.service.DurationService

class BuildContext {
    static path
    static projectOptions
    static branch
    static version

    static job
    static nexusConfig

    static notificationService
    static checkoutService
    static stageService
    static oldStageService
    static deployerService
    static cacheService
    static awxService

    static List<String> artifactIds

    static unityVersion

    static Map activeBuilds = [
            'windows': false,
            'android': false,
    ]
   
    static def isProduction() {
        return BuildContext.branch.isProduction()
    }
}

pipeline {
    agent any
    parameters {
        booleanParam(name: 'forceBuildLib', defaultValue: false, description: 'use cache lib')
        booleanParam(name: 'recreate', defaultValue: false, description: 'delete workspace and recreate it from develop')

        booleanParam(name: 'androidBuild', defaultValue: true, description: 'build android')
        booleanParam(name: 'windowsBuild', defaultValue: true, description: 'build windows')
    }

    options {
        parallelsAlwaysFailFast()
        skipDefaultCheckout()
        disableConcurrentBuilds()
    }

    stages {
        stage("init-context") {
            steps {
                script {
                    configFileProvider([configFile(fileId: 'dronDonDon-project-config', variable: 'projectFilePath')]) {
                        BuildContext.projectOptions = readJSON file: projectFilePath
                    }

                    BuildContext.artifactIds = new ArrayList<>()
                    BuildContext.branch = getBranchInfo()
                    BuildContext.job = getJobInfo()
                    BuildContext.notificationService = createNotificationService(BuildContext.projectOptions['slackChannel'])
                    BuildContext.checkoutService = createCheckoutService(BuildContext.projectOptions["repoUrl"])
                    BuildContext.path = getPathOptions()
                    BuildContext.stageService = createNewStageService(BuildContext.projectOptions["stageUrl"],
                            BuildContext.projectOptions["projectPrefix"])

                    BuildContext.oldStageService = createStageService(BuildContext.projectOptions["projectPrefix"],
                            BuildContext.projectOptions["nexusId"], BuildContext.projectOptions["stageUrl"])

                    BuildContext.deployerService = createDeployerService(BuildContext.projectOptions["projectPrefix"],
                            BuildContext.projectOptions["nexusId"], BuildContext.projectOptions["deployerUrl"])

                    BuildContext.cacheService = createCacheService(BuildContext.projectOptions["cacheBranch"])

                    configFileProvider([configFile(fileId: 'tortuga-nexus-config', variable: 'nexusConfig')]) {
                        BuildContext.nexusConfig = readJSON file: nexusConfig
                    }

                    if (BuildContext.projectOptions["deletePreviousJob"]) {
                        abortPreviousBuild()
                    }

                    if (BuildContext.branch.isWip()) {
                        currentBuild.result = 'ABORTED'
                        error('Aborted wip')
                    }
                     BuildContext.awxService = createAwxService("http://awx.tortu.ga", "ava3d-stage-runner")
                     fillActiveBuilds()
                }
            }
        }

        stage("checkout") {
            steps {
                script {
                     if (!fileExists(BuildContext.path.repoPath)) {
                        echo "restore from develop cache"
                        BuildContext.cacheService.restoreRepo(BuildContext.path.repoPath)
                    }

                    BuildContext.checkoutService.checkout(BuildContext.branch, BuildContext.path.repoPath)
                    fileOperations([folderDeleteOperation(BuildContext.path.targetPath)])
                    BuildContext.version = readFile("${BuildContext.path.repoPath}/version")
                }
            }
        }

        stage("build") {
            parallel {
                
                stage('windows-client') {
                    when {
                        equals expected: true, actual: checkBuild("windows")
                    }
                    stages {
                
                        stage("build") {
                            steps {
                                buildAppClient(BuildContext, PlatformType.WINDOWS,  params.forceBuildLib)
                            }
                        }

                        stage("deploy") {
                            steps {
                                script {
                                    deployClient(PlatformType.WINDOWS)
                                }
                            }
                        }
                    }
                }

                stage('android') {
                    when {
                        equals expected: true, actual: checkBuild("android")
                    }
                    stages {                   
                        stage("build-client") {
                            steps {
                                buildAppClient(BuildContext, PlatformType.ANDROID, params.forceBuildLib)
                            }
                        }

                        stage("deploy-client") {
                            steps {
                                script {
                                    deployClient(PlatformType.ANDROID)
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    post {
        failure {
            script {
                BuildContext.notificationService.notifyBuildFault(BuildContext.job, BuildContext.branch.user)
            }
        }

        success {
            script {
                BuildContext.notificationService.notifyBuildSuccess(BuildContext.job, BuildContext.branch.user)
            }
        }
    }
}

private void fillActiveBuilds() {
    BuildContext.activeBuilds['android'] = params.androidBuild
    BuildContext.activeBuilds['windows'] = params.windowsBuild
}

private static boolean checkBuild(String type) {
    return BuildContext.activeBuilds[type]
}

private void deployClient(PlatformType platform) {

}
def buildAppClient(def context, PlatformType type, forceBuildLib = false) {
    def clientBuildPath = "${context.path.buildPath}/${type.name}client"
    DurationService durationService = new DurationService(this)
    if (!forceBuildLib) {
        durationService.start("clientLibSaveLocalCache")
        saveLocalCache(clientBuildPath, "Library", "${type.name}client")

        durationService.end("clientLibSaveLocalCache")
    }

    if (!fileExists(clientBuildPath)) {
        fileOperations([folderCreateOperation(clientBuildPath)])
    }

    syncDirectory("${context.path.repoPath}/client/", clientBuildPath)

    if (!forceBuildLib) {
        durationService.start("clientLibRestoreLocalCache")
        restoreLocalCache(clientBuildPath, "Library", "${type.name}client")
        durationService.end("clientLibRestoreLocalCache")
    }

// retrieve  cache from master branch
    def libPath = "${clientBuildPath}/Library"
    if (!fileExists(libPath) && !forceBuildLib) {
        echo "try restore"
        durationService.start("restoreClientUnityLib")
        context.cacheService.restoreClientUnityLib(type, libPath)
        durationService.end("restoreClientUnityLib")
    }

    dir(clientBuildPath) {
        def prod = context.isProduction();
        
        def buildName = context.branch.safeBranchName
                .replace('feature-', '')
                .replace('release-', '')
                .replace('bugfix-', '')
                .replace('hotfix-', '')
        withCredentials([usernamePassword(credentialsId: 'unityUserPass', passwordVariable: 'unityPass', usernameVariable: 'unityUser')]) {

            createMaven()
                    .phase('clean')
                    .phase('package')
                    .param('profiler', prod ? "false" : "true")
                    .param('il2cpp', prod ? "true" : "false")
                    .param('splitApk', prod ? "true" : "false")
                    .param('platform', type.name)
                    .param('skipReplace', 'false')
                    .param('skipTest', 'true')
                    .param('buildVersion', context.version)
                    .param('buildName', buildName)
                    .param('unityUser', unityUser)
                    .param('unityPass', unityPass)
                    .param("production", "false" )
                    .run()
        }
        fileOperations([fileCopyOperation(excludes: '', flattenFiles: true, includes: "target/*.zip",
                targetLocation: context.path.targetPath)])
    }
}



