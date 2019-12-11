﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;


    [Header("Chase Player")]
    public bool shouldChase;
    public float rangeToChasePlayer;
    private Vector3 moveDir;

    [Header("Run Away")]
    public bool shouldRun;
    public float runawayRange;
    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;
    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;


    [Header("Shooting")]
    

    public bool shouldShoot;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public SpriteRenderer body;
    public float shootRange;

    public int deathSFX;
    public int damageSFX;
    [Header("Variables")]
    public Animator anim;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    public int health = 150;

    public bool shouldDropItem;
    public GameObject[] itemsDrop;
    public float itemDropPercent;
    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(body.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDir = Vector3.zero;
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChase  )
            {
                moveDir = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDir = wanderDirection;

                    if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDir = patrolPoints[currentPatrolPoint].position - transform.position;
                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }

                }
            }

        if(shouldRun && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDir = transform.position - PlayerController.instance.transform.position;
            }
       
      /*  else
        {
            moveDir = Vector3.zero;
        }*/

        moveDir.Normalize();

        theRB.velocity = moveDir * moveSpeed;



            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange) 
        {
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                AudioManager.instance.PlayerSFX(13);

                }
        }


    }
        else
        {
            theRB.velocity = Vector2.zero;
        }
        
        if (moveDir != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
            anim.SetBool("isMoving", false);

    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlayerSFX(damageSFX);
        Instantiate(hitEffect, transform.position, transform.rotation);
        if(health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlayerSFX(deathSFX);
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f,0f,rotation * 90));

            //drop items
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsDrop.Length);
                    Instantiate(itemsDrop[randomItem], transform.position, transform.rotation);
                }
            }
            //Instantiate(deathSplatters, transform.position, transform.rotation);
        }
    }
}
