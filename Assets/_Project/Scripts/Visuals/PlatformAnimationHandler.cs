using UnityEngine;

namespace gishadev.eclipse.Visuals
{
    /// <summary>Passive view over the platform Animator. Driven by the game controller.</summary>
    public class PlatformAnimationHandler : MonoBehaviour
    {
        private static readonly int CrushTrigger = Animator.StringToHash("Crush");

        [SerializeField] private Animator animator;

        public void PlayCrush() => animator.SetTrigger(CrushTrigger);

        private void Reset() => animator = GetComponent<Animator>();
    }
}
