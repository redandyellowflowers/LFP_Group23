using TMPro;
using UnityEngine;

public class CharacterControllerScript_Miguel : MonoBehaviour
{
    public CharacterController2D characterController;

    public GameObject spawnPoint;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;

    //public TextMeshProUGUI sigText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.W)) { jump = true; }


        if (this.gameObject.transform.position.y <= -20f)
        {
            gameObject.transform.position = spawnPoint.transform.position;
        }
    }

    //Is called a fixed number of times per second.
    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            sigText.enabled = true;
        }
        else { sigText.enabled = false; }
    }
    */
}
