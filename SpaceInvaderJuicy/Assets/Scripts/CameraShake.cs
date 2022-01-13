using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float x;
    public float y;
    public float time;
    public float magnitude;

    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.localPosition;
    }

    public void StartShake()
    {
        StartCoroutine(Shake());
    }
    
    public void StopShake()
    {
        StopAllCoroutines();
    }

    private IEnumerator Shake()
    {
        float timer = 0.0f;
        while (timer < time)
        {
            float xTemp = Random.Range(-x, x) * magnitude;
            float yTemp = Random.Range(-y, y) * magnitude;

            transform.localPosition = new Vector3(xTemp, yTemp+startPos.y, startPos.z);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPos;
    }
}
