using UnityEngine;
using LatticeComponent = Lattice.Lattice;
using LatticeItem = Lattice.LatticeItem;
using LatticeModifier = Lattice.LatticeModifier;

namespace gishadev.eclipse.Visuals
{
    /// <summary>
    /// On Awake adds a <c>LatticeModifier</c> to every child with a MeshFilter + MeshRenderer (inactive included)
    /// and links it to <see cref="lattice"/>. Existing modifiers just get the lattice linked.
    /// <para>
    /// Without compute shaders (WebGL) modifiers can't run, so this object is squashed instead: every frame the
    /// lattice handles' bounding box (still animated by the Animator) is applied as scale + position, anchored
    /// to the lattice. Assumes this object's axes are aligned with the lattice's.
    /// </para>
    /// </summary>
    public class LatticeModifierChildrenHandler : MonoBehaviour
    {
        [SerializeField] private LatticeComponent lattice;

        private bool _useSquashFallback;
        private Vector3 _restLocalScale;
        private Vector3 _restPositionInLattice;

        private void Awake()
        {
            if (lattice == null)
            {
                Debug.LogWarning($"{nameof(LatticeModifierChildrenHandler)}: no Lattice assigned.", this);
                return;
            }

            // Lattice deforms on the GPU via compute shaders + raw GraphicsBuffers. WebGL has neither, and a
            // modifier still allocates those buffers in OnEnable — which crashes the wasm build natively.
            if (!SystemInfo.supportsComputeShaders)
            {
                _useSquashFallback = true;
                _restLocalScale = transform.localScale;
                _restPositionInLattice = lattice.transform.InverseTransformPoint(transform.position);
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

        private void LateUpdate()
        {
            if (!_useSquashFallback)
                return;

            // Deformed box of the lattice in its local space; the undeformed box is -0.5..0.5 on every axis.
            Vector3Int res = lattice.Resolution;
            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;
            for (int x = 0; x < res.x; x++)
            for (int y = 0; y < res.y; y++)
            for (int z = 0; z < res.z; z++)
            {
                Vector3 p = lattice.GetHandlePosition(x, y, z);
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }

            Vector3 scale = max - min; // undeformed size is 1 → this is the per-axis squash factor
            Vector3 position = min + Vector3.Scale(_restPositionInLattice + Vector3.one * 0.5f, scale);

            transform.localScale = Vector3.Scale(_restLocalScale, scale);
            transform.position = lattice.transform.TransformPoint(position);
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
