using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("colli");
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        collision.gameObject.SendMessage("showPickable");
    //    }
    //}
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("showPickable",transform.parent.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("nshowPickable", transform.parent.gameObject);
        }
    }


    public void afterPick()
    {
        Destroy(transform.parent.gameObject,0.2f);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
