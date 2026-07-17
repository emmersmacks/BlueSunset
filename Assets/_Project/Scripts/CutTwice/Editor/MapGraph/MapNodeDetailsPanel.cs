using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CutTwice.Editor.MapGraph
{
    public class MapNodeDetailsPanel : VisualElement
    {
        public MapNodeDetailsPanel()
        {
            style.paddingLeft = 6;
            style.paddingRight = 6;
            style.paddingTop = 6;

            Bind(null);
        }

        public void Bind(SerializedProperty property)
        {
            Clear();

            var header = new Label
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 4
                }
            };
            Add(header);

            if (property == null)
            {
                header.text = "Nothing selected";
                return;
            }

            header.text = property.displayName;

            var field = new PropertyField(property, string.Empty);
            field.BindProperty(property);
            Add(field);
        }
    }
}
