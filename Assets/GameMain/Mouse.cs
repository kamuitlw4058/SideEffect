using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera m_MainCamera;

    Rigidbody2D rigidbody;

    public List<Collider2D> Triggers = new List<Collider2D>();


    Camera MainCamera
    {
        get
        {
            if (m_MainCamera == null)
            {
                m_MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
            }

            return m_MainCamera;
        }
    }

    [ShowInInspector]
    Ray? ray;


    public bool IsEnter;
    public bool IsDraging;

    public Vector3 TargetPosition;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }


    public void Update()
    {


    }

    public Vector3 DragOffset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D:{other}");
        if (!Triggers.Contains(other))
        {
            Triggers.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"OnTriggerExit2D:{other}");

        if (Triggers.Contains(other))
        {
            Triggers.Remove(other);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag");
        gameObject.layer = LayerMask.NameToLayer("Drag");
        // rigidbody.simulated = false;
        // IsDraging = true;
        // rigidbody

        // MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        // ray = MainCamera.ScreenPointToRay(new Vector3(eventData.position.x, eventData.position.y, 0.1f));
        // // MainCamera.transform.TransformPoint(eventData.position)
        // // ray = new Ray(MainCamera.transform.position, MainCamera.transform.TransformDirection(MainCamera.transform.TransformPoint(eventData.position) * 3));
        // // Debug.Log($"OnDrag:{ray}");
        // if (Physics.Raycast(ray.Value, out RaycastHit hit, 3, LayerMask.GetMask("Table")))
        // {
        //     Debug.Log($"Hit:{hit.collider.gameObject.name}");
        //     TargetPosition = hit.point + (Vector3.up * 0.01f);
        //     transform.position = hit.point + (Vector3.up * 0.01f);

        // }

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = MainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10f)) + DragOffset;
        // ray = MainCamera.ScreenPointToRay(new Vector3(eventData.position.x, eventData.position.y, 0.1f));
        // ray = new Ray(MainCamera.transform.position, MainCamera.transform.TransformDirection(MainCamera.transform.TransformPoint(eventData.position) * 3));
        // // // Debug.Log($"OnDrag:{ray}");
        // if (Physics.Raycast(ray.Value, out RaycastHit hit, 3, LayerMask.GetMask("Table")))
        // {
        //     Debug.Log($"Hit:{hit.collider.gameObject.name}");
        //     transform.position = MainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0.1f));


        // }
        // else
        // {
        //     Debug.Log($"Not Hit");
        // }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag");
        gameObject.layer = LayerMask.NameToLayer("Default");


        // rigidbody.isKinematic = false;
        // IsDraging = false;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick");
        DragOffset = transform.position - MainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10f));

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"OnPointerEnter");
        IsEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"OnPointerExit");
        IsEnter = false;

    }



    // Update is called once per frame

    private void OnDrawGizmos()
    {
        if (ray != null)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(ray.Value);
            Gizmos.color = oldColor;
        }


    }


}
