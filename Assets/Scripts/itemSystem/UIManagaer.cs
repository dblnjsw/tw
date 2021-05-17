using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagaer : MonoBehaviour
{
    public List<GameObject> itemCells;
    public GameObject prefabItem;
    public GameObject player;
    List<Item> items;
    Dictionary<string, GameObject> dic_itemIns;
    // Start is called before the first frame update
    void Start()
    {
        dic_itemIns = GameObject.FindGameObjectWithTag("DataPool").GetComponent<DataPool>().dic_itemsIns;
        items = player.GetComponent<Equipment>().il;
        foreach (Item item in items)
        {
            if (item.ItemName != "empty")
            {
                prefabItem.GetComponent<Image>().sprite = dic_itemIns[item.ItemName].GetComponent<SpriteRenderer>().sprite;
                GameObject.Instantiate(prefabItem, itemCells[item.Position].transform.position, Quaternion.identity, itemCells[item.Position].transform);

            }
        }
    }

    public void enterItem(Item item)
    {
        prefabItem.GetComponent<Image>().sprite = dic_itemIns[item.ItemName].GetComponent<SpriteRenderer>().sprite;
        GameObject.Instantiate(prefabItem, itemCells[item.Position].transform.position, Quaternion.identity, itemCells[item.Position].transform);
    }

    public void changeItemNum(Item item)
    {
        if (itemCells[item.Position].transform.childCount==0)
        {
            enterItem(item);
        }
        itemCells[item.Position].transform.GetChild(0).GetComponentInChildren<Text>().text = item.Num.ToString();
    }

    public void exitItem(int pos)
    {
        GameObject.Destroy(itemCells[pos].transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ItemChange()
    {
        
    }
}
