using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelsContainer _levelsContainer;

    [SerializeField] private int _coinsAddByLvl;

    private Level _curLevel;
    private bool _isInAP;

    public int LevelNum
    {
        get => PlayerPrefs.GetInt("Level", 0);
        set => PlayerPrefs.SetInt("Level", value);
    }

    public bool HapticEnabled
    {
        get => PlayerPrefs.GetInt("HapticEnabled", 1) == 1;
        set => PlayerPrefs.SetInt("HapticEnabled", value == true ? 1 : 0);
    }

    public int Coins
    {
        get => PlayerPrefs.GetInt("Coins", 0);
        set => PlayerPrefs.SetInt("Coins", value);
    }

    public int UpgradeElement
    {
        get => PlayerPrefs.GetInt("UpgradeElement", 0);
        set => PlayerPrefs.SetInt("UpgradeElement", value);
    }

    public int GetCoinsAddByLvl() { return _coinsAddByLvl; }

    private void Awake()
    {
        Instance = this;

        _isInAP = false;
        _curLevel = null;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(_curLevel != null)
        {
            Destroy(_curLevel.gameObject);
        }

        if(_levelsContainer != null)
        {
            Level level = _levelsContainer.GetLevelPrefab(LevelNum);
            if (level)
            {
                _curLevel = Instantiate(level);
            }
        }

        CameraMovement.Instance.Init();

        UIManager.Instance.ChangeState(UIState.Start);
        //ADSManager.Instance.ShowSubscription();
    }

    public void OnLevelStarted()
    {
        _isInAP = true;

        

        PlayHaptic(HapticTypes.MediumImpact);

        Player.Instance.isInBoundery = true;
        ADSManager.Instance.ShowBunner();
        ADSManager.Instance.AnalyticsStartLevel(_curLevel.name, LevelNum + 1);
    }

    public void OnLevelFinished(bool success)
    {
        ADSManager.Instance.AnalyticsEndLevel(_curLevel.name, LevelNum + 1, success);
        if (success)
        {
            UIManager.Instance.ChangeState(UIState.Finish);

            LevelNum++;

            AddCoins(_coinsAddByLvl);
        }
        else
        {
            UIManager.Instance.ChangeState(UIState.Lose);
        }
       
    }

    public void PlayHaptic(HapticTypes type)
    {
        if (HapticEnabled)
        {
            MMVibrationManager.Haptic(type);
        }
    }

    public void AddCoins(int countCoins)
    {
        Coins += countCoins;
    }
  
}


