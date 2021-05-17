using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.position = transform.position;
        eventData.pointerDrag.transform.parent = transform;
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
