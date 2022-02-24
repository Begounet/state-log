using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace StateLog
{
    public class EditorSelectableLabel : VisualElement, INotifyValueChanged<string>
    {
        public new class UxmlFactory : UxmlFactory<EditorSelectableLabel, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription _text = new UxmlStringAttributeDescription { name = "text" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription { get { yield break; } }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((EditorSelectableLabel)ve).text = _text.GetValueFromBag(bag, cc);
            }
        }

        [SerializeField]
        private string _text = string.Empty;
        public string text
        {
            get => ((INotifyValueChanged<string>)this).value;
            set
            {
                ((INotifyValueChanged<string>)this).value = value;
            }
        }

        string INotifyValueChanged<string>.value
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    if (panel != null)
                    {
                        using (ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(_text, value))
                        {
                            evt.target = this;
                            ((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
                            SendEvent(evt);
                        }
                    }
                    else
                    {
                        ((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
                    }
                }
            }
        }

        public GUIStyle MessageStyle;

        public EditorSelectableLabel()
        {
            var container = new IMGUIContainer(() =>
            {
                if (MessageStyle == null)
                {
                    MessageStyle = new GUIStyle("CN Message");
                }

                var content = UnsafeGenericPool<GUIContent>.Get();
                content.text = text;
                content.tooltip = null;
                content.image = null;

                MessageStyle.wordWrap = (style.flexWrap.value == Wrap.Wrap);

                Vector2 size = MessageStyle.CalcSize(content);
                try
                {
                    EditorGUILayout.SelectableLabel(text, MessageStyle, GUILayout.Width(size.x + 10), GUILayout.Height(size.y + 10), GUILayout.ExpandHeight(true));
                }
                catch (Exception) { }
            });
            Add(container);
        }

        public void SetValueWithoutNotify(string newValue)
        {
            if (_text != newValue)
            {
                _text = newValue;
            }
        }
    }
}
