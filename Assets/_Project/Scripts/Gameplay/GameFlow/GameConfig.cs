using UnityEngine;

namespace gishadev.eclipse.Gameplay.GameFlow
{
    [CreateAssetMenu(menuName = "gishadev.eclipse/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Tooltip("Round length in seconds. Lose when it runs out before the vehicle is stripped.")]
        [SerializeField, Min(1f)] private float roundDuration = 60f;

        public float RoundDuration => roundDuration;
    }
}
