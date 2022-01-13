using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corail : MonoBehaviour
{
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            hp--;

                GetComponent<SpriteRenderer>().color = Color.red;
                StartCoroutine(whitecolor());
            

            if (hp <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
        Destroy(other.gameObject);
    }

    IEnumerator whitecolor()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;

    }
}
