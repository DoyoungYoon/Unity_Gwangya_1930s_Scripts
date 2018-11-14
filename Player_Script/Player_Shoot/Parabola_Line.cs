using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola_Line : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public float rotationSpeed = 4.04f;
    public Transform grenadeHold;
    Rigidbody rigidbody;

    Vector3 center = Vector3.zero;
    Vector3 theArc = Vector3.zero;

    void Start()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 25;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (null == Camera.main)
            return;

        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitdist = 0.0f;
        Vector3 targetPoint = Vector3.zero;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            targetPoint = ray.GetPoint(hitdist);

            center = (transform.position + targetPoint) * 0.5f;
            center.y -= 70.0f;

            Quaternion targetRotation = Quaternion.LookRotation(center - transform.position);
             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            RaycastHit hitInfo;

            if (Physics.Linecast(transform.position, targetPoint, out hitInfo))
            {
                targetPoint = hitInfo.point;
            }
        }
        else
        {
            targetPoint = transform.position;
        }

        Vector3 RelCenter = transform.position - center;
        Vector3 aimRelCenter = targetPoint - center;

        for (float index = 0.0f, interval = -0.0417f; interval < 1.0f;)
        {
            theArc = Vector3.Slerp(RelCenter, aimRelCenter, interval += 0.0417f);
            lineRenderer.SetPosition((int)index++, theArc + center);
        }
    }

    void ParabolaFormula()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        float g = Physics.gravity.y;
        float t = Time.deltaTime;
    }

    public void ThrowGranade()
    {

        transform.position = grenadeHold.position;
        transform.rotation = grenadeHold.rotation;
        rigidbody.AddForce(new Vector3(0, 50, 0));
    }
}


