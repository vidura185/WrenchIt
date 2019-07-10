using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpannerController : MonoBehaviour
{
    public Transform pivotOne;
    public Transform pivotTwo;
    private Collider pivotOneCollider;
    private Collider pivotTwoCollider;


    public Transform attachPivot;

    public float rotateSpeed;

    private void Start()
    {
        
    }
    private void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        if (transform.parent != attachPivot)
            transform.parent = attachPivot;

        transform.Rotate(attachPivot.transform.position, rotateSpeed * Time.deltaTime);
    }

    public void OnAttach(Transform newPivot)
    {
        attachPivot = newPivot;
    }

}
 
public class SpannerEnd
{

}