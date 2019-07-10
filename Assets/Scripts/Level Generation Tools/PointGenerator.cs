using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//this class is in charge of spawning the initial point, get
public class LevelGeneratorEditor : EditorWindow
{
    GeneratedPoint initialPoint;
    public GeneratedPoint pointPrefab;

    public string fileName = "newLevel";
    PointMap loadingPointMap;

    [MenuItem("Window/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGeneratorEditor));
    }

    void OnGUI()
    {
        #region GENERATING NEW LEVEL
        EditorGUILayout.LabelField("Generate New Level", EditorStyles.boldLabel);

        pointPrefab = (GeneratedPoint)EditorGUILayout.ObjectField(pointPrefab, typeof(GeneratedPoint), false);

        if (GUILayout.Button("New Level"))
        {
            if (initialPoint != null)
            {
                DestroyImmediate(initialPoint);
            }
            if (pointPrefab != null)
                initialPoint = Instantiate(pointPrefab);
                
        }
        #endregion

        DrawUILine(Color.black, 1, 25);

        #region GENERATING NEW POINT
        EditorGUILayout.LabelField("Add New Point", EditorStyles.boldLabel);
        if (GUILayout.Button("New Point"))
        {
            GameObject[] selectedObjects = UnityEditor.Selection.gameObjects;

            GeneratedPoint focusPoint;
            foreach (GameObject selectedObject in selectedObjects)
            {
                if (selectedObject.GetComponent<GeneratedPoint>())
                {
                    GeneratedPoint newPoint = Instantiate(pointPrefab);

                    newPoint.transform.parent = selectedObject.transform;
                    newPoint.parentPoint = selectedObject;
                    break;
                }
            }
            
        }

        #endregion

        DrawUILine(Color.black, 1, 25);

        #region LOADING NEW LEVEL
        EditorGUILayout.LabelField("Load Existing Level", EditorStyles.boldLabel);

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
                    nextPoint.parentPoint = previousPoint.gameObject;

                    previousPoint = nextPoint.transform;
                }
            }
        }
        #endregion

        DrawUILine(Color.black, 1, 25);

        #region SAVING GENERATED MAP AS SCRIPTABLE OBJECT
        EditorGUILayout.LabelField("Save Level", EditorStyles.boldLabel);

        fileName = GUILayout.TextField(fileName);
        if (fileName == "")
            fileName = "newPointMap";

        if (GUILayout.Button("Save Point Map"))
        {
            SaveLevelData();
        }
        #endregion
    }

    public void SaveLevelData()
    {

        GeneratedPoint[] points = initialPoint.GetComponentsInChildren<GeneratedPoint>();


        PointMap pointMap = ScriptableObject.CreateInstance<PointMap>();

        pointMap.points = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
            pointMap.points[i] = points[i].transform.position;

        AssetDatabase.CreateAsset(pointMap, "Assets/Levels/" + fileName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
