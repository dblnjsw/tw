using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Doozy.Engine.UI;

public class Life : MonoBehaviour
{
    public int life = 10;
    Movement movement;
    bool isMove, isImmu, isDie;
    float immuTime,immuStartTime;
    ParticleSystem particle;


    public class DamageMessage
    {
        public int damage;
        public float ori;
        public DamageMessage(float o, int d = 10)
        {
            damage = d;
            ori = o;
        }

    }
    // Start is called before the first frame update
    private void Start()
    {
        movement = GetComponentInParent<Movement>();
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.localScale.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        if (Time.time - immuStartTime > immuTime)
        {
            isImmu = false;
        }
    }

    public void beImmu(float time)
    {
        immuTime = time;
        immuStartTime = Time.time;
        isImmu = true;
    }
    public void getDamage(DamageMessage dm)
    {
        if (isImmu)
            return;
        life -= dm.damage;
        if (life <= 0 && !isDie)
        {
            died();
        }
        tanShe(dm.ori);
    }
    void died()
    {
        isDie = true;
        if (transform.parent.tag == "Player")
        {
            //ui
            //UIPopup pop = UIPopup.GetPopup("die");
            //pop.Show();

        }
        transform.parent.position = new Vector3(transform.position.x, transform.position.y, -1);
        GetComponentInParent<BoxCollider2D>().enabled = false;
        
        Destroy(transform.parent.gameObject, 1);
    }
    void tanShe(float ori)
    {
        if (ori > 0)
        {
            movement.moveLockEvent(new Vector2(-5,15), .5f, true);
        }
        else
        {
            movement.moveLockEvent(new Vector2(5, 15), .5f, true);
        }

        particle.Play();
    }
}
