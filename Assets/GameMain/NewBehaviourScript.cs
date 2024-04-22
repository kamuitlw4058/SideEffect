using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
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


    [Button]
    public void Show()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void OnDrawGizmos()
    {
        MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        Ray ray = new Ray(MainCamera.transform.position, MainCamera.transform.TransformDirection(Vector3.forward * 3));
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);


        var tableLayer = LayerMask.NameToLayer("Table");
        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // Debug.Log($"Hit:{hit.collider.gameObject.name}, Hit Point:{hit.point}");
            Gizmos.DrawSphere(hit.point, 0.01f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point + Vector3.up * 0.03f, 0.01f);

        }


        Gizmos.color = oldColor;



    }
}
