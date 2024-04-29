using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pangoo.Core.VisualScripting;
using Pangoo.Core.Common;
using UnityEditor;
using Sirenix.OdinInspector;

public class CardDrag : BaseImmersed, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Camera MainCamera;

    public Transform Target;

    public Rigidbody cardRigidbody;

    public TargetSlot Slot;
    // Start is called before the first frame update
    void Start()
    {
        cardRigidbody = GetComponent<Rigidbody>();
        if (MainCamera == null)
        {
            MainCamera = ShortcutMainCamera.Instance;
        }


    }

    string CaseClueHasVariable;

    [ShowInInspector]
    public bool Has
    {
        get
        {
            if (CaseClueHasVariable == null)
            {
                CaseClueHasVariable = dynamicObject.Main.GameConfig.GameMainConfig.CaseClueHasVariable;
            }

            if (CaseClueHasVariable != null && dynamicObject != null)
            {
                return dynamicObject.GetVariable<bool>(CaseClueHasVariable);
            }

            return false;
        }
    }

    public bool IsDraging;

    // Update is called once per frame
    void Update()
    {
        if (Has && !IsDraging)
        {
            cardRigidbody.isKinematic = false;
        }
        else
        {
            cardRigidbody.isKinematic = true;
        }
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


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter.:{other.gameObject.name}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            Target = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"OnTriggerExit.:{other.gameObject.name}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Target") && other.gameObject.transform == Target)
        {
            Target = null;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag");
        IsDraging = true;
        gameObject.layer = LayerMask.NameToLayer("Drag");
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

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log($"OnEndDrag");
        if (Target != null)
        {
            transform.position = Target.position;
            var TargetSlot = Target.GetComponent<TargetSlot>();
            if (TargetSlot != null)
            {

                if (Slot != null)
                {
                    Slot.CardDragInstance = null;
                }
                Slot = TargetSlot;
                TargetSlot.CardDragInstance = this;
            }
        }
        else
        {
            if (Slot != null)
            {
                Slot.CardDragInstance = null;
            }
        }
        gameObject.layer = LayerMask.NameToLayer("Card");
        IsDraging = false;

    }
}

