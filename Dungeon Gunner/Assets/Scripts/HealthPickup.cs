using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    public float waitToCollect = 0.5f;
    public int pickupSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitToCollect > 0)
        {
            waitToCollect -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && waitToCollect <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);

            Destroy(gameObject);
            AudioManager.instance.PlayerSFX(pickupSFX);
        }
    }
}
