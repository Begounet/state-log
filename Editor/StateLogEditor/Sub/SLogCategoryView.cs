using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateLog
{
    public class SLogCategoryView : VisualElement
    {
        public event Action<LogStepData, LogItemData> OnLogItemSelected;

        public new class UxmlFactory : UxmlFactory<SLogCategoryView, UxmlTraits> { }

        private LogStepData _logStepData;

        public SLogCategoryView()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathUtils.Resources.SLogCategoryViewUxml);
            visualTree.CloneTree(this);

            var content = this.Q<ListView>();
            content.makeItem += MakeItem;
            content.bindItem += BindItem;
            content.onSelectedIndicesChange += OnListSelectionIndicesChanged; ;
        }

        private VisualElement MakeItem()
            => new SLogContainerView();

        private void BindItem(VisualElement root, int index)
        {
            var logContainerView = root as SLogContainerView;
            logContainerView.Populate(_logStepData.Items[index]);
        }

        public void Populate(LogStepData logStepData)
        {
            var content = this.Q<ListView>();
            content.Clear();

            // Fold in list by default
            content.Q<Foldout>("unity-list-view__foldout-header").value = false;

            _logStepData = logStepData;
            content.headerTitle = logStepData.Name;
            content.itemsSource = logStepData.Items;
        }

        private void OnListSelectionIndicesChanged(System.Collections.Generic.IEnumerable<int> itemIndexes)
        {
            if (itemIndexes.Any())
            {
                var itemIndex = itemIndexes.FirstOrDefault();
                OnLogItemSelected.Invoke(_logStepData, _logStepData.Items[itemIndex]);
            }
            else
            {
                OnLogItemSelected.Invoke(_logStepData, null);
            }
        }
    }
}
