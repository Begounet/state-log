using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateLog
{
    public class SLogInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SLogInspectorView, UxmlTraits> { }

        private Label _stepTitleLabel;
        private Label _messageLabel;
        private EditorSelectableLabel _stackTraceLabel; 

        public SLogInspectorView()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathUtils.Resources.SLogInspectorViewUxml);
            visualTree.CloneTree(this);

            _stepTitleLabel = this.Q<Label>("step-title");
            _messageLabel = this.Q<Label>("message");
            _stackTraceLabel = this.Q<EditorSelectableLabel>("stacktrace");
        }

        public void LoadLogStepData(LogStepData logStepData, LogItemData logItemData)
        {
            if (logStepData == null || logItemData == null)
            {
                ClearSteps();
                return;
            }

            _stepTitleLabel.text = logStepData.Name;
            _messageLabel.text = logItemData.Message;
            _stackTraceLabel.text = StackTraceUtils.StackTraceWithHyperlink(logItemData.StackTrace);
        }

        public void ClearSteps()
        {
            _stepTitleLabel.text = string.Empty;
            _messageLabel.text = string.Empty;
            _stackTraceLabel.text = string.Empty;
        }
    }
}