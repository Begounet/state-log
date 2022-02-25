using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StateLog
{
    public static class SLog
    {
        private const string StateLogNameFormat = "StateLogs/StateLog_{0}.json";

        private static LogData _logData;
        private static LogStepData _currentLogStep;
        private static StateLogSettings _settings;

        private static bool IsEnabled => (_settings?.Enabled ?? false);
        private static string LogFilePath = string.Empty;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            _settings = StateLogSettings.GetSettings();
            if (!_settings.Enabled)
            {
                return;
            }

            if (_logData == null)
            {
                _logData = new LogData();
                _logData.Steps = new List<LogStepData>();

                Application.logMessageReceived += OnLogMessageReceived;
                Application.focusChanged += OnApplicationFocusChanged;
                Application.quitting += OnApplicationQuit;
            }
        }

        public static void StartStep(string stepName)
        {
            Init();

            if (!IsEnabled) return;

            _currentLogStep = new LogStepData() { Name = stepName };
            _logData.Steps.Add(_currentLogStep);
        }

        private static void OnApplicationQuit()
        {
            DumpLogData();
        }

        private static void OnApplicationFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                DumpLogData();
            }
        }

        private static void OnLogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            var logItemData = new LogItemData()
            {
                Message = condition,
                StackTrace = stackTrace,
                LogType = logType
            };

            var items = (_currentLogStep != null) ? _currentLogStep.Items : _logData.Uncategorized.Items;
            items.Add(logItemData);
        }

        private static void DumpLogData()
        {
            if (!IsEnabled) return;

            CacheOutputFilePath();

            string jLogData = JsonUtility.ToJson(_logData, prettyPrint: true);

            CreateLogDirectoryIFN(LogFilePath);
            File.WriteAllText(LogFilePath, jLogData);

            if (_settings.LogOutputDestination)
            {
                Debug.Log($"Dump state log: {LogFilePath}");
            }
        }

        private static void CacheOutputFilePath()
        {
            if (!string.IsNullOrEmpty(LogFilePath))
            {
                return;
            }

            int counter = 0;
            do
            {
                LogFilePath = Path.Combine(Application.persistentDataPath, string.Format(StateLogNameFormat, ++counter));
            } while (File.Exists(LogFilePath));
        }

        private static void CreateLogDirectoryIFN(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
