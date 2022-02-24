using System.Collections.Generic;
using UnityEngine;

namespace StateLog
{
    [System.Serializable]
    public class LogStepData
    {
        [SerializeField]
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField]
        private List<LogItemData> _items = new List<LogItemData>();
        public List<LogItemData> Items
        {
            get => _items;
            set => _items = value;
        }

        public bool HasItems => (_items != null && _items.Count > 0);
    }
}
