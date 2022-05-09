using UnityEditor;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ColorAttribute : MultiPropertyAttribute
    {
        readonly Color color;
        public Color originalColor;
        public ColorAttribute(float r, float g, float b)
        {
            color = new Color(r, g, b);
        }

        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            originalColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.color = originalColor;
        }
    }
}