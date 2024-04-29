using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pangoo.Core.VisualScripting;
using Pangoo.Core.Common;
using UnityEditor;
using MetaTable;

public class CombineButton : BaseImmersed, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Camera MainCamera;

    public Transform Target;



    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log($"OnPointerEnter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick Combine");
        var dynamicObjectUuid1 = string.Empty;
        var dynamicObjectUuid2 = string.Empty;


        foreach (var slot in SlotManager.targetSlots)
        {
            if (slot.CheckInstance)
            {
                var index = SlotManager.targetSlots.IndexOf(slot);
                if (index == 0)
                {
                    dynamicObjectUuid1 = slot.CardDragInstance.dynamicObject.Row.Uuid;
                }
                if (index == 1)
                {
                    dynamicObjectUuid2 = slot.CardDragInstance.dynamicObject.Row.Uuid;

                }
            }
            else
            {
                Debug.Log($"Check Slot:{slot.name} is false");

            }
        }

        if (!dynamicObjectUuid1.IsNullOrWhiteSpace() && !dynamicObjectUuid2.IsNullOrWhiteSpace())
        {
            dynamicObject.Main.Case.Combine(dynamicObjectUuid1, dynamicObjectUuid2);
        }

        dynamicObject.StartExecuteEvent();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log($"OnPointerExit");
    }


    private void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;

        if (MainCamera == null)
        {
            MainCamera = ShortcutMainCamera.Instance;
        }

        var mouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        // Gizmos.DrawLine(mouseRay.origin, mouseRay.origin + (mouseRay.direction * 20));

        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, 20, LayerMask.GetMask("Table")))
        {
            Gizmos.DrawLine(mouseRay.origin, hitInfo.point);
            Gizmos.DrawSphere(hitInfo.point + hitInfo.normal.normalized * 0.1f, 0.1f);

        }


        Gizmos.color = oldColor;
    }
}

