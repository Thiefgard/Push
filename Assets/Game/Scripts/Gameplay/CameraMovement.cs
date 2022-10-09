using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;

    [SerializeField] private GameObject _confettiVfx;
    [SerializeField] private float _offsetOnRun;
    [SerializeField] private Vector3 _offsetPosMM;
    [SerializeField] private Vector3 _offsetPosAP;
    [SerializeField] private Vector3 _offsetRotMM;
    [SerializeField] private Vector3 _offsetRotAP;
    [SerializeField] private Vector3 _offsetPosFinish;
    [SerializeField] private Vector3 _offsetRotFinish;
    [SerializeField] private float _speed;
    private float _superOffset = -10f;

    [SerializeField]bool _isSuperCamera;

    private float _normalCameraForwardSpeed = 0.08f;
    private float _superHitForwardSpeed = 0.04f;
    public GameObject _target;
    bool _finishCam = false;
    private float _calcOffset = 0f;
    private float _curForwardOffset = 0f;
    private Vector3 _offset;

    private void Awake()
    {
        Instance = this;
        _speed = _normalCameraForwardSpeed;
    }

    public void Init()
    {
            _target = Player.Instance.gameObject;
            _offset = GetComponentInChildren<Transform>().position - _target.transform.position;
    }
   

    private void FixedUpdate()
    {
        
        if (_target)
        {
            _finishCam = _target.GetComponent<Player>().isFinished;
            _curForwardOffset = Mathf.Lerp(_curForwardOffset, CalculateForwardOffset(), _superHitForwardSpeed);
            if (_isSuperCamera)
            {
                _calcOffset = _superOffset;
                if(_curForwardOffset < -7)
                {
                    _isSuperCamera = false;
                }
            }
            else _calcOffset = 0;

            transform.position = Vector3.Lerp(transform.position, CalculateNeedPos(), _speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, CalculateNeedRot(), _speed);
        }
    }

    private Vector3 CalculateNeedPos()
    {
        Vector3 offset = Vector3.zero;
        UIState state = UIManager.Instance.GetCurrentState();
        if(state == UIState.Start)
        {
            offset = _offsetPosMM;
            _speed = _normalCameraForwardSpeed;
        }
        if (_finishCam)
        {
            offset = _offsetPosFinish;
        }
        else
        {
            offset = _offsetPosAP;
            _speed = _normalCameraForwardSpeed;
        }
        Vector3 needPos = CalculateOffsetPos(offset);

        return needPos;
    }
    private float CalculateForwardOffset()
    {
        return _calcOffset;
    }

    IEnumerator SuperCameraTime()
    {
        _speed = _superHitForwardSpeed;
        yield return new WaitForSeconds(1f);
        _isSuperCamera = false;
        _speed = _normalCameraForwardSpeed;
    }

    //thick about character dir or velocity
    private Quaternion CalculateNeedRot()
    {
        Quaternion rot = Quaternion.identity;

        UIState state = UIManager.Instance.GetCurrentState();

        if (state == UIState.Start)
        {
            rot = Quaternion.Euler(_offsetRotMM);
        }
        if (_finishCam)
        {

            rot = Quaternion.Euler(_offsetRotFinish);
        }
        else
        {
            rot = Quaternion.Euler(_offsetRotAP);
        }

        return rot;
    }

    public void SetForwardOffset( float val)
    {
        _curForwardOffset = val;
    }

    private Vector3 CalculateOffsetPos(Vector3 offset)
    {
        Vector3 needPos = _target.transform.position;
        needPos += Vector3.right * offset.x;
        needPos += Vector3.forward * offset.z;
        needPos += new Vector3(0, offset.y, 0);


        needPos += transform.forward * _curForwardOffset;

        //if (_targetCharacter && _targetCharacter.IsRun())
        //{
        //    needPos += _targetCharacter.GetDirMove() * _offsetOnRun;
        //}

        return needPos;
    }

    

    public bool IsWeaponSuperHit(bool check)
    {
        return _isSuperCamera = check;
    }
    public void SetActiveConfetti(bool val)
    {
        _confettiVfx.SetActive(val);
    }
}
