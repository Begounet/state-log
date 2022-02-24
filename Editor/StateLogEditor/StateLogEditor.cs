using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.IO;

namespace StateLog
{
    public class StateLogEditor : EditorWindow
    {
        public LogData LogData { get; private set; }

        private ScrollView _categoryScrollView;
        private SLogInspectorView _inspectorView;

        [MenuItem("Tools/State Log Viewer")]
        public static void ShowStateLogViewer()
        {
            StateLogEditor wnd = GetWindow<StateLogEditor>();
            wnd.titleContent = new GUIContent("State Log Viewer");
            wnd.Show();
        }

        public void CreateGUI()
        {
            titleContent = new GUIContent("StateLogEditor");

            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathUtils.Resources.StateLogEditorUxml);
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathUtils.Resources.StateLogEditorUss);
            root.styleSheets.Add(styleSheet);

            _categoryScrollView = root.Q<ScrollView>("category-list");
            _inspectorView = root.Q<SLogInspectorView>();

            var fileMenu = root.Q<ToolbarMenu>("menu-file");
            FillFileMenu(fileMenu.menu);
        }

        private void FillFileMenu(DropdownMenu menu)
        {
            menu.AppendAction("Open...", (a) =>
            {
                string filePath = EditorUtility.OpenFilePanelWithFilters("Open Graph log file", Application.persistentDataPath, new string[] { "Json", "json" });
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    try
                    {
                        string jData = File.ReadAllText(filePath);
                        var logData = JsonUtility.FromJson<LogData>(jData);
                        LoadLogData(logData);
                    }
                    catch (Exception ex)
                    {
                        EditorUtility.DisplayDialog("Opening Graph log error", ex.Message, "Ok");
                        _inspectorView.Clear();
                    }
                }
            });
        }

        public void LoadLogData(LogData data)
        {
            _inspectorView.ClearSteps();
            _inspectorView.visible = false;
            _categoryScrollView.Clear();

            LogData = data;

            foreach (var logStep in data.EnumerateLogSteps())
            {
                if (!logStep.HasItems)
                {
                    continue;
                }

                var logCategory = new SLogCategoryView();
                logCategory.Populate(logStep);
                logCategory.OnLogItemSelected += OnLogItemSelected;
                _categoryScrollView.Add(logCategory);
            }
        }

        private void OnLogItemSelected(LogStepData logStepData, LogItemData logItemData)
        {
            _inspectorView.visible = true;
            _inspectorView.LoadLogStepData(logStepData, logItemData);
        }
    }
}