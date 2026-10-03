using TMPro;
using UnityEngine;

namespace gishadev.eclipse.GUI
{
    /// <summary>Passive view: only displays a value. Driven by <see cref="MoneyPresenter"/>.</summary>
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyTMP;

        public void SetMoney(int money) => moneyTMP.text = $"$ {money}";
    }
}
