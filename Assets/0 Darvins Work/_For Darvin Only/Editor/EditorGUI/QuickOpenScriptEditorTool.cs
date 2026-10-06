#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.ShortcutManagement;
namespace Darvin.EditorTool
{

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ScriptHeaderButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawScriptHeaderButton();
            DrawDefaultInspector();
        }

        void DrawScriptHeaderButton()
        {
            MonoBehaviour mono = (MonoBehaviour)target;
            MonoScript script = MonoScript.FromMonoBehaviour(mono);

            if (script == null) return;

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            // Script name (like default header feel)
            GUILayout.Label("Open Script");

            GUILayout.FlexibleSpace();

            // Small icon button
            // GUIContent icon = EditorGUIUtility.IconContent("Open Icon");
            // icon.tooltip = "Open Script";

            if (GUILayout.Button(mono.GetType().Name))
            {
                AssetDatabase.OpenAsset(script);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    public class OpenAllAttachedScriptsShortcut
    {
        // [Shortcut("Custom/Open All Attached Scripts", KeyCode.Q, ShortcutModifiers.Action)] // Ctrl+Q
        static void OpenAllScripts()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects == null || selectedObjects.Length == 0)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }

            int openedCount = 0;

            foreach (var go in selectedObjects)
            {
                var components = go.GetComponents<MonoBehaviour>();

                foreach (var comp in components)
                {
                    if (comp == null) continue;

                    MonoScript script = MonoScript.FromMonoBehaviour(comp);
                    if (script != null)
                    {
                        AssetDatabase.OpenAsset(script);
                        openedCount++;
                    }
                }
            }

            if (openedCount == 0)
            {
                Debug.LogWarning("No scripts found on selected GameObjects.");
            }
            else
            {
                Debug.Log($"Opened {openedCount} script(s).");
            }
        }
    }
}
#endif