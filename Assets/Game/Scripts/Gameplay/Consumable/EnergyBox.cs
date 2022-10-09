using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBox : MonoBehaviour
{
    [SerializeField] GameObject _collectEffect;

    //private AudioSource _collectSound;
    private void Start()
    {
        //_collectSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Weapon")
        {
            Player player = Player.Instance;
            if (player.energyBoxCount < 3)
            {
                GameObject weapon = WeaponHolder.Instace._selectedWeapon;
                weapon.transform.localScale += new Vector3(0.05f, 0, 0);
                player.energyBoxCount++;
                //_collectSound.Play();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                GameObject effect = Instantiate(_collectEffect, Level.Instance.transform);
                effect.transform.position = transform.position + new Vector3(0, -0.5f, 0);
                Destroy(gameObject, 0.7f);
            }
            else
            {
                //_collectSound.Play();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                GameObject effect = Instantiate(_collectEffect, Level.Instance.transform);
                effect.transform.position = transform.position + new Vector3(0, -0.4f, 0);
                Destroy(gameObject, 0.7f);
            }
        }
    }
    
}
