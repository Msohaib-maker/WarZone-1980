using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 5.0f;
    private GameObject player;
    //public NavMeshAgent enemy;


    //public float speed = 5.0f;
    public float stoppingRange = 18.0f;
    public float shootingRange = 30.0f; // Distance at which the enemy starts shooting
    public GameObject bulletPrefab; // Prefab of the bullet to shoot
    public Transform firePoint; // Point where the bullet is spawned
    public float shootInterval = 1.0f; // Interval between shots
    public float bulletSpeed = 29.0f;

    private bool isShooting = false;
    private float lastShootTime;

    public float verticalMovementAmount = 0.5f; // Amount of vertical movement
    public float verticalMovementSpeed = 1.0f;


    public float CollisonAvoidanceDistance = 50f;

    float time_interval = 0;
    [SerializeField]private Vector3 targetPosition;
    float y_pos = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure to tag your player GameObject with 'Player'.");
        }

        y_pos = transform.position.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
            return;

        

        FacePlayer();
        StartShoot();
        AvoidUFOCollision();
       

        time_interval -= Time.deltaTime;


    }

    void FacePlayer()
    {
        Vector3 direction = player.transform.position - transform.position; // Direction towards the player
        direction.y = 0; // Ignore the height difference to prevent tilting
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; // Calculate angle on the Y-axis
        Quaternion rotation = Quaternion.Euler(-90, 0, angle); // Only rotate around the Y-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f); // Smooth Y-axis rotation
    }


    void AvoidUFOCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, CollisonAvoidanceDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject) // Avoid self
            {
                Vector3 avoidanceDirection = (transform.position - collider.transform.position).normalized;
                targetPosition += avoidanceDirection * 1.0f;
            }
        }
    }

    //private void EnemyPlayerKiTaraf()
    //{
    //    Vector3 playerDirection = (player.transform.position - transform.position).normalized;
    //    targetPosition = player.transform.position + playerDirection * Random.Range(2.0f, 5.0f);
    //    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    //}

    private void StartShoot()
    {
        if (!isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    public float bulletDuration = 5.0f;

    IEnumerator Shoot()
    {
        isShooting = true;
        
            // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            // Calculate direction towards the player
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
            // Set bullet velocity
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            // Destroy bullet after some time
        Destroy(bullet,bulletDuration);

            // Wait for the next shot
        yield return new WaitForSeconds(shootInterval);


        isShooting=false;
        //Debug.Log("cook");
        
    }
}
