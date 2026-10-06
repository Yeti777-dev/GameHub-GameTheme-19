using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameObjectScriptViewer : EditorWindow
{
    private class Row
    {
        public GameObject gameObject;
        public MonoScript scriptAsset;
        public string scriptName;
    }

    private readonly List<Row> rows = new List<Row>();

    private Vector2 scrollPos;
    private string search = "";
    private Dictionary<string, bool> scriptFilters = new Dictionary<string, bool>();
    private Dictionary<string, int> scriptCounts = new Dictionary<string, int>();

    [MenuItem("Tools/Darvin/GameObject Script Viewer")]
    static void ShowWindow()
    {
        GetWindow<GameObjectScriptViewer>("Script Viewer");
    }

    private void OnGUI()
    {
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Scan Selected", GUILayout.Height(30)))
        {
            ScanSelection();
        }

        if (GUILayout.Button("Clear", GUILayout.Width(80), GUILayout.Height(30)))
        {
            rows.Clear();
        }

        if (GUILayout.Button("Copy All", GUILayout.Width(100), GUILayout.Height(30)))
        {
            CopyAll();
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        search = EditorGUILayout.TextField("Search", search);

        GUILayout.Space(5);

        EditorGUILayout.LabelField(
            $"Total Scripts : {rows.Count}",
            EditorStyles.boldLabel);

        GUILayout.Space(5);

        EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        int column = 0;

        foreach (var pair in scriptCounts)
        {
            scriptFilters[pair.Key] = GUILayout.Toggle(
                scriptFilters[pair.Key],
                $"{pair.Key} ({pair.Value})",
                GUILayout.Width(170));

            column++;

            if (column % 4 == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        GUILayout.Space(5);

        DrawHeader();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var row in rows)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string s = search.ToLower();

                if (!row.gameObject.name.ToLower().Contains(s) &&
                    !row.scriptName.ToLower().Contains(s))
                    continue;
            }

            if (!scriptFilters[row.scriptName])
                continue;

            DrawRow(row);
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        GUILayout.Label("GameObject", GUILayout.Width(180));
        GUILayout.Label("", GUILayout.Width(28));

        GUILayout.Label("Reference", GUILayout.Width(180));

        GUILayout.Label("Script", GUILayout.Width(180));
        GUILayout.Label("", GUILayout.Width(28));

        GUILayout.Label("Reference", GUILayout.Width(180));

        GUILayout.Label("", GUILayout.Width(45));
        GUILayout.Label("", GUILayout.Width(45));

        EditorGUILayout.EndHorizontal();
    }

    void DrawRow(Row row)
    {
        EditorGUILayout.BeginHorizontal("box");

        // GameObject Name
        EditorGUILayout.SelectableLabel(
            row.gameObject.name,
            GUILayout.Width(180),
            GUILayout.Height(EditorGUIUtility.singleLineHeight));

        // Copy GO Name
        if (GUILayout.Button("C", GUILayout.Width(24)))
        {
            EditorGUIUtility.systemCopyBuffer = row.gameObject.name;
        }

        // GameObject Reference
        EditorGUILayout.ObjectField(
            row.gameObject,
            typeof(GameObject),
            true,
            GUILayout.Width(180));

        // Script Name
        EditorGUILayout.SelectableLabel(
            row.scriptName,
            GUILayout.Width(180),
            GUILayout.Height(EditorGUIUtility.singleLineHeight));

        // Copy Script Name
        if (GUILayout.Button("C", GUILayout.Width(24)))
        {
            EditorGUIUtility.systemCopyBuffer = row.scriptName;
        }

        // Script Reference
        EditorGUILayout.ObjectField(
            row.scriptAsset,
            typeof(MonoScript),
            false,
            GUILayout.Width(180));

        // Ping GO
        if (GUILayout.Button("GO", GUILayout.Width(40)))
        {
            EditorGUIUtility.PingObject(row.gameObject);
            Selection.activeGameObject = row.gameObject;
        }

        // Ping Script
        if (GUILayout.Button("CS", GUILayout.Width(40)))
        {
            if (row.scriptAsset != null)
            {
                EditorGUIUtility.PingObject(row.scriptAsset);
                Selection.activeObject = row.scriptAsset;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    void ScanSelection()
    {
        rows.Clear();

        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Please select a GameObject.");
            return;
        }

        ScanRecursive(Selection.activeGameObject.transform);

        Debug.Log($"Found {rows.Count} scripts.");

        scriptFilters.Clear();
        scriptCounts.Clear();

        foreach (var row in rows)
        {
            if (!scriptCounts.ContainsKey(row.scriptName))
            {
                scriptCounts[row.scriptName] = 0;
                scriptFilters[row.scriptName] = true;
            }

            scriptCounts[row.scriptName]++;
        }
    }

    void ScanRecursive(Transform current)
    {
        MonoBehaviour[] behaviours = current.GetComponents<MonoBehaviour>();

        // Ignore GameObjects without MonoBehaviour scripts
        if (behaviours.Length > 0)
        {
            foreach (MonoBehaviour mb in behaviours)
            {
                if (mb == null)
                    continue;

                rows.Add(new Row
                {
                    gameObject = current.gameObject,
                    scriptName = mb.GetType().Name,
                    scriptAsset = MonoScript.FromMonoBehaviour(mb)
                });
            }
        }

        foreach (Transform child in current)
        {
            ScanRecursive(child);
        }
    }

    void CopyAll()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var row in rows)
        {
            sb.AppendLine($"{row.gameObject.name}\t{row.scriptName}");
        }

        EditorGUIUtility.systemCopyBuffer = sb.ToString();

        Debug.Log("Copied all rows.");
    }
}