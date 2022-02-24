using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateLog
{
    [Serializable]
    public class LogData
    {
        [SerializeField]
        private LogStepData _uncategorized = new LogStepData() { Name = "Uncategorized" };
        public LogStepData Uncategorized
        {
            get => _uncategorized;
            set => _uncategorized = value;
        }

        [SerializeField]
        private List<LogStepData> _steps = new List<LogStepData>();
        public List<LogStepData> Steps
        {
            get => _steps;
            set => _steps = value;
        }

        public IEnumerable<LogStepData> EnumerateLogSteps()
        {
            yield return _uncategorized;
            foreach (var step in _steps)
            {
                yield return step;
            }
        }
    }
}
