using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour {
    private string targetTag;
    public float speed = 10;
    public float damage;

    private void Awake()
    {
        targetTag = "Enemy";
    }

    void Start () {
        GameObject.Destroy(this.gameObject, 5);
    }

	void Update () {
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
       
	}

    public void SetSpeed(float newSpeed)            //총의 종류에 따라 총알 속도 설정
    {
        speed = newSpeed;
    }

    public void SetTarget(string target)
    {
        targetTag = target;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void OnHitObject(GameObject hit)                                    //맞을시 Bullet 제거
    {
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)                  //총알에 맞은 collision의 tag를 확인 후 적일시 데미지, 아닐시 그냥 파괴
    {
        if(collider.gameObject.tag == targetTag)
        {
            OnHitObject(collider.gameObject);
            IDamageble damagebleObject = collider.gameObject.GetComponent<IDamageble>();
            if(damagebleObject != null)
            {
                damagebleObject.TakeHit(damage);
                
            }
            
        }
        else if(collider.gameObject.tag == "Player_")               //Player 캐릭터와 총알이 충돌시 통과
        {
            //OnHitObject(collider.gameObject);
        }
        else if(collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "Scene" && collider.gameObject.tag != "area1" && collider.gameObject.tag != "area2")
        {
            OnHitObject(collider.gameObject);
        }
    }
}
