using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using MoreMountains.NiceVibrations;

public static class ExtensionMethods
{
    public static void ApplyBasicAttributes(this Button button, bool playHaptic = true, float pulseAmplitude = 0.05f, float pulseDuration = 0.3f, System.Action onPointerDownCallback = null)
    {
        if (button.GetComponent<EventTrigger>())
        {
            GameObject.Destroy(button.GetComponent<EventTrigger>());
        }

        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        var pointerUp = new EventTrigger.Entry();
        var pointerDown = new EventTrigger.Entry();

        float baseScale = button.transform.localScale.x;

        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => button.transform.DOScale(baseScale, pulseDuration / 2f));

        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => { button.transform.DOScale(baseScale * (1f - pulseAmplitude), pulseDuration / 2f); if (playHaptic) MMVibrationManager.Haptic(HapticTypes.LightImpact); if (onPointerDownCallback != null) onPointerDownCallback(); });

        trigger.triggers.Add(pointerUp);
        trigger.triggers.Add(pointerDown);
    }

    public static void ApplySelectionAttributes(this SelectionButton selectionButton, System.Action pushCallback, System.Action releaseCallback, bool playHaptic = true, float pressDownscale = 0.0f, float pulseDuration = 0.3f)
    {
        if (selectionButton.GetComponent<EventTrigger>())
        {
            GameObject.Destroy(selectionButton.GetComponent<EventTrigger>());
        }

        EventTrigger trigger = selectionButton.gameObject.AddComponent<EventTrigger>();

        var pointerUp = new EventTrigger.Entry();
        var pointerDown = new EventTrigger.Entry();

        float baseScale = selectionButton.transform.localScale.x;

        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => releaseCallback());

        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => { selectionButton.transform.DOScale(baseScale * (1f - pressDownscale), pulseDuration / 2f); if (playHaptic && selectionButton.isSelectable) MMVibrationManager.Haptic(HapticTypes.LightImpact); pushCallback(); });

        trigger.triggers.Add(pointerUp);
        trigger.triggers.Add(pointerDown);
    }
}
