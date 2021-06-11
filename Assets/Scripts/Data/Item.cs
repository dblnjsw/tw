using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string itemName;
    private string itemClass;       //eg:剑、镐子
    private string itemType;        //eg:消耗品、武器、砖块
    private double weaponLength;
    private int damage;
    private int position;
    private int num;

    public string ItemName
    {
        get
        {
            return itemName;
        }

        set
        {
            itemName = value;
        }
    }

    public string ItemClass
    {
        get
        {
            return itemClass;
        }

        set
        {
            itemClass = value;
        }
    }

    public string ItemType
    {
        get
        {
            return itemType;
        }

        set
        {
            itemType = value;
        }
    }

    public double WeaponLength
    {
        get
        {
            return weaponLength;
        }

        set
        {
            weaponLength = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public int Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public int Num
    {
        get
        {
            return num;
        }

        set
        {
            num = value;
        }
    }

    public Item(string itemName, int position = -1, string itemType="block",string itemClass = "block", double weaponLength=0, int damage=0, int num=1)
    {
        this.ItemName = itemName;
        this.ItemClass = itemClass;
        this.ItemType = itemType;
        this.WeaponLength = weaponLength;
        this.Damage = damage;
        this.Position = position;
        this.Num = num;
    }

    public Item()
    {

    }

    

    void fire1()
    {

    }
    void fire2() { }
}
