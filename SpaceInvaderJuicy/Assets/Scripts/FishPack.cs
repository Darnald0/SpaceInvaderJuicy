using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPack : MonoBehaviour
{
    public float speed = 2f;
    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 10f;
    public float minY = -10f;

    private float tChange = 0f;
    public float minMoveDirCD = 0.5f;
    public float maxMoveDirCD = 1.5f;
    private float randomX;
    private float randomY;
    public Vector3 dir;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-2.0f, 2.0f);
            randomY = Random.Range(-2.0f, 2.0f);

            tChange = Time.time + Random.Range(minMoveDirCD, maxMoveDirCD);
        }

        Vector3 goToPos = new Vector3(randomX, randomY, 0);
        transform.Translate(goToPos * speed * Time.deltaTime);

        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }

        Vector3 border = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), 0);
        transform.position = border;

        Vector3 currentPos = transform.position;

         dir = goToPos - currentPos;


        if(dir.normalized.x <= 0)
        {
            sr.flipX = true;
        } else
        {
            sr.flipX = false;
        }

    }
}
