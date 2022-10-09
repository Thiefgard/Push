using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoins : UIElement
{
    public static UICoins Instance;

    [SerializeField] private Text _textCoinsCnt;

    int coins = 0;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();

        UpdateText();
        coins = GameManager.Instance.Coins;
    }

    public void UpdateText()
    {
        _textCoinsCnt.text = GameManager.Instance.Coins.ToString();
    }

    public void ShowCoins(int value)
    {
        coins += value;
        _textCoinsCnt.text = coins.ToString();
    }
}
