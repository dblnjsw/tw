using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDescribe
{
    private string itemName;
    private string describe;

    public string ItemName { get => itemName; set => itemName = value; }
    public string Describe { get => describe; set => describe = value; }
}
