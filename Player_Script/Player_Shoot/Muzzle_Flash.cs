using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle_Flash : MonoBehaviour {

    public GameObject muzzleFlashHolder;
    public Sprite[] muzzleFlashSprites;
    public SpriteRenderer[] spriteRenderers;

    public float muzzleFlashTime;

	void Start () {
        Deactivate();
        muzzleFlashTime = 0.05f;
    }
	
    public void Activate()
    {
        muzzleFlashHolder.SetActive(true);
        int muzzleFlashSpriteIndex = Random.Range(0, muzzleFlashSprites.Length);
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = muzzleFlashSprites[muzzleFlashSpriteIndex];
        }
        Invoke("Deactivate", muzzleFlashTime);
    }
	
    void Deactivate()
    {
        muzzleFlashHolder.SetActive(false);
    }
}
