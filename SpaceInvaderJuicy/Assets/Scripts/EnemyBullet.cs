using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 75f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.down);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherTag = other.gameObject.tag;
        if (otherTag == "Player" || otherTag == "Ground" || otherTag == "Shield" || otherTag == "Bullet")
        {
            if (otherTag == "Player" || otherTag == "Bullet")
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
