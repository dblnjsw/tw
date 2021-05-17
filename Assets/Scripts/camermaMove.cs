using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camermaMove : MonoBehaviour
{
    public Transform player;
    public float Ahead;//当角色向右移动时，摄像机比任务位置领先，当角色向左移动时，摄像机比角色落后
    public Vector3 Targetpos;//摄像机的最终目标
    public float smooth;//摄像机平滑移动的值
    Vector3 Pos;


    // Use this for initialization
    void Start()
    {
        float s_baseOrthographicSize = Screen.height / 64.0f / 2.0f;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;
        Targetpos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if (player.transform.localScale.x > 0)
        {
            Targetpos = new Vector3(player.transform.position.x + Ahead, transform.position.y, transform.position.z);
        }
        if (player.transform.localScale.x < 0)
        {
            Targetpos = new Vector3(player.transform.position.x - Ahead, transform.position.y, transform.position.z);
        }
        //让摄像机进行平滑的移动
        transform.position = Vector3.Lerp(transform.position, Targetpos, smooth);
    }

    void LateUpdate()
    {
        Pos = player.transform.position - gameObject.transform.position;
        Pos.z = 0;    //摄像机的图层不能变化，所以z一直是0
        gameObject.transform.position += Pos / 20;
    }

}

