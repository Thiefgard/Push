using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIElement
{
    [SerializeField] private List<int> _listPrices;

    [SerializeField] private Button _button;
    [SerializeField] private Text _textCurUpgrade;
    [SerializeField] private GameObject _objCanUpgrade;
    [SerializeField] private GameObject _btnGray;
    [SerializeField] private Text _textPrice;
    [SerializeField] private GameObject _objMax;

    private int _maxUpgrade;

    private void Awake()
    {
        _maxUpgrade = _listPrices.Count;
    }

    public override void Show()
    {
        UpdateVisual();

        base.Show();
    }

    private void UpdateVisual()
    {
        int curUpgrade = GameManager.Instance.UpgradeElement;
        _textCurUpgrade.text = curUpgrade.ToString() + "/" + _maxUpgrade.ToString();

        if (curUpgrade == _maxUpgrade)
        {
            _objMax.SetActive(true);
            _objCanUpgrade.SetActive(false);

            _button.interactable = false;
        }
        else
        {
            int price = CalculateUpgradePrice();
            _textPrice.text = price.ToString();

            if (GameManager.Instance.Coins >= price)
            {
                _btnGray.SetActive(false);
                _objCanUpgrade.SetActive(true);

                _button.interactable = true;
            }
            else
            {
                _btnGray.SetActive(true);
                _objCanUpgrade.SetActive(false);

                _button.interactable = false;
            }
        }
    }

    private int CalculateUpgradePrice()
    {
        int curUpgrade = GameManager.Instance.UpgradeElement;
        if (GameManager.Instance.UpgradeElement < _maxUpgrade)
        {
            return _listPrices[curUpgrade];
        }
        return _listPrices[_maxUpgrade - 1];
    }

    public void Upgrade()
    {
        if(GameManager.Instance.UpgradeElement < _maxUpgrade)
        {
            int price = CalculateUpgradePrice();
            if (GameManager.Instance.Coins >= price)
            {
                GameManager.Instance.AddCoins(-price);
                UICoins.Instance.UpdateText();

                GameManager.Instance.UpgradeElement++;

                UpdateVisual();
            }
        }
    }
}
