using UnityEngine;

namespace EclipseJam.Gameplay.OrbitalCamera
{
    [CreateAssetMenu(menuName = "EclipseJam/Orbital Camera Config", fileName = "OrbitalCameraConfig")]
    public class OrbitalCameraConfig : ScriptableObject
    {
        [Tooltip("Degrees per second around the rig (A/D).")]
        [SerializeField] private float horizontalSpeed = 90f;

        [Tooltip("Degrees per second up/down (W/S).")]
        [SerializeField] private float verticalSpeed = 60f;

        [SerializeField] private bool invertHorizontal;
        [SerializeField] private bool invertVertical;

        [Tooltip("Zoom rate (R/F). 1 = distance changes by a factor of e per second; limits come from RadialAxis range.")]
        [SerializeField] private float zoomSpeed = 1f;

        [SerializeField] private bool invertZoom;

        [Header("Mouse")]
        [Tooltip("Degrees per pixel of mouse movement while RMB is held.")]
        [SerializeField] private float mouseOrbitSensitivity = 0.2f;

        [SerializeField] private bool invertMouseHorizontal;
        [SerializeField] private bool invertMouseVertical;

        [Tooltip("Zoom per scroll notch. 0.1 ≈ 10% distance change per notch.")]
        [SerializeField] private float scrollZoomStep = 0.1f;

        [Tooltip("Off: scroll up zooms in.")]
        [SerializeField] private bool invertScroll;

        public float HorizontalSpeed => horizontalSpeed;
        public float VerticalSpeed => verticalSpeed;
        public bool InvertHorizontal => invertHorizontal;
        public bool InvertVertical => invertVertical;
        public float ZoomSpeed => zoomSpeed;
        public bool InvertZoom => invertZoom;
        public float MouseOrbitSensitivity => mouseOrbitSensitivity;
        public bool InvertMouseHorizontal => invertMouseHorizontal;
        public bool InvertMouseVertical => invertMouseVertical;
        public float ScrollZoomStep => scrollZoomStep;
        public bool InvertScroll => invertScroll;
    }
}
