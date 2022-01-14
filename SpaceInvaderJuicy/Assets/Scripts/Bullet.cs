using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 75f;

    private Vector2 explodePos;
    public PlayerController player;
    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
        if (player.activeParticle && particle.isStopped)
        {
            particle.Play();
        }
        else if (!player.activeParticle)
        {
            particle.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherTag = other.gameObject.tag;
        if (otherTag == "Enemy" || otherTag == "Ceiling" || otherTag == "Shield" || otherTag == "Bullet")
        {
            if (otherTag == "Bullet")
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
