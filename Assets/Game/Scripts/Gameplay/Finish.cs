using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishLevel(other);
        }
    }

    public  void FinishLevel(Collider other)
    {
        other.GetComponent<Player>().isFinished = true;
        CameraMovement.Instance.SetActiveConfetti(true);
    }
}
