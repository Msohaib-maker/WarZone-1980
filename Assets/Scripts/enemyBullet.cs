using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            CanvasUpdate.instance.takeDamage(1.0f);
            
        }

        if (other.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
