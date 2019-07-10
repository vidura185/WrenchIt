using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//position vectors for each consecutive point are stored in this scriptable object
public class PointMap : ScriptableObject
{
    public Vector2[] points;
}