using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private Transform muzzle;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float respawnTime = 2f;

    private SpriteRenderer sprite;

    private Collider2D col;

    private Vector2 startPos;

    private GameObject currentBullet;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        if (currentBullet == null && Input.GetKey(KeyCode.Space))
        {
            GameObject instantiateBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            currentBullet = instantiateBullet;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.Instance.UpdateLives();
        StopAllCoroutines();
        StartCoroutine(Respawn());
    }

    System.Collections.IEnumerator Respawn()
    {
        enabled = false;
        col.enabled = false;
        ChangeSpriteAlpha(0.0f);

        yield return new WaitForSeconds(0.25f * respawnTime);

        transform.position = startPos;
        enabled = true;
        ChangeSpriteAlpha(0.25f);

        yield return new WaitForSeconds(0.75f * respawnTime);

        ChangeSpriteAlpha(1.0f);
        col.enabled = true;
    }

    private void ChangeSpriteAlpha(float value)
    {
        var color = sprite.color;
        color.a = value;
        sprite.color = color;
    }
}
