﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D rb;

    public Transform gunArm;

    

    public Animator anim;

  /*  public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;
    */
    public SpriteRenderer bodySR;


    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCD = 1f, dashInvinc = .5f;

    public int dashSFX;
    [HideInInspector]
    public float dashCounter;
    private float  dashCoolCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();

    [HideInInspector]
    public int currentGun;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
       // theCam = Camera.main;

        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();
            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);

            rb.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }
            //rotate gun hand
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            //shoot the bullet from fire point 
            /* if (Input.GetMouseButtonDown(0))
             {
                 Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                 shotCounter = timeBetweenShots;
                 AudioManager.instance.PlayerSFX(12);
             }
             if (Input.GetMouseButton(0))
             {
                 shotCounter -= Time.deltaTime;

                 if (shotCounter <= 0)
                 {
                     Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                     AudioManager.instance.PlayerSFX(12);
                     shotCounter = timeBetweenShots;
                 }
             }*/

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(availableGuns.Count > 0)
                {
                    currentGun++;
                    if(currentGun >= availableGuns.Count)
                    {
                        currentGun = 0;
                    }

                    SwitchGun();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvincible(dashInvinc);
                    AudioManager.instance.PlayerSFX(dashSFX);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCD;
                }
            }
            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
                anim.SetBool("isMoving", false);


        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }

    }


    public void SwitchGun()
    {
        foreach(Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
