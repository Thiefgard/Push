using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vaveda.Integration.Scripts;
using Vaveda.Integration.Scripts.Fads;

public class ChangeWeapon : UIElement
{
    private WeaponHolder wh;

    private bool _isWatchAds = false;

    private void Start()
    {
        wh = WeaponHolder.Instace;
    }
    private void Update()
    {
        if(wh == null)
        {
            wh = WeaponHolder.Instace;
        }
    }

    public void Change()
    {
        wh.NextWeapon();
    }
    public void OnChangeWeaponAds()
    {
        if (Services.Instance.FadsService is { HasRewardedVideo: true })
        {
            Services.Instance.FadsService.RewardedShouldReward += ChangeWeaponReward;
            Services.Instance.FadsService.ShowRewardedVideo(Placements.PLACEMENT_REWARDED_LEVEL_GETHINTS);
        }
    }

    private void ChangeWeaponReward()
    {
        Services.Instance.FadsService.RewardedShouldReward -= ChangeWeaponReward;
        _isWatchAds = true;
        Change();
    }

}
