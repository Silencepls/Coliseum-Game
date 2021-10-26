using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLogic : MonoBehaviour
{
    private float duration { get; set; }
    private bool settedUp { get; set; }
    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
    private void Update()
    {
        if (!settedUp)
        {
            return;
        }
        else
        {
            if(duration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
