using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//this class is in charge of spawning the initial point, get
public class LevelGeneratorEditor : EditorWindow
{
    GeneratedPoint initialPoint;

    public GeneratedPoint pointPrefab;
    public ControlPoint controlPointPrefab;

    public string fileName = "newLevel";

    PointMap loadingPointMap;

    public Spanner spanner;

    [MenuItem("Window/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGeneratorEditor));
    }

    void OnGUI()
    {
        pointPrefab = (GeneratedPoint)EditorGUILayout.ObjectField(pointPrefab, typeof(GeneratedPoint), false);
        controlPointPrefab = (ControlPoint)EditorGUILayout.ObjectField(controlPointPrefab, typeof(ControlPoint), false);

        if (GUILayout.Button("New Level"))
        {
            if (initialPoint != null)
            {
                Destroy(initialPoint);
            }
            if (pointPrefab != null)
                initialPoint = Instantiate(pointPrefab);
                
        }

        if (GUILayout.Button("New Point"))
        {
            GameObject[] selectedObjects = UnityEditor.Selection.gameObjects;

            GeneratedPoint focusPoint;
            foreach (GameObject selectedObject in selectedObjects)
            {
                if (selectedObject.GetComponent<GeneratedPoint>())
                {
                    GeneratedPoint newPoint = Instantiate(pointPrefab);
                    //newPoint.transform.position = Vector2.up * spanner.length;
                    newPoint.transform.parent = selectedObject.transform;
                    newPoint.parentPoint = selectedObject;

                    newPoint.controlPoint = Instantiate(controlPointPrefab);
                    newPoint.controlPoint.transform.parent = selectedObject.transform;
                    newPoint.controlPoint.point = newPoint;
                    break;
                }
            }
            
        }

        GUILayout.Space(10);
        loadingPointMap = (PointMap)EditorGUILayout.ObjectField(loadingPointMap, typeof(PointMap), true);
        if (GUILayout.Button("Load Level"))
        {
            //clear current level
            if (initialPoint != null)
                Destroy(initialPoint);

            if (loadingPointMap != null)
            {
                initialPoint = Instantiate(pointPrefab);
                initialPoint.transform.position = loadingPointMap.points[0];
                Transform previousPoint = initialPoint.transform;

                foreach (Vector2 pointLocation in loadingPointMap.points)
                {
                    var nextPoint = Instantiate(pointPrefab);
                    nextPoint.transform.position = pointLocation;
                    nextPoint.transform.parent = previousPoint;

                    //setting control point
                    nextPoint.controlPoint = Instantiate(controlPointPrefab);
                    nextPoint.controlPoint.transform.position = nextPoint.transform.position;
                    nextPoint.controlPoint.transform.parent = previousPoint;
                    nextPoint.controlPoint.point = nextPoint;

                    previousPoint = nextPoint.transform;
                }
            }
        }

        GUILayout.Space(10);
        fileName = GUILayout.TextField(fileName);
        if (fileName == "")
            fileName = "newPointMap";

        if (GUILayout.Button("Save Point Map"))
        {
            SaveLevelData();
        }
    }

    public void SaveLevelData()
    {
        List<GeneratedPoint> temp = new List<GeneratedPoint>(initialPoint.GetComponentsInChildren<GeneratedPoint>());
        List<GeneratedPoint> points = new List<GeneratedPoint>();

        points.Add(initialPoint);
        points.AddRange(temp);

        PointMap pointMap = ScriptableObject.CreateInstance<PointMap>();

        pointMap.points = new Vector2[points.Count];

        for (int i = 0; i < points.Count; i++)
            pointMap.points[i] = points[i].transform.position;

        AssetDatabase.CreateAsset(pointMap, "Assets/" + fileName + ".asset");
    }
}



