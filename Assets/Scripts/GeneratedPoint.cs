using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPoint : MonoBehaviour
{
    public GameObject nextPoint;
    public GameObject parentPoint;


    private void OnDrawGizmosSelected()
    {
        if (parentPoint != null)
        {
            Vector3 offset = gameObject.transform.position - parentPoint.transform.position;
            offset = offset.normalized;

            transform.position = parentPoint.transform.position + offset;
        }
    }
}

