using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLife : Life
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Life l = collision.transform.GetComponentInChildren<Life>();
            float ori = collision.transform.position.x - transform.position.x;
            l.getDamage(new DamageMessage(-ori));
        }
    }
}
