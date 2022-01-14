using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    internal int currentRow;
    internal int column;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float minShootCD = 1;

    [SerializeField]
    private float maxShootCD = 10;

    [SerializeField]
    private GameObject[] bulletType;

    private float timer;
    private float currentTime;
    [SerializeField] private Transform followTarget;

    private AudioSource sourceSFX;
    public bool activateSFX;
    public bool activateAnim;

    private void Start()
    {
        sourceSFX = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (followTarget)
        {
            transform.position = followTarget.position;
        }

        timer += Time.deltaTime;
        if (timer < currentTime)
        {
            return;
        }

        switch (followTarget.gameObject.name)
        {
            case "Fugu(Clone)":
                Instantiate(bulletType[0], spawnPoint.position, Quaternion.identity);
                break;
            case "Shark(Clone)":
                Instantiate(bulletType[1], spawnPoint.position, Quaternion.identity);
                break;
            case "Lantern(Clone)":
                Instantiate(bulletType[2], spawnPoint.position, Quaternion.identity);
                break;
            default:
                Debug.Log(followTarget.gameObject.name);
                break;
        }

        timer = 0f;
        currentTime = Random.Range(minShootCD, maxShootCD);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (!other.collider.GetComponent<Bullet>())
        {
            return;
        }

        GameManager.Instance.UpdateScore(Swarm.Instance.GetPoints(followTarget.gameObject.name));
        Swarm.Instance.IncreaseDeathCount();

        //followTarget.GetComponentInChildren<SpriteRenderer>().enabled = false;
        StartCoroutine(OnDeath(followTarget.gameObject));
        Swarm.Instance.currentNumberOfInvader--;
        currentRow = currentRow - 1;
        if (currentRow < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Setup();
        }

        if (other.gameObject.tag == "Bullet")
        {
            if (activateSFX)
            {
                sourceSFX.PlayOneShot(sourceSFX.clip);
            }
            Destroy(other.gameObject);
        }
    }

    public void Setup()
    {
        currentTime = Random.Range(minShootCD, maxShootCD);
        followTarget = Swarm.Instance.GetInvader(currentRow, column);
    }

    IEnumerator OnDeath(GameObject invader)
    {
        if (activateAnim)
        {
            var renderer = invader.GetComponent<SpriteRenderer>();
            Color color = renderer.color;
            for (float i = 1.0f; i >= -0.05f; i -= 0.05f)
            {
                Color c = renderer.color;
                c.a = i;
                renderer.color = c;

                //invader.transform.localScale -= new Vector3(i,i,0);

                if (Swarm.Instance.isMovingRight)
                {
                    invader.transform.position += new Vector3(i, 0, 0);
                }
                else
                {
                    invader.transform.position -= new Vector3(i, 0, 0);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        invader.SetActive(false);
        yield return null;
    }
}
