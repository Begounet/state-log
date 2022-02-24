using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace StateLog
{
    public static class StackTraceUtils
    {
        private delegate string StacktraceWithHyperlinksDlg(string stackTraceText, int callstackTextStart);

        private static StacktraceWithHyperlinksDlg _stacktraceWithHyperlinksFunc;

        public static string StackTraceWithHyperlink(string stackTrace, int callstackTextStart = 0)
        {
            CacheStackTraceWithHyperLinkMethodIFN();
            return _stacktraceWithHyperlinksFunc.Invoke(stackTrace, callstackTextStart);
        }

        private static void CacheStackTraceWithHyperLinkMethodIFN()
        {
            if (_stacktraceWithHyperlinksFunc == null)
            {
                var stacktraceWithHyperlinksMI = typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow").GetMethod("StacktraceWithHyperlinks", BindingFlags.NonPublic | BindingFlags.Static);
                _stacktraceWithHyperlinksFunc = (StacktraceWithHyperlinksDlg)stacktraceWithHyperlinksMI.CreateDelegate(typeof(StacktraceWithHyperlinksDlg), null);
            }
        }
    }
}
