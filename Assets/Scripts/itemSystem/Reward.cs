using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Reward : MonoBehaviour
{
    Vector3 trans1;//记录原位置
    Vector3 trans2;//简谐运动变化的位置，计算得出

    public float zhenFu = 0.5f;//振幅
    public float HZ = 1f;//频率
    // Start is called before the first frame update
    void Start()
    {
        trans2 = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        trans2.y = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + transform.parent.position.y + 0.3f;

        transform.position = trans2;
    }
}
