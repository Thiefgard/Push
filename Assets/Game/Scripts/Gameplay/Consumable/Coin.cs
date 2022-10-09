using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
   
    [SerializeField] float _rotateSpeed = 1f;
    [SerializeField] int _coinValue = 1;
    [SerializeField] GameObject _collectEffect;
    //[SerializeField] RectTransform instPos;
    private void Start()
    {
        //_collectSound = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        transform.Rotate(0, _rotateSpeed, 0, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Weapon")
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        //_collectSound.Play();
        Vector3 posScreen = Camera.main.WorldToScreenPoint(transform.position);
        UIFlyCoins.Instance.SpawnCoins(posScreen, 1, 0.2f);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponentInChildren<Transform>().gameObject.SetActive(true);
        GameObject effect =  Instantiate(_collectEffect, Level.Instance.transform);
        effect.transform.position = transform.position + new Vector3(0, 0.5f, 0) ;
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
   

    public int GetCoin()
    {
        return _coinValue;
    }
}
