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

    private GameObject currentBullet;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
