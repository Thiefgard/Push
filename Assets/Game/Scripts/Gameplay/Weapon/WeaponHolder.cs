using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Instace;
   public enum WeaponType
    {
        Rail,
        Wand,
        RocketLauncher
    }

    [SerializeField] public WeaponType _weapon;
    [SerializeField] Vector3 _weaponSpawnPoint;
    [SerializeField] GameObject _rail;
    [SerializeField] GameObject _wand;
    [SerializeField] GameObject _rocket;
    [SerializeField] public int energy;

    
    [SerializeField] public bool _canHit = true;
    public bool _isSuperHit;
    
    Animator _anim;
    Animator _playerAnim;
    public GameObject _selectedWeapon;

    private void Awake()
    {
        Instace = this;
    }
    private void Update()
    {
        //slider.value = energy;
    }
    private void Start()
    {
        SelectWeapon();
        _selectedWeapon = Instantiate(SelectWeapon(), Level.Instance.transform);
        _selectedWeapon.transform.parent = this.transform;
        _selectedWeapon.transform.position = this.transform.position;
        energy = 0;
        _anim = _selectedWeapon.GetComponent<Animator>();
    }

    public void WeaponHit(Animator playerAnim)
    {
        if (_canHit )
        {
            if(energy != 100)
            {
                _canHit = false;
                _playerAnim = playerAnim;
                PlayerHitAnim(true);
                _anim.SetBool("callAttack", true);
            }
            if (energy == 100)
            {
                
                _canHit = false;
                energy = 0;
                PlayerHitAnim(true);
                StartCoroutine(SuperHit());
            }
        }
    }
    IEnumerator SuperHit()
    {
        CameraMovement.Instance.IsWeaponSuperHit(true);
        _anim.SetBool("superAttack", true);
        if(_weapon == WeaponType.Wand)
        {
            _selectedWeapon.GetComponent<Wand>()._isSuper = true;
        }
        
        yield return new WaitForSeconds(0.5f);
        _isSuperHit = false;
        yield break;
    }
    GameObject SelectWeapon()
    {
        if (WeaponType.Rail == _weapon)
        {
            return _rail;
        }
        else if (WeaponType.Wand == _weapon)
        {
            return _wand;
        }
        else
            return _rocket;
    }

    public int AddEnergy(int value)
    {
        energy += value;
        if (energy > 100) return energy = 100;
        return energy;
    }

    public void NextWeapon()
    {
        int length = System.Enum.GetValues(typeof(WeaponType)).Length;
        if((int)_weapon < length - 1)
        {
            _weapon++;
            Destroy(_selectedWeapon);
            _selectedWeapon = Instantiate(SelectWeapon(), Level.Instance.transform);
            _selectedWeapon.transform.parent = this.transform;
            _selectedWeapon.transform.position = this.transform.position;
            energy = 0;
            _anim = _selectedWeapon.GetComponent<Animator>();

        }
        else
        {
            _weapon = 0;
            Destroy(_selectedWeapon);
            _selectedWeapon = Instantiate(SelectWeapon(), Level.Instance.transform);
            _selectedWeapon.transform.parent = this.transform;
            _selectedWeapon.transform.position = this.transform.position;
            energy = 0;
            _anim = _selectedWeapon.GetComponent<Animator>();
        }
    }
    public void PlayerHitAnim(bool canHit)
    {
        if (canHit)
        {
            if (_weapon == WeaponType.RocketLauncher)
            {
                _playerAnim.SetLayerWeight(1, 1);
                _playerAnim.SetBool("isRocket", true);
            }
            if(_weapon == WeaponType.Wand)
            {
                _playerAnim.SetLayerWeight(1, 1);
                _playerAnim.SetBool("isWand", true);
            }
        }
        else
        {
            _playerAnim.SetLayerWeight(1, 0);
            _playerAnim.SetBool("isRocket", false);
            _playerAnim.SetBool("isWand", false);
        }
    }
    
    
}
