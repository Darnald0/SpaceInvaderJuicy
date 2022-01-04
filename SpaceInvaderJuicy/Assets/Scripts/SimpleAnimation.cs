using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
