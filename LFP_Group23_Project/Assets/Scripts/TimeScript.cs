using UnityEngine;
using UnityEngine.InputSystem;

public class TimeScript : MonoBehaviour
{
    public float slowMo = .5f;
    private float normalTime = 1.0f;

    public bool SlowMo;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = slowMo;
            Time.fixedDeltaTime = .02f * Time.timeScale;

            SlowMo = true;
        }
        else
        {
            Time.timeScale = normalTime;
            Time.fixedDeltaTime = .02f * Time.timeScale;

            SlowMo = false;
        }

        animator.SetBool("SlowMo", SlowMo);
    }
}
