using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : UIElement
{
    public static UIControls Instance;

    [SerializeField] public Joystick _joystick;

    public Vector3 GetDir() { return _joystick.Direction; }

    private void Awake()
    {
        Instance = this;
    }
    
}
