using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skin/NewSkin", fileName = "NewSkin")]
public class SkinInfo : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _skin;

    public bool isOpen;

    public string Name => _name;

    public Sprite Sprite => _sprite;

    public GameObject Skin => _skin;
}
