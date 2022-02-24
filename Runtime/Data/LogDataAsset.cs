using System.IO;
using UnityEngine;

namespace StateLog
{
    [CreateAssetMenu(fileName = "LogDataAsset", menuName = "LogDataAsset")]
    public class LogDataAsset : ScriptableObject
    {
        [SerializeField]
        private LogData _data;
        public LogData Data
        {
            get => _data;
            set => _data = value;
        }

        [ContextMenu("Dump as Json")]
        public void DumpAsJson()
        {
            string jData = JsonUtility.ToJson(_data, prettyPrint: true);
            string filePath = Path.Combine(Application.dataPath, "StateLog.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, jData);
        }
    }
}
