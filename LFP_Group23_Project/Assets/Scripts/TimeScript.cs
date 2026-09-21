using UnityEngine;

public class TimeScript : MonoBehaviour
{
    private float slowMo = .5f;
    private float normalTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Time.timeScale = normalTime;
            Time.fixedDeltaTime = .02f * Time.timeScale;
        }
        else
        {
            Time.timeScale = slowMo;
            Time.fixedDeltaTime = .02f * Time.timeScale;
        }
    }
}
