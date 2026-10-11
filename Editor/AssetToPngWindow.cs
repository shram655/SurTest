using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetToPngWindow : EditorWindow
{
    public GameObject targetPrefab;
    private int iconSize = 512; 

    [Header("Настройки ракурса как на фото")]
    public float angleX = 90f;       // Изначально 90
    public float angleY = -65f;      // Изначально -65
    public float cameraFOV = 40f;    // Изначально 40
    public float distance = -2.5f;   // Изначально -2.5
    public float cameraOffsetY = 1f; // Изначально 1

    [Header("Настройки обводки")]
    public int outlineWidth = 4;     // Ширина обводки в пикселях
    [Range(0.1f, 1f)]
    public float outlineSmoothness = 0.5f; // Плавность сглаживания краев

    [MenuItem("Tools/Префаб в PNG (Сглаженная обводка)")]
    public static void ShowWindow()
    {
        GetWindow<AssetToPngWindow>("Exact Icon Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Создание прозрачных иконок со сглаживанием", EditorStyles.boldLabel);
        
        targetPrefab = (GameObject)EditorGUILayout.ObjectField("Префаб блока/лута", targetPrefab, typeof(GameObject), false);

        EditorGUILayout.Space();
        GUILayout.Label("Тонкая настройка ракурса:", EditorStyles.miniBoldLabel);
        
        angleX = EditorGUILayout.Slider("Наклон X (Вверх/Вниз)", angleX, -90f, 90f);
        angleY = EditorGUILayout.Slider("Поворот Y (Влево/Вправо)", angleY, -180f, 180f);
        cameraFOV = EditorGUILayout.Slider("Перспектива (FOV)", cameraFOV, 10f, 60f);
        distance = EditorGUILayout.Slider("Дистанция (Ближе/Дальше)", distance, -5f, -1f);
        cameraOffsetY = EditorGUILayout.Slider("Высота камеры", cameraOffsetY, -1f, 2f);

        EditorGUILayout.Space();
        GUILayout.Label("Настройка контура:", EditorStyles.miniBoldLabel);
        outlineWidth = EditorGUILayout.IntSlider("Ширина обводки (px)", outlineWidth, 0, 15);
        outlineSmoothness = EditorGUILayout.Slider("Сглаживание краев", outlineSmoothness, 0.1f, 1f);

        EditorGUILayout.Space();

        if (GUILayout.Button("Создать точную PNG иконку") && targetPrefab != null)
        {
            CreateTransparentIcon(targetPrefab);
        }
    }

    private void CreateTransparentIcon(GameObject prefab)
    {
        GameObject tempObj = Instantiate(prefab, Vector3.zero, Quaternion.Euler(angleX, angleY, 0f));
        
        GameObject camObj = new GameObject("TempIconCamera");
        Camera cam = camObj.AddComponent<Camera>();
        
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0f, 0f, 0f, 0f); 
        
        cam.orthographic = false; 
        cam.fieldOfView = cameraFOV; 
        
        camObj.transform.position = new Vector3(0f, cameraOffsetY, distance);
        camObj.transform.LookAt(Vector3.zero);

        RenderTexture rt = new RenderTexture(iconSize, iconSize, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;
        
        Texture2D screenShot = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
        
        cam.Render();
        RenderTexture.active = rt;
        
        screenShot.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        screenShot.Apply();
        
        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        DestroyImmediate(camObj);
        DestroyImmediate(tempObj);

        // ПРИМЕНЕНИЕ СГЛАЖЕННОЙ ОБВОДКИ
        if (outlineWidth > 0)
        {
            screenShot = ApplySmoothWhiteOutline(screenShot, outlineWidth, outlineSmoothness);
        }

        string folderPath = Path.Combine(Application.dataPath, "ItemIcons");
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string localPath = $"Assets/ItemIcons/{prefab.name}_Icon.png";
        string globalPath = Path.Combine(Application.dataPath, $"ItemIcons/{prefab.name}_Icon.png");

        byte[] bytes = screenShot.EncodeToPNG();
        DestroyImmediate(screenShot);
        File.WriteAllBytes(globalPath, bytes);
        
        AssetDatabase.Refresh();

        TextureImporter importer = AssetImporter.GetAtPath(localPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite; 
            importer.alphaIsTransparency = true; 
            
            // Изменено на Bilinear/Trilinear для корректной работы мягкого полупрозрачного сглаживания
            importer.filterMode = FilterMode.Bilinear; 
            importer.SaveAndReimport();
        }

        Debug.Log($"✅ Сглаженная иконка успешно создана! Путь: Assets -> ItemIcons");
    }

    // Алгоритм сглаженной обводки на основе расстояния до объекта
    private Texture2D ApplySmoothWhiteOutline(Texture2D sourceTexture, int width, float smoothness)
    {
        int w = sourceTexture.width;
        int h = sourceTexture.height;
        
        Texture2D resultTexture = new Texture2D(w, h, TextureFormat.RGBA32, false);
        Color[] sourcePixels = sourceTexture.GetPixels();
        Color[] resultPixels = new Color[sourcePixels.Length];
        
        System.Array.Copy(sourcePixels, resultPixels, sourcePixels.Length);

        float maxDist = width;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int currentIndex = y * w + x;
                
                // Обрабатываем только пустые зоны или частично прозрачные края исходника
                if (sourcePixels[currentIndex].a < 0.9f)
                {
                    float minDistance = float.MaxValue;

                    // Ищем ближайший плотный пиксель объекта
                    for (int ny = -width - 1; ny <= width + 1; ny++)
                    {
                        for (int nx = -width - 1; nx <= width + 1; nx++)
                        {
                            int checkX = x + nx;
                            int checkY = y + ny;

                            if (checkX >= 0 && checkX < w && checkY >= 0 && checkY < h)
                            {
                                int neighborIndex = checkY * w + checkX;
                                if (sourcePixels[neighborIndex].a > 0.5f)
                                {
                                    float dist = Mathf.Sqrt(nx * nx + ny * ny);
                                    if (dist < minDistance)
                                    {
                                        minDistance = dist;
                                    }
                                }
                            }
                        }
                    }

                    // Если нашли объект неподалеку, рассчитываем мягкий альфа-канал контура
                    if (minDistance <= maxDist)
                    {
                        // Плавное затухание прозрачности от края объекта к внешней границе
                        float alpha = 1f - Mathf.Clamp01((minDistance - 1f) / (maxDist * smoothness));
                        
                        // Смешиваем оригинальный пиксель с белым контуром
                        if (alpha > sourcePixels[currentIndex].a)
                        {
                            Color outlineColor = Color.white;
                            outlineColor.a = alpha;
                            
                            // Сохраняем цвета текстуры внутри, накладывая обводку под нее
                            resultPixels[currentIndex] = Color.Lerp(outlineColor, sourcePixels[currentIndex], sourcePixels[currentIndex].a);
                        }
                    }
                }
            }
        }

        resultTexture.SetPixels(resultPixels);
        resultTexture.Apply();
        DestroyImmediate(sourceTexture);
        return resultTexture;
    }
}
