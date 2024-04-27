using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pangoo.Core.VisualScripting;
using Pangoo.Core.Common;
using UnityEditor;

public class CardDrag : BaseImmersed, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"OnPointerEnter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"OnPointerExit");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"OnDrag");
        var mouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, 20, LayerMask.GetMask("Table")))
        {
            // Gizmos.DrawLine(mouseRay.origin, hitInfo.point);
            // Gizmos.DrawSphere(hitInfo.point + hitInfo.normal.normalized * 0.1f, 0.1f);
            transform.position = hitInfo.point + hitInfo.normal.normalized * 0.1f;
        }


        // Gizmos.DrawLine(mouseRay.origin, mouseRay.origin + (mouseRay.direction * 20));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag");
    }
}

