using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleAxisClickability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Camera secondaryCamera;
    
    // Detect if mouse is hovering over this UI element
    public void OnPointerEnter(PointerEventData eventData) {
        secondaryCamera.depth = 0;
    }

    public void OnPointerExit(PointerEventData eventData) {
        secondaryCamera.depth = -2;
    }
}