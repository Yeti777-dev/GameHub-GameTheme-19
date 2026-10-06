using UnityEditor;
using UnityEngine;

# if UNITY_EDITOR

public class EditorWindowTools : EditorWindow
{
    [MenuItem("Tools/Darvin/Texture/Change Selected Folder Texture Max Size/64")]
    public static void ChangeTextureMaxSizeTo64()
    {
        int maxTextureSize = 64;

        // Get selected objects (can be folders or assets)
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No folder selected!");
            return;
        }

        int changedCount = 0;

        foreach (Object obj in selectedObjects)
        {
            string folderPath = AssetDatabase.GetAssetPath(obj);

            // Skip if not a folder
            if (!AssetDatabase.IsValidFolder(folderPath))
                continue;

            // Find textures ONLY inside this folder
            string[] textureGuids = AssetDatabase.FindAssets("t:Texture", new[] { folderPath });

            foreach (string guid in textureGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null)
                {
                    importer.maxTextureSize = maxTextureSize;
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    changedCount++;
                }
            }
        }

        Debug.Log($"Updated {changedCount} textures to max size {maxTextureSize}");
    }
    [MenuItem("Tools/Darvin/Texture/Change Selected Folder Texture Max Size/128")]
    public static void ChangeTextureMaxSizeTo128()
    {
        int maxTextureSize = 128;

        // Get selected objects (can be folders or assets)
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No folder selected!");
            return;
        }

        int changedCount = 0;

        foreach (Object obj in selectedObjects)
        {
            string folderPath = AssetDatabase.GetAssetPath(obj);

            // Skip if not a folder
            if (!AssetDatabase.IsValidFolder(folderPath))
                continue;

            // Find textures ONLY inside this folder
            string[] textureGuids = AssetDatabase.FindAssets("t:Texture", new[] { folderPath });

            foreach (string guid in textureGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null)
                {
                    importer.maxTextureSize = maxTextureSize;
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    changedCount++;
                }
            }
        }

        Debug.Log($"Updated {changedCount} textures to max size {maxTextureSize}");
    }
    [MenuItem("Tools/Darvin/Texture/Change Selected Folder Texture Max Size/512")]
    public static void ChangeSelectedTextureMaxSizeTo512()
    {
        int maxTextureSize = 512;

        // Get selected objects (can be folders or assets)
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No folder selected!");
            return;
        }

        int changedCount = 0;

        foreach (Object obj in selectedObjects)
        {
            string folderPath = AssetDatabase.GetAssetPath(obj);

            // Skip if not a folder
            if (!AssetDatabase.IsValidFolder(folderPath))
                continue;

            // Find textures ONLY inside this folder
            string[] textureGuids = AssetDatabase.FindAssets("t:Texture", new[] { folderPath });

            foreach (string guid in textureGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null)
                {
                    importer.maxTextureSize = maxTextureSize;
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    changedCount++;
                }
            }
        }

        Debug.Log($"Updated {changedCount} textures to max size {maxTextureSize}");
    }
    [MenuItem("Tools/Darvin/Texture/Change All Texture Max Size/512")]
    public static void ChangeTextureMaxSizeTo512()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture"); // Find all textures in the project
        int maxTextureSize = 512;// Set Max Size
        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.maxTextureSize = maxTextureSize;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        Debug.Log("All textures' max size has been set to " + maxTextureSize);
    }
    [MenuItem("Tools/Darvin/Texture/Change All Texture Max Size/1024")]
    public static void ChangeTextureMaxSizeTo1024()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture"); // Find all textures in the project
        int maxTextureSize = 1024;// Set Max Size
        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.maxTextureSize = maxTextureSize;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        Debug.Log("All textures' max size has been set to " + maxTextureSize);
    }
    [MenuItem("Tools/Darvin/Texture/Change All Texture Max Size/2048")]
    public static void ChangeTextureMaxSizeTo2048()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture"); // Find all textures in the project
        int maxTextureSize = 2048;// Set Max Size
        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.maxTextureSize = maxTextureSize;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        Debug.Log("All textures' max size has been set to " + maxTextureSize);
    }
    [MenuItem("Tools/Darvin/Texture/Change All Texture Max Size/4096")]
    public static void ChangeTextureMaxSizeTo4096()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture"); // Find all textures in the project
        int maxTextureSize = 4096;// Set Max Size
        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.maxTextureSize = maxTextureSize;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        Debug.Log("All textures' max size has been set to " + maxTextureSize);
    }
    [MenuItem("Tools/Darvin/Remove All Colliders")]
    private static void RemoveAllColliders()
    {
        RemoveAllMeshColliders();
        RemoveAllBoxColliders();
        RemoveAllCapsuleColliders();
        RemoveAllSphereColliders();
    }
    [MenuItem("Tools/Darvin/Remove All Mesh Colliders")]
    private static void RemoveAllMeshColliders()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            Debug.LogWarning("Select a parent object first!");
            return;
        }

        MeshCollider[] colliders = parent.GetComponentsInChildren<MeshCollider>(true);

        foreach (MeshCollider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        Debug.Log($"Removed {colliders.Length} Mesh Colliders from {parent.name}");
    }

    [MenuItem("Tools/Darvin/Remove All Box Colliders")]
    private static void RemoveAllBoxColliders()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            Debug.LogWarning("Select a parent object first!");
            return;
        }

        BoxCollider[] colliders = parent.GetComponentsInChildren<BoxCollider>(true);

        foreach (BoxCollider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        Debug.Log($"Removed {colliders.Length} Box Colliders from {parent.name}");
    }

    [MenuItem("Tools/Darvin/Remove All Capsule Colliders")]
    private static void RemoveAllCapsuleColliders()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            Debug.LogWarning("Select a parent object first!");
            return;
        }

        CapsuleCollider[] colliders = parent.GetComponentsInChildren<CapsuleCollider>(true);

        foreach (CapsuleCollider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        Debug.Log($"Removed {colliders.Length} Capsule Colliders from {parent.name}");
    }
    [MenuItem("Tools/Darvin/Remove All Sphere Colliders")]
    private static void RemoveAllSphereColliders()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            Debug.LogWarning("Select a parent object first!");
            return;
        }

        SphereCollider[] colliders = parent.GetComponentsInChildren<SphereCollider>(true);

        foreach (SphereCollider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        Debug.Log($"Removed {colliders.Length} Sphere Colliders from {parent.name}");
    }

    [MenuItem("Tools/Darvin/Remove All Mesh Filter & Mesh Renderer")]
    private static void RemoveAllMeshRenderer()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            Debug.LogWarning("Select a parent object first!");
            return;
        }

        MeshRenderer[] Meshesh = parent.GetComponentsInChildren<MeshRenderer>(true);
        MeshFilter[] MeshFilter = parent.GetComponentsInChildren<MeshFilter>(true);

        foreach (MeshRenderer Renderer in Meshesh)
        {
            DestroyImmediate(Renderer);
        }
        foreach (MeshFilter Filter in MeshFilter)
        {
            DestroyImmediate(Filter);
        }

        Debug.Log($"Removed {Meshesh.Length} Mesh Renderer from {parent.name}");
        Debug.Log($"Removed {MeshFilter.Length} Mesh Filter from {parent.name}");
    }
    // [MenuItem("Tools/Darvin/Make Box Mesh from colliders")]
    // private static void MakeBoxMeshFromColliders()
    // {
    //     GameObject parent = Selection.activeGameObject;

    // }
    [MenuItem("Tools/Darvin/Collider to Mesh Generator")]
    public static void ShowWindow()
    {
        GetWindow<EditorWindowTools>("Collider to Mesh");
    }

    private string parentName = "GeneratedWalls";
    private Mesh boxMesh;
    private Material wallMaterial;
    private bool keepOriginalColliders = true;

    private void OnGUI()
    {
        GUILayout.Label("Box Collider to Mesh Converter", EditorStyles.boldLabel);

        parentName = EditorGUILayout.TextField("Parent Object Name:", parentName);
        wallMaterial = (Material)EditorGUILayout.ObjectField("Wall Material:", wallMaterial, typeof(Material), false);
        keepOriginalColliders = EditorGUILayout.Toggle("Keep Original Colliders:", keepOriginalColliders);
        boxMesh = (Mesh)EditorGUILayout.ObjectField("Box mesh:", boxMesh, typeof(Mesh), false);
        // boxMesh = EditorGUILayout.Rende("Keep Original Colliders:", boxMesh);

        if (GUILayout.Button("Generate Meshes from Selected Colliders"))
        {
            GenerateMeshesFromSelected();
        }
    }

    private void GenerateMeshesFromSelected()
    {
        // Get all selected objects with BoxColliders

        GameObject parent = Selection.activeGameObject;


        // BoxCollider[] selectedColliders = Selection.GetFiltered<BoxCollider>(SelectionMode.Editable | SelectionMode.TopLevel);
        BoxCollider[] selectedColliders = parent.GetComponentsInChildren<BoxCollider>();

        if (selectedColliders.Length == 0)
        {
            Debug.LogWarning("No BoxColliders selected!");
            return;
        }

        // Create parent object
        GameObject parentObject = new GameObject(parentName);
        Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent Object");

        foreach (BoxCollider collider in selectedColliders)
        {
            CreateMeshFromCollider(collider, parentObject.transform);
        }

        Debug.Log($"Generated {selectedColliders.Length} wall meshes under {parentName}");
    }

    private void CreateMeshFromCollider(BoxCollider collider, Transform parent)
    {
        // Create new GameObject
        GameObject meshObject = new GameObject($"{collider.gameObject.name}_Mesh");
        meshObject.transform.SetParent(parent);

        // Position and scale to match collider
        meshObject.transform.position = collider.transform.position + collider.center;
        // meshObject.transform.position += ;
        meshObject.transform.rotation = collider.transform.rotation;

        // Add mesh components
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();

        // Create box mesh matching the collider
        // Mesh boxMesh = new Mesh();
        Vector3 size = collider.size;
        meshObject.transform.localScale = size;


        // boxMesh.RecalculateNormals();

        // Assign mesh
        meshFilter.sharedMesh = boxMesh;
        meshCollider.sharedMesh = boxMesh;

        // Assign material if specified
        if (wallMaterial != null)
        {
            meshRenderer.sharedMaterial = wallMaterial;
        }

        // Optionally remove original collider
        if (!keepOriginalColliders)
        {
            Undo.DestroyObjectImmediate(collider);
        }

        Undo.RegisterCreatedObjectUndo(meshObject, "Create Mesh from Collider");

        // Create new GameObject
        // GameObject meshObject = new GameObject($"{collider.gameObject.name}_Mesh");
        // meshObject.transform.SetParent(parent);

        // // Position and scale to match collider
        // meshObject.transform.position = collider.transform.position;
        // meshObject.transform.position -= collider.center;
        // meshObject.transform.rotation = collider.transform.rotation;
        // meshObject.transform.localScale = collider.transform.lossyScale;

        // // Add mesh components
        // MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        // MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
        // MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();

        // // Create box mesh matching the collider
        // Mesh boxMesh = new Mesh();
        // Vector3 size = collider.size;

        // // Vertices (8 vertices of a box)
        // Vector3[] vertices = new Vector3[8]
        // {
        //     new Vector3(-size.x/2, -size.y/2, -size.z/2),
        //     new Vector3(size.x/2, -size.y/2, -size.z/2),
        //     new Vector3(size.x/2, size.y/2, -size.z/2),
        //     new Vector3(-size.x/2, size.y/2, -size.z/2),
        //     new Vector3(-size.x/2, -size.y/2, size.z/2),
        //     new Vector3(size.x/2, -size.y/2, size.z/2),
        //     new Vector3(size.x/2, size.y/2, size.z/2),
        //     new Vector3(-size.x/2, size.y/2, size.z/2)
        // };

        // // Triangles (12 triangles forming a box)
        // int[] triangles = new int[36]
        // {
        //     0, 2, 1, 0, 3, 2, // back
        //     1, 2, 6, 6, 5, 1, // right
        //     4, 5, 6, 6, 7, 4, // front
        //     0, 7, 3, 0, 4, 7, // left
        //     0, 1, 5, 5, 4, 0, // bottom
        //     2, 3, 7, 7, 6, 2  // top
        // };

        // // UVs (simple mapping)
        // Vector2[] uv = new Vector2[8]
        // {
        //     new Vector2(0, 0),
        //     new Vector2(1, 0),
        //     new Vector2(1, 1),
        //     new Vector2(0, 1),
        //     new Vector2(0, 0),
        //     new Vector2(1, 0),
        //     new Vector2(1, 1),
        //     new Vector2(0, 1)
        // };

        // boxMesh.vertices = vertices;
        // boxMesh.triangles = triangles;
        // boxMesh.uv = uv;
        // boxMesh.RecalculateNormals();

        // // Assign mesh
        // meshFilter.sharedMesh = boxMesh;
        // meshCollider.sharedMesh = boxMesh;

        // // Assign material if specified
        // if (wallMaterial != null)
        // {
        //     meshRenderer.sharedMaterial = wallMaterial;
        // }

        // // Optionally remove original collider
        // if (!keepOriginalColliders)
        // {
        //     Undo.DestroyObjectImmediate(collider);
        // }

        // Undo.RegisterCreatedObjectUndo(meshObject, "Create Mesh from Collider");
    }
}
#endif
