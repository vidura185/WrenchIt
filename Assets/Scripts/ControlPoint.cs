using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    public GeneratedPoint point;
    private void OnDrawGizmosSelected()
    {
        if (point != null && point.parentPoint != null)
        {
            //normalized Vector3 from parent to control point to control distance
            Vector3 offset = gameObject.transform.position - point.parentPoint.transform.position;
            offset = offset.normalized;

            point.transform.position = point.parentPoint.transform.position + offset;
        }
    }
}