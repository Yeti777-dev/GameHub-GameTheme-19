using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class WindowLockShortcut
{
    // Alt + Q (change &q to %l for Ctrl+L, etc.)
    [MenuItem("Tools/Toggle Window Lock &q")]
    private static void ToggleWindowLock()
    {
        // Find the window under the cursor or the focused window
        EditorWindow win = EditorWindow.mouseOverWindow;
        if (win == null) win = EditorWindow.focusedWindow;
        if (win == null) return;  // No window

        // Get the runtime type and name
        System.Type windowType = win.GetType();
        string typeName = windowType.Name;

        // Use the UnityEditor assembly for reflection
        Assembly unityEditorAssembly = typeof(Editor).Assembly;

        if (typeName == "InspectorWindow")
        {
            // Inspector lock is a public property
            PropertyInfo prop = windowType.GetProperty("isLocked",
                BindingFlags.Instance | BindingFlags.Public);
            if (prop != null)
            {
                bool locked = (bool)prop.GetValue(win, null);
                prop.SetValue(win, !locked, null);
                win.Repaint();
            }
        }
        else if (typeName == "ProjectBrowser")
        {
            // ProjectBrowser lock (non-public property)
            PropertyInfo prop = windowType.GetProperty("isLocked",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null)
            {
                bool locked = (bool)prop.GetValue(win, null);
                prop.SetValue(win, !locked, null);
                win.Repaint();
            }
        }
        else if (typeName == "SceneHierarchyWindow")
        {
            // Hierarchy lock: get m_SceneHierarchy then its isLocked
            FieldInfo field = windowType.GetField("m_SceneHierarchy",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                object sceneHierarchyObj = field.GetValue(win);
                if (sceneHierarchyObj != null)
                {
                    System.Type sceneHierarchyType = sceneHierarchyObj.GetType();
                    PropertyInfo prop = sceneHierarchyType.GetProperty("isLocked",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (prop != null)
                    {
                        bool locked = (bool)prop.GetValue(sceneHierarchyObj, null);
                        prop.SetValue(sceneHierarchyObj, !locked, null);
                        win.Repaint();
                    }
                }
            }
        }
        else
        {
            Debug.Log($"'{typeName}' does not support locking.");
        }
    }

    // MenuItem validation: enable only when a window exists
    [MenuItem("Tools/Toggle Window Lock &q", true)]
    private static bool ToggleWindowLockValidate()
    {
        return EditorWindow.mouseOverWindow != null || EditorWindow.focusedWindow != null;
    }
}