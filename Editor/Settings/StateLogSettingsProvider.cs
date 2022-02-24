using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;

namespace StateLog
{
    public class StateLogSettingsProvider
    {
        private const string ProjectTitlePath = "Project/State Log";
        private const string DefaultAssetPath = "Packages/com.apperture.state-log/Runtime/StateLog-Settings.asset";

        private static readonly string[] Keywords =
        {
            "",
        };

        private static SerializedObject _settingsSO = null;

        [SettingsProvider]
        private static SettingsProvider CreateProjectPreferencesSettingsProvider()
        {
            return new SettingsProvider(ProjectTitlePath, SettingsScope.Project)
            {
                guiHandler = DrawProjectGUI,
                keywords = Keywords
            };
        }

        private static void DrawProjectGUI(string obj)
        {
            if (_settingsSO == null)
            {
                var settings = GetOrCreateSettings<StateLogSettings>();
                _settingsSO = new SerializedObject(settings);
            }

            if (_settingsSO == null)
            {
                return;
            }

            _settingsSO.Update();
            DrawSettings();
        }

        private static void DrawSettings()
        {
            EditorGUI.BeginChangeCheck();

            SerializedProperty sp = _settingsSO.GetIterator();

            sp.NextVisible(true);
            while (sp.NextVisible(false))
            {
                EditorGUILayout.PropertyField(sp);
            }

            if (EditorGUI.EndChangeCheck())
            {
                _settingsSO.ApplyModifiedProperties();
            }
        }

        private static T GetOrCreateSettings<T>() where T : ScriptableObject
        {
            T settings = null;

            string[] settingsPaths = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            if (settingsPaths.Length == 0)
            {
                settings = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(settings, DefaultAssetPath);
                AddToAddressables();
            }
            else
            {
                if (settingsPaths.Length > 1)
                {
                    Debug.LogWarning($"Multiple {typeof(T).Name} have been found. Should be only one in the project.");
                }

                string settingsPath = AssetDatabase.GUIDToAssetPath(settingsPaths[0]);
                settings = AssetDatabase.LoadAssetAtPath<T>(settingsPath);
            }

            return settings;
        }

        private static void AddToAddressables()
        {
            var settingsAddr = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(DefaultAssetPath);
            var groupSettings = settingsAddr.FindGroup(StateLogSettings.AddressableGroupName);
            if (groupSettings == null)
            {
                groupSettings = settingsAddr.CreateGroup(StateLogSettings.AddressableGroupName, setAsDefaultGroup: false, readOnly: true, postEvent: false, null);
            }
            settingsAddr.CreateAssetReference(guid);
            var entry = settingsAddr.CreateOrMoveEntry(guid, groupSettings, readOnly: true);
            entry.SetAddress(StateLogSettings.AddressableName);
        }
    }
}

