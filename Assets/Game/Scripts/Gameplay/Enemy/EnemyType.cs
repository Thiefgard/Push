using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public enum Type
    {
        Base,
        Fast,
        Giant,
        Range
    }

    [SerializeField] public Type _type;
}
