using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public bool stoppedTime = false;

    float _a;
    float _b;

    private void Start()
    {
        instance = this;
        _a = Time.timeScale;
        _a = Time.fixedDeltaTime;
    }
    public IEnumerator StopTime()
    {
        stoppedTime = true;
        Time.timeScale = 0.001f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(5f);
        NormalTime();
    }
    public void NormalTime()
    {
        Time.timeScale = _a;
        Time.fixedDeltaTime = _b;
    }
    public void WinScene()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayScene()
    {
        SceneManager.LoadScene(1);
    }
    public void ItemScene()
    {
        SceneManager.LoadScene(3);
    }
    public void StartScene()
    {
        SceneManager.LoadScene(0);
    }
}
