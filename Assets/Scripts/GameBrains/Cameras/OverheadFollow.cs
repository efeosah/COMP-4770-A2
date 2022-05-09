using UnityEngine;

namespace GameBrains.Cameras
{
    [AddComponentMenu("Scripts/GameBrains/Cameras/Overhead Follow Camera")]
    [RequireComponent(typeof(Camera))]
    public class OverheadFollow : TargetedCamera
    {
        [SerializeField] Camera overheadCamera;
        //TODO-3: Convert to new Input System
        //TODO: Make SelectableZoomCamera as a base class??
        [SerializeField] string zoomAxis = "Mouse ScrollWheel";
        [Range(0.01f, 0.5f)]
        [SerializeField] float zoomSpeed = 0.01f;
        float maximumZoomOut;

        public override void Awake()
        {
            base.Awake();
            maximumZoomOut = overheadCamera.orthographicSize;
        }
        public override void LateUpdate()
        {
            base.LateUpdate();

            var zoomDirection = Input.GetAxis(zoomAxis);
            var size = overheadCamera.orthographicSize;

            if (zoomDirection < 0f) { size /= 1f - zoomSpeed * zoomDirection; }
            else if (zoomDirection > 0f) { size *= 1f + zoomSpeed * zoomDirection; }

            overheadCamera.orthographicSize = Mathf.Clamp(size, 1f, maximumZoomOut);

            var cachedTransform = overheadCamera.transform;
            Vector3 position = Target.position;
            position.y = cachedTransform.position.y;
            cachedTransform.position = position;

            RaiseOnUpdated();
        }
    }
}