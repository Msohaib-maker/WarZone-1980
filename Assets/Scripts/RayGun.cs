using System.Collections;
using static UnityEngine.Rendering.SplashScreen;
using UnityEngine;
using System.Collections.Generic;

public class RayGun : MonoBehaviour
{
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;

    RaycastHit hit;
    float range = 1000.0f;

    private Queue<GameObject> laserPool = new Queue<GameObject>();
    public int poolSize = 20;
    public float Duration = 10;

    

    private void Start()
    {
        // Pre-instantiate laser objects and store them in the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject laser = Instantiate(m_shotPrefab);
            laser.SetActive(false); // Deactivate the laser initially
            laserPool.Enqueue(laser);
        }
    }


    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetMouseButton(0))
            {
                //Debug.Log("click");

                if (Time.time > m_shootRateTimeStamp)
                {
                    shootRay();
                    CanvasUpdate.instance.laserShot();
                    m_shootRateTimeStamp = Time.time + shootRate;
                }
            }
        }
        

    }

    void shootRay()
    {
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        

        ShootLaser(transform.position);


    }

    private void ShootLaser(Vector3 targetPosition)
    {
        GameObject laser = GetLaserFromPool(); // Get an inactive laser

        if (laser != null)
        {
            laser.transform.position = transform.position; // Set start position
            laser.transform.rotation = transform.rotation; // Aim at target
            laser.SetActive(true); // Activate the laser

            // Deactivate the laser after a delay
            StartCoroutine(DeactivateLaser(laser, Duration));
        }
    }

    // Get an inactive laser from the pool
    private GameObject GetLaserFromPool()
    {
        if (laserPool.Count > 0)
        {
            return laserPool.Dequeue();
        }
        else
        {
            Debug.LogWarning("Laser pool exhausted. Consider increasing pool size.");
            return null;
        }
    }

    // Return the laser to the pool after use
    private System.Collections.IEnumerator DeactivateLaser(GameObject laser, float delay)
    {
        yield return new WaitForSeconds(delay);
        laser.SetActive(false);
        laserPool.Enqueue(laser); // Return to pool
    }



}