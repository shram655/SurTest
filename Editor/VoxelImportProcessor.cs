using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Автоматическая настройка ассетов из папки Assets/VoxelExport:
//  - после импорта .obj создаётся persistent-материал .mat с шейдером
//    Custom/CubeWorldStyle и текстурой палитры в _MainTex, и модель на него ссылается;
//  - PNG-палитра импортируется как Point-текстура без сжатия и мипмапов.
// Создание ассетов выполняется СТРОГО после импорта (EditorApplication.delayCall),
// иначе Unity ругается "restricted during asset importing".
public class VoxelImportProcessor : AssetPostprocessor
{
    const string ExportFolder = "VoxelExport";

    static readonly Queue<string> pending = new Queue<string>();
    static bool scheduled;

    // Вызывается, когда модель уже импортирована. Здесь создавать ассеты нельзя —
    // просто запоминаем путь и откладываем работу до конца импорта.
    void OnPostprocessModel(GameObject go)
    {
        if (!IsOurAsset()) return;

        pending.Enqueue(assetPath);
        if (!scheduled)
        {
            scheduled = true;
            EditorApplication.delayCall += ProcessPending;
        }
    }

    // Настройка импорта текстуры палитры: резкие пиксели, без сжатия и мипмапов.
    void OnPreprocessTexture()
    {
        if (!IsOurAsset()) return;

        var ti = (TextureImporter)assetImporter;
        ti.filterMode = FilterMode.Point;
        ti.mipmapEnabled = false;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        ti.sRGBTexture = true;
    }

    static void ProcessPending()
    {
        scheduled = false;
        while (pending.Count > 0)
        {
            string objPath = pending.Dequeue();
            try
            {
                ApplyMaterial(objPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"VoxelImportProcessor: {e}");
            }
        }
    }

    static void ApplyMaterial(string objPath)
    {
        var shader = Shader.Find("Custom/CubeWorldStyle");
        if (shader == null) return;

        string dir = Path.GetDirectoryName(objPath).Replace('\\', '/');
        string name = Path.GetFileNameWithoutExtension(objPath);

        // Папка Materials рядом с моделью.
        string matFolder = dir + "/Materials";
        if (!AssetDatabase.IsValidFolder(matFolder))
            AssetDatabase.CreateFolder(dir, "Materials");
        string matPath = matFolder + "/" + name + ".mat";

        // Persistent-материал: создаём один раз, дальше переиспользуем.
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (mat == null)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, matPath); // вне импорта — разрешено
        }

        mat.shader = shader;
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(dir + "/" + name + "_palette.png");
        if (tex != null) mat.SetTexture("_MainTex", tex);
        EditorUtility.SetDirty(mat);

        // Привязываем наш материал к импортированной модели.
        var model = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
        if (model != null)
        {
            foreach (var r in model.GetComponentsInChildren<Renderer>(true))
            {
                var mats = r.sharedMaterials;
                for (int i = 0; i < mats.Length; i++) mats[i] = mat;
                r.sharedMaterials = mats;
            }
            EditorUtility.SetDirty(model);
        }

        AssetDatabase.SaveAssets(); // вне импорта — разрешено
    }

    bool IsOurAsset()
    {
        return assetPath != null && assetPath.Replace('\\', '/').Contains(ExportFolder);
    }
}