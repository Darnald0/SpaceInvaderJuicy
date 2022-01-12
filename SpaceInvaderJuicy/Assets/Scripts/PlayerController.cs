using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    public float maxRadiusTurn;
    public float speedTurn;

    float speed;
    float speedV;
    [SerializeField]
    public float maxSpeed;
    [SerializeField]
    public float secondToDecelerate;
    [SerializeField]
    float secondToMaxSpeed;
    float moveHz = 0;
    float moveVrt = 0;

    private ParticleSystem bubble;

    public bool betterMovement;
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
        if (!betterMovement)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                transform.Translate(0, maxSpeed * Time.deltaTime * 0.7f, 0);

            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -maxSpeed * Time.deltaTime * 0.7f, 0);

            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(maxSpeed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-maxSpeed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            moveHz = Input.GetAxis("Horizontal");
            moveVrt = Input.GetAxis("Vertical");

            #region Acceleration
            if (Input.GetKey(KeyCode.D))
            {
                if (speed < maxSpeed)
                {
                    speed += (maxSpeed / secondToMaxSpeed) * Time.deltaTime;
                }
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (speed > -maxSpeed)
                {
                    speed -= (maxSpeed / secondToMaxSpeed) * Time.deltaTime;
                }
                if (speed < -maxSpeed)
                {
                    speed = -maxSpeed;
                }
            }

            if (Input.GetKey(KeyCode.Z))
            {
                if (speedV < maxSpeed)
                {
                    speedV += (maxSpeed / secondToMaxSpeed) * Time.deltaTime;
                }
                if (speedV > maxSpeed)
                {
                    speedV = maxSpeed;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (speedV > -maxSpeed)
                {
                    speedV -= (maxSpeed / secondToMaxSpeed) * Time.deltaTime;
                }
                if (speedV < -maxSpeed)
                {
                    speedV = -maxSpeed;
                }
            }
            #endregion
            #region Decelerate
            if (moveHz == 0)
            {
                if (speed < 0)
                {
                    speed += (maxSpeed / secondToDecelerate) * Time.deltaTime;
                }
                else if (speed > 0)
                {
                    speed -= (maxSpeed / secondToDecelerate) * Time.deltaTime;
                }
                else
                {
                    speed = 0;
                }
            }
            if (moveVrt == 0)
            {
                if (speedV < 0)
                {
                    speedV += (maxSpeed / secondToDecelerate) * Time.deltaTime;
                }
                else if (speedV > 0)
                {
                    speedV -= (maxSpeed / secondToDecelerate) * Time.deltaTime;
                }
                else
                {
                    speedV = 0;
                }
            }
            #endregion
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y + speedV * Time.deltaTime);
        }
        if (transform.position.y > startPos.y + maxDistanceUp)
        {
            transform.position = new Vector2(transform.position.x, startPos.y + maxDistanceUp);
        }
        if (transform.position.y < startPos.y - maxDistanceDown)
        {
            transform.position = new Vector2(transform.position.x, startPos.y - maxDistanceDown);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            float turn = speedTurn * Time.deltaTime;
            if (transform.rotation.eulerAngles.z%360 + turn > maxRadiusTurn && (transform.rotation.eulerAngles.z + turn) < 360- maxRadiusTurn)
            {
                turn = maxRadiusTurn - transform.rotation.eulerAngles.z;
            }
            this.transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + turn);
        }
        if (Input.GetKey(KeyCode.D))
        {
            float turn = -speedTurn * Time.deltaTime;
            float temp = transform.rotation.eulerAngles.z;
            if ((temp%360 + turn) <360 -maxRadiusTurn && (temp + turn) > maxRadiusTurn)
            {
                turn = -maxRadiusTurn - temp;
            }
            this.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + turn);
        }
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.S))
        {
            if (transform.rotation.z < 0)
            {
                float turn = speedTurn * Time.deltaTime;
                if (transform.rotation.z + turn > 0)
                {
                    turn = -transform.rotation.z;
                }
                this.transform.rotation = new Quaternion(0, 0, transform.rotation.z + turn, 0);
            }
            else
            {
                float turn = -speedTurn * Time.deltaTime;
                if (transform.rotation.z + turn < 0)
                {
                    turn = -transform.rotation.z;
                }
                this.transform.rotation = new Quaternion(0, 0, transform.rotation.z + turn, 0);
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
