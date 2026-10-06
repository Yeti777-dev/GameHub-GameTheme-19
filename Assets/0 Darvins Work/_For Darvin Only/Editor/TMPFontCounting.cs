#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class TMPFontCounting : EditorWindow
{
    private class FontUsageData
    {
        public List<TMP_Text> users = new List<TMP_Text>();
        public bool foldout;
    }

    private Dictionary<TMP_FontAsset, FontUsageData> fontUsage =
        new Dictionary<TMP_FontAsset, FontUsageData>();

    private Vector2 scroll;

    [MenuItem("Tools/Darvin/TMP_Text/TMP Font Counter")]
    public static void ShowWindow()
    {
        GetWindow<TMPFontCounting>("TMP Font Counter");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        if (GUILayout.Button("Scan Fonts", GUILayout.Height(30)))
        {
            ScanFonts();
        }

        GUILayout.Space(10);

        if (fontUsage.Count == 0)
        {
            EditorGUILayout.HelpBox("Click Scan Fonts to analyze.", MessageType.Info);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var pair in fontUsage)
        {
            TMP_FontAsset font = pair.Key;
            FontUsageData data = pair.Value;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            data.foldout = EditorGUILayout.Foldout(
                data.foldout,
                $"{font.name}  (Used By: {data.users.Count})",
                true
            );

            if (GUILayout.Button("Ping Font", GUILayout.Width(80)))
            {
                EditorGUIUtility.PingObject(font);
            }

            EditorGUILayout.EndHorizontal();

            if (data.foldout)
            {
                EditorGUI.indentLevel++;

                foreach (var tmp in data.users)
                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(tmp.gameObject.name, GUILayout.Width(250)))
                    {
                        Selection.activeObject = tmp.gameObject;
                        EditorGUIUtility.PingObject(tmp.gameObject);
                    }

                    GUILayout.Label(tmp.GetType().Name);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ScanFonts()
    {
        fontUsage.Clear();

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            // Scan whole scene
            ProcessTMPArray(FindObjectsOfType<TextMeshProUGUI>(true));
            ProcessTMPArray(FindObjectsOfType<TextMeshPro>(true));
        }
        else
        {
            // Scan all selected roots
            foreach (GameObject root in selectedObjects)
            {
                ProcessTMPArray(root.GetComponentsInChildren<TextMeshProUGUI>(true));
                ProcessTMPArray(root.GetComponentsInChildren<TextMeshPro>(true));
            }
        }

        Repaint();
    }

    private void ProcessTMPArray<T>(T[] tmps) where T : TMP_Text
    {
        foreach (var tmp in tmps)
        {
            if (tmp.font == null)
                continue;

            if (!fontUsage.ContainsKey(tmp.font))
                fontUsage[tmp.font] = new FontUsageData();

            fontUsage[tmp.font].users.Add(tmp);
        }
    }
}
#endif