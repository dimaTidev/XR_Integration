using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Camera))]
public class XR_MouseInteractor : MonoBehaviour
{
    [SerializeField] AInteractor interactor;

    Camera m_cam;
    Camera Cam => m_cam ??= GetComponent<Camera>();
    // protected override void Update()
    // {
    //     base.Update();
    //     if (Input.GetMouseButtonDown(0))
    //         Pickup_byGrip();
    //
    //     if (Input.GetMouseButtonUp(0))
    //         Drop();
    // }
    //
    // protected override Ray Get_Ray() => Cam.ScreenPointToRay(Input.mousePosition);


    [SerializeField] LayerMask rayMask;
    [SerializeField] Transform targetMove;
    [SerializeField] Vector3 m_offset;

    Vector3 offset => Cam.transform.rotation * m_offset;// Cam.transform.TransformPoint(m_offset);

    float grabDistance;

    Vector3 mouseLoseOffset;

    private void Update() => ReadInputs();

    void ReadInputs()
    {
        if (!targetMove)
            return;

       

        if (Input.GetKeyDown(KeyCode.R))
            mouseLoseOffset = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) - mouseLoseOffset;

        if (Input.GetKeyUp(KeyCode.R))
            mouseLoseOffset = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) - mouseLoseOffset;

        if (Input.GetKey(KeyCode.R)) //handRotation state
        {
            float x = Input.GetAxis("Mouse Y") * 3;
            float y = Input.GetAxis("Mouse X") * 3;
            float z = Input.mouseScrollDelta.y * 3;

            targetMove.Rotate(x, y, z, Space.World);
           // targetMove.Rotate(z, x, y, Space.World);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
                mouseLoseOffset = Vector3.zero;

            grabDistance += Input.mouseScrollDelta.y * Time.deltaTime * 3;
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabDistance) - mouseLoseOffset;
            targetMove.position = Cam.ScreenToWorldPoint(currentScreenPoint) + offset;

            if (Input.GetMouseButtonDown(0))
            {
                interactor.Pickup_byGrip();
                grabDistance = Vector3.Distance(targetMove.position, Cam.transform.position);
            }

            if (Input.GetMouseButtonDown(2))
            {
                RaycastHit hit;
                if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, 100, rayMask))
                    grabDistance = Vector3.Distance(hit.point, Cam.transform.position);
            }


            if (Input.GetMouseButtonUp(0))
                interactor.Drop();
        }
    }

    private void OnGUI()
    {
        Rect rect = new Rect(10,10,400, 100);
        GUI.Box(rect, "");
        GUI.Label(rect, " For Snap hand to the surface click mouseButton(2)\n For Change distance to hand use mouseScroll\n For rotate hand hold Hold key `R` and use mouse X,Y axis with mouseScroll\n Use key Q for reset position to cursor ");
    }
}
