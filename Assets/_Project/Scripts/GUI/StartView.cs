using UnityEngine;

namespace gishadev.eclipse.GUI
{
    /// <summary>Passive view over the "Press To Start" popup. Driven by <see cref="StartPresenter"/>.</summary>
    public class StartView : MonoBehaviour
    {
        [SerializeField] private GameObject startPopup;

        public void SetVisible(bool visible) => startPopup.SetActive(visible);
    }
}
