using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private GameObject player;
    private Camera cam;

    [Header("values")]
    public float followSpeed = 2f;
    public float yOffset = 1f;
    public float xOffset = 0f;
    public float cameraFOV = 10f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        cam = gameObject.GetComponentInChildren<Camera>();

        cam.orthographicSize = cameraFOV;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            gameObject.GetComponent<CameraFollowScript>().enabled = false;
        }
        else
        {
            Vector3 newPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, -10);
            transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }
    }
}
