using System;
using DronDonDon.Core.Audio.Service;
using IoC;
using IoC.Api;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DronDonDon.Core.Audio.IoC
{
    public class AudioModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<AudioService>(() => CreateAudioService(container));
            container.RegisterSingleton<SoundService>();
            container.RegisterSingleton<MusicService>();
        }

        private AudioService CreateAudioService(IIoCContainer container)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("Embeded/Audio/pfAudioManager");
            if (audioManagerPrefab == null) {
                throw new NullReferenceException("Audio Manager prefab not found!");
            }
            GameObject rootObj = ((IoCContainer) container).gameObject;
            GameObject audioManagerInstance = Object.Instantiate(audioManagerPrefab, rootObj.transform, false);
            audioManagerInstance.name = "_audioManager_";
            audioManagerInstance.transform.SetAsLastSibling();
            return audioManagerInstance.GetComponent<AudioService>();
        }
    }
}