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

        public float HorizontalSpeed => horizontalSpeed;
        public float VerticalSpeed => verticalSpeed;
        public bool InvertHorizontal => invertHorizontal;
        public bool InvertVertical => invertVertical;
        public float ZoomSpeed => zoomSpeed;
        public bool InvertZoom => invertZoom;
    }
}
