using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pangoo.Core.VisualScripting;
using Pangoo.Core.Common;
using UnityEditor;

public class TargetSlot : BaseImmersed, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Camera MainCamera;

    public CardDrag CardDragInstance;

    void Start()
    {
        SlotManager.RegisterTargetSlot(this);
    }

    public bool CheckInstance
    {
        get
        {
            return CardDragInstance != null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"OnPointerEnter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick Combine");


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"OnPointerExit");
    }

}

