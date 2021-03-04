using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject HitEffect;

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        GameObject t_Effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(t_Effect,1.5f);
        Destroy(gameObject);
    }

}
