using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private int powerUpID;
    [SerializeField]
    private AudioClip _clip;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if(transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position, 10.0f);
            
            if(player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("Triple Shot Collected");
                        break;
                    case 1:
                        player.SpeedUpActive();
                        Debug.Log("SpeedUp Collected");
                        break;
                    case 2:
                        player.ShieldActive();
                        Debug.Log("Shield Collected");
                        break;
                    default:
                        Debug.Log("Something Went Wrong");
                        break;   
                }
            }


            Destroy(this.gameObject);
        }
    }
}
