// using System;
// using GameBrains.Extensions.Attributes;
// using UnityEditor;
// using UnityEngine;
//
// namespace GameBrains.Editor.PropertyDrawers
// {
//     /// <summary>
//     /// Note: Doesn't work on arrays or lists because of how Unity works.
//     /// It can hide the elements of the array, but not the array property itself.
//     /// </summary>
//     [CustomPropertyDrawer(typeof(ConditionalPropertyAttribute))]
//     public class ConditionalPropertyDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             bool isVisible = IsVisible(property);
//             if (isVisible)
//             {
//                 EditorGUI.BeginProperty(position, label, property);
//                 EditorGUI.PropertyField(position, property, label, true);
//                 EditorGUI.EndProperty();
//             }
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             // Make the height 0 if we aren't drawing the property,
//             // otherwise a blank space appears in the inspector
//             bool isVisible = IsVisible(property);
//             float height = -2;
//             if (isVisible)
//             {
//                 height = EditorGUI.GetPropertyHeight(property);
//             }
//
//             return height;
//         }
//
//         private bool IsVisible(SerializedProperty property)
//         {
//             // Extract info from the attribute
//             var conditionalPropertyAttribute = attribute as ConditionalPropertyAttribute;
//             if (conditionalPropertyAttribute == null)
//             {
//                 Debug.LogError(
//                     "ConditionPropertyAttributeDrawer couldn't cast attribute to ConditionalPropertyAttribute");
//             }
//
//             string propertyToCheckName = conditionalPropertyAttribute.propertyToCheck;
//             string requiredValue = "null";
//             if (conditionalPropertyAttribute.compareValue != null)
//             {
//                 requiredValue = conditionalPropertyAttribute.compareValue.ToString();
//             }
//
//             // Default to visible if the logic isn't working out (properties can't be found, etc)
//             bool isVisible = true;
//
//             // Determine if we need to hide the property
//             if (!string.IsNullOrEmpty(propertyToCheckName))
//             {
//                 // Make sure we can find the property it depends on
//                 var propertyToCheck = property.serializedObject.FindProperty(propertyToCheckName);
//                 if (propertyToCheck != null)
//                 {
//                     string actualValue = AsStringValue(propertyToCheck);
//                     // See if the values match
//                     isVisible =
//                         string.Equals(
//                             requiredValue,
//                             actualValue,
//                             StringComparison.OrdinalIgnoreCase);
//                 }
//             }
//
//             return isVisible;
//         }
//
//         string AsStringValue(SerializedProperty prop)
//         {
//             switch (prop.propertyType)
//             {
//                 case SerializedPropertyType.String:
//                     return prop.stringValue;
//
//                 case SerializedPropertyType.Character:
//                 case SerializedPropertyType.Integer:
//                     if (prop.type == "char") return Convert.ToChar(prop.intValue).ToString();
//                     return prop.intValue.ToString();
//
//                 case SerializedPropertyType.ObjectReference:
//                     return prop.objectReferenceValue != null
//                         ? prop.objectReferenceValue.ToString()
//                         : "null";
//
//                 case SerializedPropertyType.Boolean:
//                     return prop.boolValue.ToString();
//
//                 case SerializedPropertyType.Enum:
//                     return prop.enumNames[prop.enumValueIndex];
//
//                 default:
//                     return string.Empty;
//             }
//         }
//     }
// }