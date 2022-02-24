using UnityEngine;
using System;
using UnityEngine.AddressableAssets;

namespace StateLog
{
    public class StateLogSettings : ScriptableObject
    {
        public const string AddressableGroupName = "Settings";
        public const string AddressableName = "StateLogSettings";

        [SerializeField, Tooltip("Enable the capture and generation of the state log file")]
        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        [SerializeField, Tooltip("If true, the destination of the generated state log file will be displayed when the application ends.")]
        private bool _logOutputDestination = true;
        public bool LogOutputDestination
        {
            get => _logOutputDestination;
            set => _logOutputDestination = value;
        }

        public static StateLogSettings GetSettings()
        {
            var op = Addressables.LoadAssetAsync<StateLogSettings>(AddressableName);
            return op.WaitForCompletion();
        }
    }
}
