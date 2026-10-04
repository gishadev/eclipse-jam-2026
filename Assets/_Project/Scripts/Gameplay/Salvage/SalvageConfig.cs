using PrimeTween;
using UnityEngine;

namespace gishadev.eclipse.Gameplay.Salvage
{
    [CreateAssetMenu(menuName = "gishadev.eclipse/Salvage Config", fileName = "SalvageConfig")]
    public class SalvageConfig : ScriptableObject
    {
        [Header("Bolt — unscrew → shake → pull")]
        [Tooltip("Spin time around the bolt axis (local Z).")]
        [SerializeField, Min(0.01f)] private float boltUnscrewDuration = 0.6f;
        [Tooltip("Full turns while unscrewing.")]
        [SerializeField] private float boltTurns = 3f;
        [SerializeField, Min(0.01f)] private float boltShakeDuration = 0.15f;
        [SerializeField] private float boltShakeStrength = 0.02f;
        [SerializeField] private float boltShakeFrequency = 30f;
        [SerializeField, Min(0.01f)] private float boltPullDuration = 0.2f;
        [Tooltip("How far the bolt is pulled out along its axis, in world units.")]
        [SerializeField] private float boltPullDistance = 0.3f;
        [SerializeField] private Ease boltPullEase = Ease.OutCubic;

        [Header("Part — strip out")]
        [SerializeField, Min(0.01f)] private float partShakeDuration = 0.25f;
        [SerializeField] private float partShakeStrength = 0.05f;
        [SerializeField] private float partShakeFrequency = 25f;
        [SerializeField, Min(0.01f)] private float partYankDuration = 0.2f;
        [Tooltip("How far the part flies off along the clicked surface normal, in world units.")]
        [SerializeField] private float partYankDistance = 1.5f;
        [SerializeField] private Ease partYankEase = Ease.OutCubic;

        [Header("Part — loose wobble (no bolts left, endless)")]
        [Tooltip("Max rotation offset in degrees.")]
        [SerializeField] private float looseWobbleStrength = 1.5f;
        [SerializeField] private float looseWobbleFrequency = 8f;
        [Tooltip("Length of one shake cycle before it repeats.")]
        [SerializeField, Min(0.05f)] private float looseWobbleCycleDuration = 1f;

        public float BoltUnscrewDuration => boltUnscrewDuration;
        public float BoltTurns => boltTurns;
        public float BoltShakeDuration => boltShakeDuration;
        public float BoltShakeStrength => boltShakeStrength;
        public float BoltShakeFrequency => boltShakeFrequency;
        public float BoltPullDuration => boltPullDuration;
        public float BoltPullDistance => boltPullDistance;
        public Ease BoltPullEase => boltPullEase;

        public float PartShakeDuration => partShakeDuration;
        public float PartShakeStrength => partShakeStrength;
        public float PartShakeFrequency => partShakeFrequency;
        public float PartYankDuration => partYankDuration;
        public float PartYankDistance => partYankDistance;
        public Ease PartYankEase => partYankEase;

        public float LooseWobbleStrength => looseWobbleStrength;
        public float LooseWobbleFrequency => looseWobbleFrequency;
        public float LooseWobbleCycleDuration => looseWobbleCycleDuration;
    }
}
