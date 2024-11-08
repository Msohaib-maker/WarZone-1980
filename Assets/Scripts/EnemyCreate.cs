using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public Transform[] points = new Transform[3];
    public GameObject enemyPrefab;
    bool isEnemyExist = false;
    public int count_enemy = 0;
    


    public static EnemyCreate instance;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {

        if (count_enemy <= 0)
        {
            isEnemyExist = false;
        }

        if (!isEnemyExist && CanvasUpdate.instance.kills < CanvasUpdate.instance.targetkills)
        {
            
            for (int i=0;i<points.Length;i++)
            {
                Transform randomPoint = points[i];
                GameObject enemyInstance = Instantiate(enemyPrefab, randomPoint.position,Quaternion.Euler(0,0,0));

                count_enemy++;
            }

            isEnemyExist =true;
      

        }


        if (CanvasUpdate.instance.isWin || CanvasUpdate.instance.isLoss)
        {
            DestroyAllEnemies();
        }

    }

    void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        count_enemy = 0; // Reset count_enemy
        isEnemyExist = false; // Reset isEnemyExist
    }
}
