using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faisceau : MonoBehaviour
{
    public List<SpriteRenderer> Faisceaux;
    private float[] OffSets;
    public Vector2 frenquency = new Vector2(0.8f,1);
    private float[] frenquencys;
    void Start()
    {
        OffSets = new float[Faisceaux.Count];
        frenquencys = new float[Faisceaux.Count];
        for (int i = 0; i < Faisceaux.Count; i++)
        {
            OffSets[i] = Random.Range(0, Mathf.PI * 2);
            frenquencys[i] = Random.Range(frenquency.x, frenquency.y);
            float n = Mathf.InverseLerp(-1,1,Mathf.Sin(OffSets[i]+Time.time*frenquencys[i]));
            Color c = Faisceaux[i].color;
            c.a = n;
            Faisceaux[i].color = c;
        }
    }
    void Update()
    {
        for(int i = 0; i < Faisceaux.Count; i++)
        {
            float n = Mathf.InverseLerp(-1, 1, Mathf.Sin(OffSets[i]+Time.time*frenquencys[i]));
            Color c = Faisceaux[i].color;
            c.a = n;
            Faisceaux[i].color = c;
        }
    }
    
}
