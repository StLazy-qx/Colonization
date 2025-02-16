using UnityEngine;
using TMPro;

public class ViewWalletBalance : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _balanceText;

    private void OnEnable()
    {
        _wallet.BalanceChanged += OnValueChange;
    }

    private void OnDisable()
    {
        _wallet.BalanceChanged -= OnValueChange;
    }

    private void OnValueChange(int value)
    {
        _balanceText.text = $"{value}";
    }
}
