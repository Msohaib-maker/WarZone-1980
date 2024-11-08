using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;

    private void OnTriggerEnter(Collider other)
    {
        MainRot.instance.TakeDamage(rotationSpeed);
    }
}
