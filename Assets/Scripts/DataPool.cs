using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool : MonoBehaviour
{
    public List<GameObject> rewardsIns;
    public List<GameObject> itemsIns;
    public Dictionary<string, GameObject> dic_rewards;
    public Dictionary<string, GameObject> dic_itemsIns;
    Dictionary<string, Object> dic_drop;

    public Dictionary<string, Item> dic_items;


    // Start is called before the first frame update
    void Start()
    {
        //初始化所有物品
        dic_items = new Dictionary<string, Item>();
        dic_itemsIns = new Dictionary<string, GameObject>();
        dic_rewards = new Dictionary<string, GameObject>();
        dic_items["golden"] = new Item("golden");
        dic_items["redStone"] = new Item("redStone");
        dic_items["diamond"] = new Item("diamond");
        dic_items["ground"] = new Item("ground");





        foreach (GameObject g in itemsIns)
        {
            dic_itemsIns.Add(g.name,g);
        }
        foreach (GameObject g in rewardsIns)
        {
            dic_rewards.Add(g.name, g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
