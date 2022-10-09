using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float playerMovementSpeed = 5f;
    [SerializeField] private float playerRotationSpeed = 4f;
    [SerializeField] private Rigidbody playerRb;

    public WeaponHolder weapon;
    public bool isFinished = false;
    private bool isStartDance = false;
    public bool isInBoundery = true;
    private bool isDead = false;
    private float lockRotation = 0;
    private Vector3 movement;
    [SerializeField]private Animator anim;
    public int energyBoxCount = 0;
    private float speed = 0;
    [SerializeField] private GameObject _playerCurrentSkin;
    private bool isFirstHit;
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
       
    }

    private void Start()
    {
        isFirstHit = false;
        UIControls.Instance._joystick.Reset();
        playerRb = GetComponent<Rigidbody>();
        weapon = WeaponHolder.Instace;
    }

    private void Update()
    {
        if (_playerCurrentSkin == null)
        ////{
        ////    PlayerPrefs
        ////    anim = _playerCurrentSkin.GetComponent<Animator>();
        //}
        if (!isInBoundery)
        {
            movement = new Vector3(0, 0, 0);
            playerRb.velocity = Vector3.zero;
            return;
        }

        UIState state = UIManager.Instance.GetCurrentState();
        if (state == UIState.ActionPhase)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!isFirstHit) isFirstHit = true;
                else
                    weapon.WeaponHit(anim);
            }
        }
        
    }
    
    
    public void SetSkin(GameObject skin)
    {
        if(anim != null)
        {
            Destroy(anim.gameObject);
        }
        _playerCurrentSkin = Instantiate(skin, transform);
        anim = _playerCurrentSkin.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (isFinished)
        {
            movement = new Vector3(0, 0, 0);
            playerRb.velocity = Vector3.zero;
            if (!isStartDance)
            {
                isStartDance = true;
                weapon.gameObject.SetActive(false);
                anim.SetBool("finished", true);
                anim.speed = 0.8f;
                transform.rotation = Quaternion.Euler(0, -180, 0);
                GameManager.Instance.OnLevelFinished(true);
            }
            return;
        }
        if (weapon._isSuperHit) movement = Vector3.zero;
        if (!weapon._isSuperHit || isInBoundery)
        {
            if (!isDead)
            {
                MoveAction();
            }
        }
        if(movement != Vector3.zero)
        {
            speed = UIControls.Instance.GetDir().magnitude * playerMovementSpeed;
            playerRb.MovePosition(playerRb.position + movement * Time.fixedDeltaTime * speed);
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotationSpeed * Time.deltaTime);
        }
        else
        {
            speed = 0;
        }
        if(anim != null) anim.SetFloat("Velocity", speed);

    }

    private void MoveAction()
    {
        Vector2 dir = UIControls.Instance.GetDir();
        movement = new Vector3(dir.x, 0, dir.y);
        movement.Normalize();
        transform.rotation = Quaternion.Euler(lockRotation, transform.eulerAngles.y, lockRotation);
    }

    void EndPlayerHitAnim()
    {
        anim.SetLayerWeight(1, 0);
        if (weapon._weapon == WeaponHolder.WeaponType.RocketLauncher) anim.SetBool("isRocket", false);
    }
    public void Die()
    {
        isDead = true;
        GameManager.Instance.OnLevelFinished(false);
        anim.SetBool("isMoving", false);
        movement = Vector3.zero;
        weapon.energy = 0;
        CameraMovement.Instance._target = null;
        Destroy(gameObject, 1f);
    }
}


