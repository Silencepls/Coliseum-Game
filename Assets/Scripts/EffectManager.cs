using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject burnEffect;
    public GameObject stunEffect;
    public GameObject shootEffect;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
