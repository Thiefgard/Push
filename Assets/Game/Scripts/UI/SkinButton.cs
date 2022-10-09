using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private SkinInfo _skinInfo;
    [SerializeField] private Image _image;
    [SerializeField] private int _index;
    [SerializeField] private GameObject _pressedImage;

    public GameObject PressedImage => _pressedImage;

    public int Index
    {
        get => _index;
        set => _index = value;
    }


    public SkinInfo SkinInfo
    {
        get => _skinInfo;
        set => _skinInfo = value;
    }

    private void Start()
    {
        ChangeSprite();
    }
    private void Update()
    {
        if(_index == UIShop.Instance.SkinIndex)
        {
            if (!_pressedImage.activeInHierarchy)
            {
                _pressedImage.SetActive(true);
            }
        }
    }

    public void ChooseSkin()
    {
        if (_skinInfo.isOpen)
        {
            UIShop.Instance.AllSkins[UIShop.Instance.SkinIndex].PressedImage.SetActive(false);
            Player.Instance.SetSkin(SkinInfo.Skin);
            _pressedImage.SetActive(true);
            UIShop.Instance.SkinIndex = _index;
        }
    }

    public void ChangeSprite()
    {
        if (_skinInfo.Sprite != null && _skinInfo.isOpen)
        {
            _image.sprite = _skinInfo.Sprite;
        }
    }

    public void Open()
    {
        _skinInfo.isOpen = true;
        if (_skinInfo.Sprite != null && _skinInfo.isOpen)
        {
            _image.sprite = _skinInfo.Sprite;
        }
    }
}
