using UnityEngine;
using LatticeComponent = Lattice.Lattice;
using LatticeItem = Lattice.LatticeItem;
using LatticeModifier = Lattice.LatticeModifier;

namespace gishadev.eclipse.Visuals
{
    /// <summary>
    /// On Awake adds a <c>LatticeModifier</c> to every child with a MeshFilter + MeshRenderer (inactive included)
    /// and links it to <see cref="lattice"/>. Existing modifiers just get the lattice linked.
    /// </summary>
    public class LatticeModifierChildrenHandler : MonoBehaviour
    {
        [SerializeField] private LatticeComponent lattice;

        private void Awake()
        {
            if (lattice == null)
            {
                Debug.LogWarning($"{nameof(LatticeModifierChildrenHandler)}: no Lattice assigned.", this);
                return;
            }

            // LatticeModifier requires both, so objects with only a MeshRenderer are skipped.
            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>(true))
            {
                if (!meshRenderer.TryGetComponent(out MeshFilter _))
                    continue;

                if (!meshRenderer.TryGetComponent(out LatticeModifier modifier))
                    modifier = meshRenderer.gameObject.AddComponent<LatticeModifier>();

                Link(modifier);
            }
        }

        // A new modifier comes with one empty slot (default mask settings) — fill it, else append.
        private void Link(LatticeModifier modifier)
        {
            var items = modifier.Lattices;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Lattice == lattice)
                    return;

                if (items[i].Lattice == null)
                {
                    var item = items[i]; // struct: copy, edit, write back
                    item.Lattice = lattice;
                    items[i] = item;
                    modifier.RequestUpdate();
                    return;
                }
            }

            items.Add(new LatticeItem { Lattice = lattice, Mask = { Vertex = { Multiplier = 1f } } });
            modifier.RequestUpdate();
        }
    }
}
