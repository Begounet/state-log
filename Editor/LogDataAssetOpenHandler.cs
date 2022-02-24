using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace StateLog
{
    public class LogDataAssetOpenHandler
    {
        [OnOpenAsset]
        public static bool Open(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is LogDataAsset logDataAsset)
            {
                var StateLogEditor = EditorWindow.CreateWindow<StateLogEditor>();
                StateLogEditor.LoadLogData(logDataAsset.Data);
                StateLogEditor.Show();
                return true;
            }
            return false;
        }
    }
}

