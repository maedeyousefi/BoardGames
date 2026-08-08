using UnityEngine;
using UnityEditor;
public class LudoBoardEditor : EditorWindow
{
    private Transform mainPath;
    private GameObject cellPrefab;
    [MenuItem("Tools/Ludo Board Editor")]
    public static void Open()
    {
        GetWindow<LudoBoardEditor>("Ludo Board");
    }
    private void OnEnable() 
    { 
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable() 
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void GenerateCells()
    {
        if (mainPath == null || cellPrefab == null)
        {
            Debug.LogError("MainPath یا CellPrefab تنظیم نشده است.");
            return;
        }

        while (mainPath.childCount > 0)
        {
            DestroyImmediate(mainPath.GetChild(0).gameObject);
        }
        for (int i = 0; i < 52; i++)
        {
            GameObject cell = (GameObject)PrefabUtility.InstantiatePrefab(cellPrefab, mainPath);
            cell.name = $"Cell_{i:00}";
        }
        Debug.Log("52 cells generated successfully.");
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (mainPath == null || cellPrefab == null)
                return;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CreateCellAt(hit.point);
                e.Use();
            }
        }
    }

    private void CreateCellAt(Vector3 position)
    {
        GameObject cell = (GameObject)PrefabUtility.InstantiatePrefab(cellPrefab, mainPath);

        cell.transform.position = position;
        cell.name = $"Cell_{mainPath.childCount - 1:00}";
    }
    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Ludo Board Generator", EditorStyles.boldLabel);

        mainPath = (Transform)EditorGUILayout.ObjectField("Main Path", mainPath, typeof(Transform), true);

        cellPrefab = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", cellPrefab, typeof(GameObject), false);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate 52 Cells"))
        {
            GenerateCells();
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox("حالا خانه‌های قبلی را پاک کن و در Scene روی برد کلیک کن تا خانه‌ها ساخته شوند.", MessageType.Info);
    }
}