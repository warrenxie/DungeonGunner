using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{

    public float moveSpeed = 3f;
    private Vector3 moveDir;

    public float deceleration = 5f;
    public float lifeTime = 3f;
    public SpriteRenderer sr;
    public float fading = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        moveDir.x = Random.Range(-moveSpeed, moveSpeed);
        moveDir.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * Time.deltaTime;
        moveDir = Vector3.Lerp(moveDir, Vector3.zero, deceleration * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0f, fading * Time.deltaTime));
            if (sr.color.a == 0f) {
                Destroy(gameObject);
            }
        }
    }
}
