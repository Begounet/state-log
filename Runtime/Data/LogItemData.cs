using UnityEngine;

namespace StateLog
{
    [System.Serializable]
    public class LogItemData
    {
        [SerializeField]
        [Multiline]
        private string _message;
        public string Message
        {
            get => _message;
            set => _message = value;
        }

        [SerializeField]
        [Multiline]
        private string _stackTrace;
        public string StackTrace
        {
            get => _stackTrace;
            set => _stackTrace = value;
        }

        [SerializeField]
        private LogType _logType;
        public LogType LogType
        {
            get => _logType;
            set => _logType = value;
        }
    }
}
