using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    public float waitToCollect = .5f;

    private void Start()
    {
        
    }
    // Start is called before the first frame update
    void Update()
    {
        if (waitToCollect > 0)
        {
            waitToCollect -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToCollect <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);

            Destroy(gameObject);
            AudioManager.instance.PlayerSFX(5);
        }
    }
}
