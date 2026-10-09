using UnityEngine;

#if UNITY_WEBGL
public class WebGLNGUIFix : MonoBehaviour
{
    private UICamera uiCamera;
    
    void Start()
    {
        uiCamera = GetComponent<UICamera>();
        if (uiCamera == null) return;
        
        // Принудительно настраиваем для WebGL
        uiCamera.useMouse = true;
        uiCamera.useTouch = true;  // WebGL поддерживает тач через мышь
        uiCamera.allowMultiTouch = true;
        
        // Важно для WebGL
        uiCamera.stickyTooltip = false;
        
        Debug.Log("[WebGL] NGUI UICamera настроен");
    }
    
    void Update()
    {
        // Принудительно обновляем позицию мыши для NGUI
        if (Input.mousePosition != Vector3.zero)
        {
            UICamera.lastEventPosition = Input.mousePosition;
        }
    }
}
#endif