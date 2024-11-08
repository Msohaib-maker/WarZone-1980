using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour
{

    public Vector3 m_target;
    public GameObject collisionExplosion;
    private Collider targetCollider;

    public float bulletSpeed = 120f;



    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;// The step size is equal to speed times frame time.
        

    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("enemy"))
        {
            
            //Debug.Log("defe");
            // Do something when the shot hits the target collider (e.g., trigger an explosion)
            EnemyCreate.instance.count_enemy -= 1;
            Destroy(other.gameObject);
            explode();
        }
    }

    void explode()
    {
        if (collisionExplosion != null)
        {
            CanvasUpdate.instance.SuccessKill();
            CanvasUpdate.instance.explode_audio();
            GameObject explosion = Instantiate(
                collisionExplosion, transform.position, transform.rotation);
            Destroy(explosion, 1f);
        }


    }

}