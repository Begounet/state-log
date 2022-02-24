using UnityEngine;

namespace StateLog
{
    public static class PathUtils
    {
        public const string BaseAssetPath = "Packages/com.apperture.state-log";
        public const string EditorAssetPath = BaseAssetPath + "/Editor";
        public const string StateLogEditorAssetPath = EditorAssetPath + "/StateLogEditor";
        public const string StateLogEditorSubAssetPath = StateLogEditorAssetPath + "/Sub";

        public class Resources
        {
            public const string StateLogEditorUxml = StateLogEditorAssetPath + "/StateLogEditor.uxml";
            public const string StateLogEditorUss = StateLogEditorAssetPath + "/StateLogEditor.uss";

            public const string SLogCategoryViewUxml = StateLogEditorSubAssetPath + "/SLogCategoryView.uxml";
            public const string SLogContainerViewUxml = StateLogEditorSubAssetPath + "/SLogContainerView.uxml";
            public const string SLogInspectorViewUxml = StateLogEditorSubAssetPath + "/SLogInspectorView.uxml";
        }
    }
}
