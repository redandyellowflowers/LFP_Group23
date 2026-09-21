using UnityEngine;
using UnityEngine.InputSystem;

public class TimeScript : MonoBehaviour
{
    public float slowMo = .5f;
    private float normalTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
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
