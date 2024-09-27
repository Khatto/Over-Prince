using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager instance;

    private void Awake()
    {
        SetupSingleton();
    }

    public void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void HitStop(float duration)
    {
        if (duration == 0.0) return;
        Time.timeScale = 0;
        StartCoroutine(StopTime(duration));
    }

    private IEnumerator StopTime(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
}
