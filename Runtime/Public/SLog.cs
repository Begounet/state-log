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

        private static bool IsEnabled => _settings.Enabled;

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
                Application.quitting += OnApplicationQuit;
            }
        }

        public static void StartStep(string stepName)
        {
            if (!IsEnabled) return;

            Init();            
            _currentLogStep = new LogStepData() { Name = stepName };
            _logData.Steps.Add(_currentLogStep);
        }

        private static void OnApplicationQuit()
        {
            DumpLogData();
            _settings = null;
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

            string jLogData = JsonUtility.ToJson(_logData, prettyPrint: true);

            int counter = 0;
            string filePath;
            do
            {
                filePath = Path.Combine(Application.persistentDataPath, string.Format(StateLogNameFormat, ++counter));
            } while (File.Exists(filePath));

            CreateLogDirectoryIFN(filePath);

            File.WriteAllText(filePath, jLogData);

            if (_settings.LogOutputDestination)
            {
                Debug.Log($"Dump state log: {filePath}");
            }
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
