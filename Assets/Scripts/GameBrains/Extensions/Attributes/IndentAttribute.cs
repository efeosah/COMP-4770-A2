using UnityEditor;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class IndentAttribute : MultiPropertyAttribute
    {
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel++;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel--;
        }
    }
}