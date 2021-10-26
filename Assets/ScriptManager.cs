using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.E))
        {
            TimeManager.instance.StartScene();
        }
    }
}
