using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LifeManager : MonoBehaviour
{
    List<Vector3Int> masterBirthPoint;
    public GameObject zombie;
    // Start is called before the first frame update
    void Start()
    {
        masterBirthPoint = GetComponent<TileManager>().masterBirthPoint;
        foreach(Vector3Int v in masterBirthPoint)
        {
            Instantiate(zombie, v, Quaternion.identity);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
