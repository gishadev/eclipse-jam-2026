using UnityEngine;

namespace gishadev.eclipse.Gameplay.Salvage
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private Part[] parts;

        public Part[] Parts => parts;
    }
}
