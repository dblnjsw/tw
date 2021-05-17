using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Tilemaps;
using System.Linq;

public class Equipment : MonoBehaviour
{
    Grid grid;
    Tilemap map;
    TileManager tm;
    UIEvent uIEvent;

    public int bagVolume = 6;

    UIManagaer ui;
    Movement movement;
    Life life;

    public Dictionary<string,GameObject> dic_itemsIns;   //从dataPool获取的所有物品的实例
    public Dictionary<string, Item> dic_items;          //从dataPool获取的所有物品

    public List<Item> il;
    public Dictionary<string, Item> dic_myItem;        //缓存il，用于优化查询
    //Item currentItem;
    SaveComponent sc;
    int currentEqu = 0;
    GameObject lastEqu;


    List<GameObject> pickableItems;

    float attackStartTime, attackCD = 0.5f;
    bool canAttack = true;





    // Start is called before the first frame update
    void Start()
    {
        uIEvent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UIEvent>();
        ui = GameObject.FindGameObjectWithTag("ItemCells").GetComponent<UIManagaer>();
        life = GetComponentInChildren<Life>();
        movement = GetComponent<Movement>();
        pickableItems = new List<GameObject>();
        gameObject.SetActive(true);

        GameObject g_data = GameObject.FindGameObjectWithTag("DataPool");
        DataPool dataPool = g_data.GetComponent<DataPool>();
        dic_itemsIns = dataPool.dic_itemsIns;
        dic_items = dataPool.dic_items;

        //初始化已有的背包装备
        sc = new SaveComponent();
        il = new List<Item>();
        for(int i = 0; i < bagVolume; i++)
        {
            il.Add(new Item("empty",i));
        }

        dic_myItem = new Dictionary<string, Item>();        
        List<Item> originBag =sc.loadByJson<List<Item>>("a");
        foreach (Item item in originBag)
        {
            enterBag(item);
        }


        GameObject g = GameObject.FindGameObjectWithTag("tag");
        grid=g.GetComponent<Grid>();
        map = g.GetComponentInChildren<Tilemap>();
        tm = g.GetComponentInChildren<TileManager>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            fire1();
        }
        else if(Input.GetButtonDown("Fire2") && canAttack)
        {
            fire2();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            pickAll();
        }
        else if(Input.GetKey(KeyCode.Alpha1) && canAttack)
        {
            swapEqu(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && canAttack)
        {
            swapEqu(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3) && canAttack)
        {
            swapEqu(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4) && canAttack)
        {
            swapEqu(4);
        }
        else if (Input.GetKey(KeyCode.Alpha5) && canAttack)
        {
            swapEqu(5);
        }
        else if (Input.GetKey(KeyCode.Alpha6) && canAttack)
        {
            swapEqu(6);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uIEvent.Show_pop_ecs();
        }

    }

    private void LateUpdate()
    {
        //冷却结束
        if (Time.time - attackStartTime > attackCD && !canAttack)
        {
            canAttack = true;
            dic_itemsIns[il[currentEqu].ItemName].transform.Rotate(0, 0, 90);
        }
    }

    void swapEqu(int index)
    {
        currentEqu = index - 1;
        itemChange();
    }

    void itemChange()
    {
        Item w = il[currentEqu];
        if(dic_myItem.ContainsKey(w.ItemName))
            if (!lastEqu)
            {
                lastEqu = dic_itemsIns[w.ItemName];
                dic_itemsIns[w.ItemName].SetActive(true);

            }
            else if (lastEqu!= dic_itemsIns[w.ItemName])
            {
                lastEqu.SetActive(false);
                dic_itemsIns[w.ItemName].SetActive(true);
                lastEqu = dic_itemsIns[w.ItemName];
            }
            else
            {

            }
        else if(w.ItemName=="empty")
        {
            if (!lastEqu)
                return;
            lastEqu.SetActive(false);
            lastEqu = null;
        }
    }

