using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vaveda.Integration.Scripts;
using Vaveda.Integration.Scripts.Fads;

public class UIShop : UIElement
{
    public static UIShop Instance;

    private List<SkinButton> _allSkins = new List<SkinButton>();

    public List<SkinButton> AllSkins => _allSkins;

    [SerializeField] private SkinsPanel _skinsPanel;
    [SerializeField] private WeaponsPanel _weaponsPanel;
    public int _skinAmount = 2000;
    [SerializeField] private Button _openSkinButton;
    [SerializeField] private Text _skinButtonOpenText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _skinButtonOpenText.text = _skinAmount.ToString();
    }
    public override void Show()
    {
        base.Show();
        _skinsPanel.OnStart();
    }

    private void Update()
    {
        if(GameManager.Instance.Coins < _skinAmount)
        {
            _openSkinButton.interactable = false;
        }
        else
        {
            _openSkinButton.interactable = true;
        }
    }

    public int SkinIndex
    {
        get => PlayerPrefs.GetInt("SkinIndex", 0);
        set => PlayerPrefs.SetInt("SkinIndex", value);
    }

    public void CloseShop()
    {
        UIManager.Instance.ChangeState(UIState.Start);
    }

    public GameObject GetActiveSkin()
    {
        return _allSkins[SkinIndex].SkinInfo.Skin;
    }

    public void OnOpenSkin()
    {
        _skinsPanel.OpenSkin();
        _skinAmount += 500;
        _skinButtonOpenText.text = _skinAmount.ToString();
    }

    public void OnAddCoinReward()
    {
        if (Services.Instance.FadsService is { HasRewardedVideo: true })
        {
            Services.Instance.FadsService.RewardedShouldReward += AddCoinReward;
            Services.Instance.FadsService.ShowRewardedVideo(Placements.PLACEMENT_REWARDED_LEVEL_GETHINTS);
        }
    }

    private void AddCoinReward()
    {
        Services.Instance.FadsService.RewardedShouldReward -= AddCoinReward;
        GameManager.Instance.AddCoins(500);
        UICoins.Instance.UpdateText();
    }

    public void OpenSkinsPanel()
    {
        print("Open Skin");
        _skinsPanel.gameObject.SetActive(true);
        //_weaponsPanel.gameObject.SetActive(false);
    }
    public void OpenWeaponsPanel()
    {
        _skinsPanel.gameObject.SetActive(false);
        _weaponsPanel.gameObject.SetActive(true);
    }
}
