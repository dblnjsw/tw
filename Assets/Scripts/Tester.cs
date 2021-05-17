using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    List<Item> wl ;
    // Start is called before the first frame update
    void Start()
    {
        wl = new List<Item>();
        wl.Add(new Item("ironSword",0,"weapon","sword",10,10)) ;
        wl.Add(new Item("ironPickax",1,"weapon", "pickax"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            test1();
            Debug.Log("test1");
        }
    }

    void test1()
    {
        SaveComponent sc = new SaveComponent();
        sc.saveByJson<List<Item>>("a",wl);
    }
}
