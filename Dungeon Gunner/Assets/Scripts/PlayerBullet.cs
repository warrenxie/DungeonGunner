using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impact;

    public int damageToGive = 50;

    public int impactSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impact, transform.position, transform.rotation);
        Destroy(gameObject);
        AudioManager.instance.PlayerSFX(impactSFX);
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
        if(other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToGive);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
