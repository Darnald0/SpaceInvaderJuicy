using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.25f;

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

    public float maxDistanceUp;
    public float maxDistanceDown;

    private Rigidbody2D rb2D;
    float moveHz=0;
    float moveVrt=0;
    float acceleration = 1;
    float MinMovement = 0.1f;

    private ParticleSystem bubble;

    public bool betterMovement;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!betterMovement)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                transform.Translate(0, speed * Time.deltaTime * 0.7f, 0);
                if (transform.position.y > startPos.y + maxDistanceUp)
                {
                    transform.position = new Vector2(transform.position.x, startPos.y + maxDistanceUp);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -speed * Time.deltaTime * 0.7f, 0);
                if (transform.position.y < startPos.y - maxDistanceDown)
                {
                    transform.position = new Vector2(transform.position.x, startPos.y - maxDistanceDown);
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if (rb2D)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    //moveHz += acceleration * Input.GetAxis("Horizontal");
                    moveHz = Input.GetAxis("Horizontal") * 2;
                    if (moveHz > 2)
                    {
                        moveHz = 2;
                    }
                    else if (moveHz < -2)
                    {
                        moveHz = -2;
                    }
                }
                else
                {
                    moveHz /= (2 * (1 + Time.deltaTime));
                    if (moveHz < MinMovement)
                        moveHz = 0;
                }

                if (Input.GetAxis("Vertical") != 0)
                {
                    moveVrt = Input.GetAxis("Vertical") * 2;
                    if (moveVrt > 2)
                    {
                        moveVrt = 2;
                    }
                    else if (moveVrt < -2)
                    {
                        moveVrt = -2;
                    }
                }
                else
                {
                    moveVrt /= (2 * (1 + Time.deltaTime));
                    if (moveVrt < MinMovement)
                        moveVrt = 0;
                }

                rb2D.velocity = new Vector2(moveHz * speed, moveVrt * speed);
            }
        }
        if (currentBullet == null && Input.GetKey(KeyCode.Space))
        {
            GameObject instantiateBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            currentBullet = instantiateBullet;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet" || other.gameObject.name == "BulletSpawner(Clone)")
        {
            GameManager.Instance.UpdateLives();
            StopAllCoroutines();
            StartCoroutine(Respawn());
        }
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
