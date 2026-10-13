using UnityEngine;
using UnityEngine.UIElements;

public class WeakPointScript : MonoBehaviour
{
    //public ParticleSystem enemyDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //enemyDestroyed.Play();

            Destroy(gameObject.transform.parent.gameObject, .195f);
        }
    }
}
