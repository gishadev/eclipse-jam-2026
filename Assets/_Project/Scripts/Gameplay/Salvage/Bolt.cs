using UnityEngine;

namespace gishadev.eclipse.Gameplay.Salvage
{
    /// <summary>
    /// Holds part. Pulled out along <see cref="PullDirection"/> when unscrewed.
    /// </summary>
    public class Bolt : MonoBehaviour
    {
        [Tooltip("Pull-out direction in the bolt's local space (follows the bolt when rotated). Shown as a gizmo arrow.")]
        [SerializeField] private Vector3 pullDirection = Vector3.forward;

        /// <summary>World-space, normalized. Falls back to local +Z if the field is zero.</summary>
        public Vector3 PullDirection =>
            transform.TransformDirection(pullDirection == Vector3.zero ? Vector3.forward : pullDirection).normalized;

#if UNITY_EDITOR
        private const float GizmoLength = 0.3f;
        private const float GizmoHeadSize = 0.08f;

        private void OnDrawGizmos()
        {
            Vector3 dir = PullDirection;
            Vector3 from = transform.position;
            Vector3 to = from + dir * GizmoLength;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(from, to);

            // Arrow head: 4 short lines back from the tip, around the direction.
            Vector3 side = Vector3.Cross(dir, Mathf.Abs(Vector3.Dot(dir, Vector3.up)) > 0.99f ? Vector3.right : Vector3.up)
                .normalized;
            Vector3 up = Vector3.Cross(dir, side);
            Vector3 back = to - dir * GizmoHeadSize;
            Gizmos.DrawLine(to, back + side * GizmoHeadSize * 0.5f);
            Gizmos.DrawLine(to, back - side * GizmoHeadSize * 0.5f);
            Gizmos.DrawLine(to, back + up * GizmoHeadSize * 0.5f);
            Gizmos.DrawLine(to, back - up * GizmoHeadSize * 0.5f);
        }
#endif
    }
}
