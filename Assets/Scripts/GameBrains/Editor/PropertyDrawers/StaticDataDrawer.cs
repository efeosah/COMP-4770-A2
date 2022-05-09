using GameBrains.Editor.Extensions;
using GameBrains.Editor.PropertyDrawers.Utilities;
using GameBrains.Entities.EntityData;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(StaticData), true)]
    public class StaticDataDrawer : PropertyDrawer
    {
        StaticData staticData;
        bool showEntityData = true;
        bool showStaticInfo;
        bool showKinematicInfo;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            staticData =
                PropertyDrawerUtilities.GetActualObjectForSerializedProperty<StaticData>(
                    fieldInfo, property);

            if (staticData == null || !staticData.WrappedTransform)
            {
                return;
            }

            //showPhysicsData = EditorGUILayout.Foldout(showPhysicsData, property.displayName);
            showEntityData = EditorGUILayout.Foldout(showEntityData, "Entity Data");

            if (!showEntityData) return;

            #region Header Foldouts

            EditorGUI.indentLevel += 1;

            showStaticInfo = EditorGUILayout.Foldout(showStaticInfo, nameof(StaticData));
            if (showStaticInfo) DrawStaticData(staticData);

            if (staticData is KinematicData kinematicData)
            {
                showKinematicInfo
                    = EditorGUILayout.Foldout(showKinematicInfo, nameof(KinematicData));
                if (showKinematicInfo) DrawKinematicData(kinematicData);
            }

            EditorGUI.indentLevel -= 1;

            #endregion Header Foldouts

            // Constantly updates inspector but grinds
            // if (EditorApplication.isPlaying)
            // {
            //     EditorUtility.SetDirty(property.serializedObject.targetObject); 
            // }
        }

        #region Draw Static Data

        static void DrawStaticData(StaticData staticData)
        {
            EditorGUI.indentLevel += 1;

            staticData.ObstacleLayerMask
                = EditorGUILayoutExtensions.LayerMaskField(
                    "ObstacleLayerMask",
                    staticData.ObstacleLayerMask);

            staticData.Location =
                EditorGUILayoutExtensions.VectorXZField("Location", staticData.Location);

            staticData.Orientation
                = EditorGUILayout.FloatField("Orientation", staticData.Orientation);

            EditorGUILayoutExtensions.VectorXZField("Heading", staticData.HeadingVectorXZ);

            staticData.Radius = EditorGUILayout.FloatField("Radius", staticData.Radius);

            staticData.Height = EditorGUILayout.FloatField("Height", staticData.Height);

            staticData.CenterOffset
                = EditorGUILayoutExtensions.VectorXYZField(
                    "CenterOffset",
                    staticData.CenterOffset);

            EditorGUILayoutExtensions.VectorXYZField(
                "Top",
                staticData.Top);

            EditorGUILayoutExtensions.VectorXYZField(
                "Bottom",
                staticData.Bottom);

            EditorGUILayoutExtensions.VectorXYZField(
                "Center",
                staticData.Center);

            staticData.CloseEnoughDistance
                = EditorGUILayout.FloatField(
                    "CloseEnoughDistance",
                    staticData.CloseEnoughDistance);

            staticData.FarEnoughDistance
                = EditorGUILayout.FloatField(
                    "FarEnoughDistance",
                    staticData.FarEnoughDistance);

            staticData.CloseEnoughAngle
                = EditorGUILayout.FloatField(
                    "CloseEnoughAngle",
                    staticData.CloseEnoughAngle);

            staticData.FarEnoughAngle
                = EditorGUILayout.FloatField(
                    "FarEnoughAngle",
                    staticData.FarEnoughAngle);

            staticData.ClearColor
                = EditorGUILayout.ColorField("ClearColor", staticData.ClearColor);

            staticData.BlockedColor
                = EditorGUILayout.ColorField("BlockedColor", staticData.BlockedColor);

            EditorGUI.indentLevel -= 1;
        }

        #endregion Draw Static Data

        #region Draw Kinematic Data

        void DrawKinematicData(KinematicData kinematicData)
        {
            EditorGUI.indentLevel += 1;

            kinematicData.Velocity =
                EditorGUILayoutExtensions.VectorXZField(new GUIContent("Velocity"),
                    kinematicData.Velocity);

            kinematicData.AngularVelocity
                = EditorGUILayout.FloatField(
                    "AngularVelocity",
                    kinematicData.AngularVelocity);

            kinematicData.Acceleration =
                EditorGUILayoutExtensions.VectorXZField(new GUIContent("Acceleration"),
                    kinematicData.Acceleration);

            kinematicData.AngularAcceleration
                = EditorGUILayout.FloatField(
                    "AngularAcceleration",
                    kinematicData.AngularAcceleration);

            EditorGUILayout.FloatField("Speed", kinematicData.Speed);

            kinematicData.MaximumSpeed
                = EditorGUILayout.FloatField("MaximumSpeed", kinematicData.MaximumSpeed);

            kinematicData.MaximumAngularSpeed
                = EditorGUILayout.FloatField(
                    "MaximumAngularSpeed",
                    kinematicData.MaximumAngularSpeed);

            kinematicData.MaximumAcceleration
                = EditorGUILayout.FloatField(
                    "MaximumAcceleration",
                    kinematicData.MaximumAcceleration);

            kinematicData.MaximumAngularAcceleration
                = EditorGUILayout.FloatField(
                    "MaximumAngularAcceleration",
                    kinematicData.MaximumAngularAcceleration);

            EditorGUI.indentLevel -= 1;
        }

        #endregion Draw Kinematic Data

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}