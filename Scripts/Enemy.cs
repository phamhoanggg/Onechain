using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    float curHP;

    private void Start()
    {
        curHP = maxHP;
    }

    private void Update()
    {
        if (curHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= 3)
        {
            curHP -= other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 5;
        }
        if (GetComponent<Rigidbody2D>().velocity.magnitude >= 3)
        {
            curHP -= GetComponent<Rigidbody2D>().velocity.magnitude * 5;
        }
    }

}
