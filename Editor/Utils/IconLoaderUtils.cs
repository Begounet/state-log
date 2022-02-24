using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace StateLog
{
    public static class IconLoaderUtils
    {
        private delegate Texture2D LoadIconDlg(string name);

        private static LoadIconDlg _loadIconDlg;

        public static Texture2D LoadIcon(string name)
        {
            CacheLoadIconMethodIFN();
            return _loadIconDlg.Invoke(name);
        }

        private static void CacheLoadIconMethodIFN()
        {
            if (_loadIconDlg == null)
            {
                var loadIconMI = typeof(EditorGUIUtility).GetMethod("LoadIcon", BindingFlags.Static | BindingFlags.NonPublic);
                _loadIconDlg = (LoadIconDlg)loadIconMI.CreateDelegate(typeof(LoadIconDlg), target: null);
            }
        }
    }
}
