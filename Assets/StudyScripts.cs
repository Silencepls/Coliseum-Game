using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StudyScripts : MonoBehaviour
{
    [Flags]
    public enum State
    {
        None = 0,
        Stunned = 1,
        Slowed = 2,
        Burning = 4
    }
    public State state;

    public bool test = false;

    public string Test { get; private set; }

    private void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateStats updateStats = collision.GetComponent<UpdateStats>();
        if(updateStats != null)
        {
            updateStats.TakeDamage(5000);
        }
    }
}



#region Interfaces
public interface IMyInterFace
{
    void TestFuction();
}
public class MyClass : IMyInterFace
{
    public void TestFuction()
    {
    }
}
#endregion
