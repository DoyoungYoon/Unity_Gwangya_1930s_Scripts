using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{

    public GameObject bullet_Prefab;

    public Default_Gun arisaka;
    public Transform shootPoint;

    private float shootTime = 0;

    void Start()
    {
        // arisakaSound = GameObject.Find("arisaka").GetComponent<AudioSource>();

        if (arisaka == null)
        {
            arisaka = GetComponentInChildren<Default_Gun>();
        }
    }

    public void Shoot()
    {
        shootTime += Time.deltaTime;

        if (shootTime >= 1f)
        {
            // GetComponentInChildren<Muzzle_Flash>().Activate();
            GetComponentInChildren<Soldier_Animation>().Shoot();

            arisaka.Shoot_Enemy();
            //arisakaSound.PlayOneShot(arisakaSound.clip);
            //GameObject bullet = Instantiate(bullet_Prefab, shootPoint.position, shootPoint.rotation);
            // bullet.GetComponent<Bullet>().SetTarget("Player_");
            shootTime = 0f;
        }

    }
}