    //新增物品到背包
    void enterBag(Item item)
    {
        //成为player的子物体,映射到dic_itemsIns
        if (dic_itemsIns[item.ItemName].transform.parent != transform || !dic_itemsIns[item.ItemName].transform.parent)
        {
            Vector3 v = new Vector3(transform.position.x + 0.4f, transform.position.y - 0.2f, transform.position.z);
            GameObject go = GameObject.Instantiate(dic_itemsIns[item.ItemName], v, Quaternion.identity, transform);
            go.SetActive(false);
            dic_itemsIns[item.ItemName] = go;
        }

        //更新逻辑背包：il和dic_itemsIns
        if (dic_myItem.ContainsKey(item.ItemName))
        {
            //该物品已拥有，增加数量
            dic_myItem[item.ItemName].Num += 1;
            ui.changeItemNum(dic_myItem[item.ItemName]);
        }
        else
        {
            //如果是新物品
            if (item.Position == -1)
            {
                for (int i = 0; i < bagVolume; i++)
                {
                    if (il[i].ItemName == "empty")
                    {
                        il[i] = item;
                        il[i].Position = i;
                        ui.enterItem(il[i]);
                        break;
                    }
                }
            }
            else
            {
                //如果是存档下物品
                il[item.Position] = item;
            }
            dic_myItem[item.ItemName] = il[item.Position];
        }

    }

    public void showPickable(GameObject g)
    {
        pickableItems.Add(g);

    }
    public void nshowPickable(GameObject g)
    {
        pickableItems.Remove(g);
    }
    public void pickAll()
    {
        foreach(GameObject g in pickableItems)
        {
            if (g == null)
            {
                pickableItems.Remove(g);
                continue;
            }
            string[] splits=g.name.Split('_');
            enterBag(dic_items[splits[1]]);
        }
        foreach (GameObject g in pickableItems)
        {
            g.GetComponentInChildren<Dropable>().afterPick();
        }
        pickableItems.Clear();

    }
    public void pickA()
    {

    }

    void consumeItem()
    {
        if (il[currentEqu].Num == 1)
        {
            //从背包中删除物品
            dic_itemsIns[il[currentEqu].ItemName].SetActive(false);
            dic_myItem.Remove(il[currentEqu].ItemName);
            il[currentEqu] = new Item("empty", currentEqu);
            ui.exitItem(currentEqu);
        }
        else
        {
            il[currentEqu].Num -= 1;
            ui.changeItemNum(il[currentEqu]);
        }
    }

    void fire1()
    {
        if (il[currentEqu].ItemName=="empty")
            return;
        string type = il[currentEqu].ItemClass;
        if (type == "pickax")
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("world pos:"+MyFunc.toString(worldPos));
            Vector3Int cellPos = grid.WorldToCell(worldPos);
            Debug.Log("grid pos:" + MyFunc.toString(cellPos));

            tm.destroyTile(cellPos);
        }
        else if (type=="sword")
        {
            RaycastHit2D hit;
            if(transform.localScale.x>0)
                hit = Physics2D.Raycast(new Vector2(transform.position.x + 1,transform.position.y), Vector2.right,2,LayerMask.GetMask("life"));
            else
                hit = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y), Vector2.left, 2, LayerMask.GetMask("life"));

            
            if (hit.collider != null)
            {
                float ori = transform.position.x - hit.collider.transform.position.x;
                hit.collider.gameObject.SendMessage("getDamage",new Life.DamageMessage(ori,il[currentEqu].Damage));
            }

            life.beImmu(0.3f);
            canAttack = false;
            AttackAnim();
            attackStartTime = Time.time;
            

        }
        else if (type == "block")
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("world pos:" + MyFunc.toString(worldPos));
            Vector3Int cellPos = grid.WorldToCell(worldPos);
            Debug.Log("grid pos:" + MyFunc.toString(cellPos));

            bool isSuccess =tm.createTile(cellPos, il[currentEqu].ItemName);

            if (isSuccess)
                consumeItem();

        }
    }

    void fire2()
    {
    }

    void AttackAnim()
    {
        dic_itemsIns[il[currentEqu].ItemName].transform.Rotate(0, 0, -90);
        if (transform.localScale.x > 0)
        {
            movement.moveLockEvent(new Vector2(20, 0), 0.1f, false);
        }
        else
        {
            movement.moveLockEvent(new Vector2(-20, 0), 0.1f, false);

        }
    }
    //private void OnGUI()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
    //        {
    //            if (Input.GetKeyDown(keyCode))
    //            {
    //                Debug.LogError("Current Key is : " + keyCode.ToString());
    //            }
    //        }
    //    }
    //}
}
