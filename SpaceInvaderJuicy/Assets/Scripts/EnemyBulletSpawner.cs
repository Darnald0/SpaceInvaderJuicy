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

    private float timer;
    private float currentTime;
    [SerializeField] private Transform followTarget;
    public void Setup()
    {
        currentTime = Random.Range(minShootCD, maxShootCD);
        followTarget = Swarm.Instance.GetInvader(currentRow, column);
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

        Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
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

        followTarget.GetComponentInChildren<SpriteRenderer>().enabled = false;
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
            Destroy(other.gameObject);
    }
}
