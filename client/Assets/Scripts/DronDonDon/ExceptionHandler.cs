using System;
using System.Collections.Generic;
using System.Linq;
using Adept.Logger;
using IoC.Extension;
using JetBrains.Annotations;
using RSG;
using UnityEngine;

namespace DronDonDon
{
    public class ExceptionHandler : MonoBehaviour
    {
        private const int MAX_DUPLICATE_EXCEPTIONS = 3;

        private readonly List<ExceptionData> _exceptionList = new List<ExceptionData>();
        private bool _active = true;
        private float _exceptionTime;

        private static IAdeptLogger Logger
        {
            get { return LoggerFactory.GetLogger<ExceptionHandler>(); }
        }

        // [Inject]
        // private AcceptanceCommandNetManager _acceptanceCommandNetManager;

        private void Awake()
        {
            this.Inject();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLogMessage;
            Promise.UnhandledException += HandlePromiseException;
        }
        
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
            Promise.UnhandledException -= HandlePromiseException;
        }

        private void Update()
        {
            if (!_active) {
                return;
            }

            _exceptionTime += Time.deltaTime;

            if (_exceptionTime > 1f) {
                LogExceptions();
            }
        }
        
        private void HandlePromiseException(object sender, ExceptionEventArgs e)
        {
            if (!_active) {
                return;
            }
            HandleException(e.Exception.Message, e.Exception.StackTrace, LogType.Exception);
        }

        private void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            if (!_active) {
                return;
            }

            if (type == LogType.Exception) {
                HandleException(logString, stackTrace, type);
            }
        }

        private void HandleException(string logString, string stackTrace, LogType type)
        {
            ExceptionData exceptionData = ExceptionAlreadyHandled(logString, stackTrace, type);
            if (exceptionData == null) {
                _exceptionList.Add(new ExceptionData(logString, stackTrace, type));
            } else {
                exceptionData.IncCount();
                if (exceptionData.Count < MAX_DUPLICATE_EXCEPTIONS) {
                    return;
                }
                LogExceptions();
                _active = false;
            }
        }

        private void LogExceptions()
        {
            try {
                _exceptionTime = 0f;
                if (_exceptionList.Count <= 0) {
                    return;
                }
                foreach (ExceptionData data in _exceptionList) {
                    Logger.Error("*Uncaught Error* ErrorType: " + data.Type + " ErrorMessage: " + data.LogString + " ErrorStackTrace:"
                                 + data.StackTrace);
                }

                _exceptionList.Clear();
            } catch (Exception) {
                // silent method
            }
        }

        [CanBeNull]
        private ExceptionData ExceptionAlreadyHandled(string logString, string stackTrace, LogType type)
        {
            return _exceptionList.FirstOrDefault(data => data.Compare(logString, stackTrace, type));
        }

        // private void Alert(string title, string message, [CanBeNull] UnityAction callback, string stackTrace)
        // {
        //     DialogService dialogService = AppContext.Container.Resolve<DialogService>();
        //     Dial
        //     dialogService.DE(title, message, callback, stackTrace);
        // }

        private class ExceptionData
        {
            public ExceptionData(string logString, string stackTrace, LogType type)
            {
                LogString = logString;
                StackTrace = stackTrace;
                Type = type;
            }

            public string LogString { get; }
            public string StackTrace { get; }
            public LogType Type { get; }
            public int Count { get; private set; }

            public void IncCount()
            {
                Count++;
            }

            public bool Compare(string logString, string stackTrace, LogType type)
            {
                return logString == LogString && stackTrace == StackTrace && type == Type;
            }
        }
    }
}