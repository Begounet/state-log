using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateLog
{
    public class SLogContainerView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SLogContainerView, UxmlTraits> { }

        private Texture2D _iconInfo;
        private Texture2D _iconWarn;
        private Texture2D _iconError;

        public SLogContainerView()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathUtils.Resources.SLogContainerViewUxml);
            visualTree.CloneTree(this);

            _iconInfo = IconLoaderUtils.LoadIcon("console.infoicon.sml");
            _iconWarn = IconLoaderUtils.LoadIcon("console.warnicon.sml");
            _iconError = IconLoaderUtils.LoadIcon("console.erroricon.sml");
        }

        public void Populate(LogItemData logItemData)
        {
            this.Q<Label>().text = logItemData.Message;

            Texture2D iconTex = SelectIconByType(logItemData.LogType);
            this.Q<VisualElement>("icon").style.backgroundImage = Background.FromTexture2D(iconTex);
        }

        private Texture2D SelectIconByType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    return _iconError;
                case LogType.Warning:
                    return _iconWarn;
                case LogType.Log:
                    return _iconInfo;

                default:
                    return null;
            }
        }
    }
}
