using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Rigidbody rigidbody;

    [ShowInInspector]
    Ray? ray;


    public bool IsEnter;
    public bool IsDraging;

    public Vector3 TargetPosition;


    public void Update()
    {
        if (IsEnter || IsDraging)
        {
            rigidbody.isKinematic = true;
        }
        else
        {
            rigidbody.isKinematic = false;
        }

    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag");
        IsDraging = true;

        MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        ray = MainCamera.ScreenPointToRay(new Vector3(eventData.position.x, eventData.position.y, 0.1f));
        // MainCamera.transform.TransformPoint(eventData.position)
        // ray = new Ray(MainCamera.transform.position, MainCamera.transform.TransformDirection(MainCamera.transform.TransformPoint(eventData.position) * 3));
        // Debug.Log($"OnDrag:{ray}");
        if (Physics.Raycast(ray.Value, out RaycastHit hit, 3, LayerMask.GetMask("Table")))
        {
            Debug.Log($"Hit:{hit.collider.gameObject.name}");
            TargetPosition = hit.point + (Vector3.up * 0.01f);
            transform.position = hit.point + (Vector3.up * 0.01f);

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        ray = MainCamera.ScreenPointToRay(new Vector3(eventData.position.x, eventData.position.y, 0.1f));
        // MainCamera.transform.TransformPoint(eventData.position)
        // ray = new Ray(MainCamera.transform.position, MainCamera.transform.TransformDirection(MainCamera.transform.TransformPoint(eventData.position) * 3));
        // Debug.Log($"OnDrag:{ray}");
        if (Physics.Raycast(ray.Value, out RaycastHit hit, 3, LayerMask.GetMask("Table")))
        {
            Debug.Log($"Hit:{hit.collider.gameObject.name}");
            transform.position = hit.point + (Vector3.up * 0.01f);

        }
        else
        {
            Debug.Log($"Not Hit");
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag");
        rigidbody.isKinematic = false;
        IsDraging = false;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick");
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


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
