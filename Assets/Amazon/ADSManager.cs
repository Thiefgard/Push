using System;
using System.Collections;
using System.Collections.Generic;
using FAdsSdk;
using JambaEngine.Purchasing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vaveda.Integration.Scripts;
using Vaveda.Integration.Scripts.Analytics;
using Vaveda.Integration.Scripts.Analytics.Builders;
using Vaveda.Integration.Scripts.Fads;

public class ADSManager : MonoBehaviour
{
    public static ADSManager Instance { get; private set; }


    public bool _wasSubscription;

    private float _timeLvlStart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _wasSubscription = false;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        var analyticsBuilder = new AnalyticsBuilder().Add<VavedaAnalyticsService>();

        var adMask = AdShowType.Everything;

        Services.Instance.OnInitializationComplete += OnInitializeComplete;
        Services.Instance.Initialize(analyticsBuilder, adMask);

        JambaStartAppPurchasing.Initialize();
    }

    private void OnInitializeComplete()
    {
        Debug.Log("SDK LOADED");
    }

    public void ShowSubscription()
    {
        if (!_wasSubscription)
        {
            if (JambaStartAppPurchasing.IsCanShow)
            {
                JambaStartAppPurchasing.ShowSubscriptionDialog();
                if (!JambaStartAppPurchasing.IsSubscribed)
                {
                    Services.Instance.FadsService?.ShowBanner(Placements.PLACEMENT_BANNER_LEVEL);
                }
                _wasSubscription = true;
            }
        }
    }

    public void AnalyticsStartLevel(string nameLvl, int levelNum)
    {
        Services.Instance.AnalyticsService.LevelStartEvent(nameLvl, (uint)levelNum);

        _timeLvlStart = Time.time;
        Debug.Log($"{nameLvl} {levelNum}");
    }

    public void AnalyticsEndLevel(string nameLvl, int levelNum, bool win)
    {
        int winNum = win ? 1 : 0;

        float timeForLvl = Time.time - _timeLvlStart;

        Services.Instance.AnalyticsService.LevelEndEvent(nameLvl, (uint)levelNum, (uint)timeForLvl, (uint)winNum);
        Debug.Log($"{nameLvl} {levelNum} {timeForLvl} {winNum}");
    }

    public void ShowBunner()
    {
        if (!JambaStartAppPurchasing.IsSubscribed)
        {
            Services.Instance.FadsService?.ShowBanner(Placements.PLACEMENT_BANNER_LEVEL);
        }
    }

    public void ShowInterstitial(string placement)
    {
        if (!JambaStartAppPurchasing.IsSubscribed)
        {
            if (Services.Instance.FadsService is { HasInterstitial: true })
            {
                Services.Instance.FadsService.ShowInterstitial(placement);
            }
        }
    }

}
