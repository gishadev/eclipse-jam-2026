using UnityEngine;

namespace gishadev.eclipse.Gameplay.Salvage
{
    /// <summary>
    /// Can be salvaged only if all bolts are unscrewed.
    /// </summary>
    public class Part : MonoBehaviour
    {
        [SerializeField] private Bolt[] bolts;
        [SerializeField] private int salvagePrice;

        public Bolt[] Bolts => bolts;
        public int SalvagePrice => salvagePrice;

        /// <summary>True when no bolt is holding the part anymore (unscrewed bolts are destroyed → null).</summary>
        public bool IsLoose
        {
            get
            {
                foreach (Bolt bolt in bolts)
                    if (bolt != null)
                        return false;
                return true;
            }
        }
    }
}
