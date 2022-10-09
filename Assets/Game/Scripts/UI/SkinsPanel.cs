using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsPanel : MonoBehaviour
{
    public static SkinsPanel Instance;

    [SerializeField] private List<SkinInfo> _skinsInfoList = new List<SkinInfo>();
    [SerializeField] private SkinButton _skinButtonPrefab;
    [SerializeField] private List<SkinButton> _closeSkins = new List<SkinButton>();

    private void Awake()
    {
        Instance = this;
    }

    public virtual void OnStart()
    {
        if(UIShop.Instance.AllSkins.Count <= 0)
        {
            for (int i = 0; i < _skinsInfoList.Count; i++)
            {
                SkinButton skinButton = Instantiate(_skinButtonPrefab, transform);
                skinButton.SkinInfo = _skinsInfoList[i];
                skinButton.Index = i;
                if (i == UIShop.Instance.SkinIndex)
                {
                    skinButton.PressedImage.SetActive(true);
                }
                if (!_skinsInfoList[i].isOpen)
                {
                    _closeSkins.Add(skinButton);
                }
                UIShop.Instance.AllSkins.Add(skinButton);
            }
        }
        Player.Instance.SetSkin(UIShop.Instance.AllSkins[UIShop.Instance.SkinIndex].SkinInfo.Skin);
    }

    public void OpenSkin()
    {
        if(_closeSkins.Count > 0)
        {
            if (GameManager.Instance.Coins >= UIShop.Instance._skinAmount)
            {
                GameManager.Instance.AddCoins(-UIShop.Instance._skinAmount);
            }
            StartCoroutine(IOpenSkin());
        }
    }

    private IEnumerator IOpenSkin()
    {
        System.Random rand = new System.Random();
        for (int i = _closeSkins.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);

            SkinButton tmp = _closeSkins[j];
            _closeSkins[j] = _closeSkins[i];
            _closeSkins[i] = tmp;
        }
        for (int i = 0; i < _closeSkins.Count; i++)
        {
            _closeSkins[i].PressedImage.SetActive(true);
            yield return new WaitForSeconds(.5f);
            if(i == _closeSkins.Count - 1)
            {
                UIShop.Instance.AllSkins[UIShop.Instance.SkinIndex].PressedImage.SetActive(false);
                _closeSkins[i].Open();
                _closeSkins[i].PressedImage.SetActive(true);
                UIShop.Instance.SkinIndex = _closeSkins[i].Index;
                Player.Instance.SetSkin(_closeSkins[i].SkinInfo.Skin);
                _closeSkins.RemoveAt(i);
            }
            else
            {
                _closeSkins[i].PressedImage.SetActive(false);
            }
        }

    }
}
