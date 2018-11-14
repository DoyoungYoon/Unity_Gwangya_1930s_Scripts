using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    //public LineRenderer lineRenderer;
    public Transform grenadeHold;
    public Rigidbody rigidbody = new Rigidbody();
    public float delay=5f;
    float countDown;
    public float explosionRadius;
    private float maxGrenadeDamage;
    private float explosionPower;                       //폭발 반작용 힘
    public GameObject explosionEffect;
    public GameObject explosionSound;
    private bool isExplosed;
    
    Vector3 force;
    void Awake()
    {
        /*lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 25;*/
        rigidbody = GetComponent<Rigidbody>();
        maxGrenadeDamage = 200f;
        explosionRadius = 5f;
        explosionPower = 700f; 
    }
    void Start()
    {
        countDown = delay;
    }

    void FixedUpdate()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0 && !isExplosed)
        {
            Explosion();
        }
    }

    public void ThrowGrenade(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        force = GetVelocity(currentPos, targetPos, initialAngle);
        rigidbody.AddForce(force, ForceMode.Impulse);
        rigidbody.AddTorque(currentPos, ForceMode.Impulse);
    }

    Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)                  //마우스 커서 위치로 수류탄 투척
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - targetPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
    public void Explosion()
    {
        isExplosed = true;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Instantiate(explosionSound, transform.position, transform.rotation);
        Collider[] findObject = Physics.OverlapSphere(transform.position, explosionRadius);      //적 수류탄 폭발 반경에 있는지 확인
        foreach (Collider col in findObject)                                                                                   
        {
            float r = (col.transform.position - transform.position).magnitude;
            float grenadeDamage = maxGrenadeDamage / (explosionRadius * explosionRadius)*(r - explosionRadius) * (r - explosionRadius); //거리당 2차함수 곡선으로 데미지 감소
            grenadeDamage = Mathf.Round(grenadeDamage);
            Destructible destructibleThings = col.GetComponent<Destructible>();
            if (destructibleThings != null)
            {
                destructibleThings.Destroy();
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionPower, transform.position, explosionRadius);
            }
            if (col.tag.Equals("Enemy"))
            {
                if (col.GetComponent<Enemy_Life>() != null)
                {

                    col.GetComponent<Enemy_Life>().TakeHit(grenadeDamage);
                    Debug.Log(grenadeDamage);
                }
            }
            if (col.tag.Equals("Player_"))
            {
                if (col.GetComponent<Player_Life>() != null)
                {
                    col.GetComponent<Player_Life>().TakeHit(grenadeDamage);
                }
            }
        }

        Destroy(gameObject);
    }

    public void SetDamage(float _range)
    {
        explosionRadius = _range;
    }


}


