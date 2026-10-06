#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class DarvinPlayerPrefsTools
{
    [MenuItem("Tools/Darvin/Player Prefs/Delete all Data")]
    private static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs and saved screenshots have been deleted.");
    }
}
#endif
