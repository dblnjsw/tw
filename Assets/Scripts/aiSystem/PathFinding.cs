using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    class Plat
    {
        public Vector3Int left, right;
        int height, length;


        public Plat(Vector3Int leftPoint, Vector3Int rightPoint, int[] edge)
        {
            left = new Vector3Int(leftPoint.x, leftPoint.y + 1, leftPoint.z);
            right = new Vector3Int(rightPoint.x + 1, rightPoint.y + 1, rightPoint.z);

            length = rightPoint.x - leftPoint.x;
            height = rightPoint.y;
        }

        public Plat(Vector3Int leftPoint, Vector3Int rightPoint, int[] edge1, int[] edge2)
        {
            left = new Vector3Int(leftPoint.x, leftPoint.y + 1, leftPoint.z);
            right = new Vector3Int(rightPoint.x + 1, rightPoint.y + 1, rightPoint.z);

            length = rightPoint.x - leftPoint.x;
            height = rightPoint.y;
        }
    }

    Plat sPlat, dPlat;
    List<Plat> allNearPlat;
    Vector2Int rightTop;
    int r;


    Grid grid;
    Tilemap tileMap;

    void IsPlatReachable()
    {

    }

    /// <summary>
    /// 返回四个int值。1/2代表是否存在左/右边缘点，3/4代表左/右边缘点偏移量。不可站立返回null
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    private int[] SetEdgePoint(Vector3Int tilePos, float playerHeight)
    {
        if (!tileMap.GetTile(tilePos))
            return null;

        int[] edgePoint = { 0, 0, 0, 0 };
        int height = (int)playerHeight + 1;
        //上方瓦片，第一个数字为x偏移量，_代表负
        TileBase near01 = tileMap.GetTile(new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z));
        TileBase near02 = tileMap.GetTile(new Vector3Int(tilePos.x, tilePos.y + 2, tilePos.z));
        TileBase near03 = tileMap.GetTile(new Vector3Int(tilePos.x, tilePos.y + 3, tilePos.z));
        TileBase[] nearTops = { near01, near02, near03 };
        //左上和右上瓦片
        TileBase near11 = tileMap.GetTile(new Vector3Int(tilePos.x + 1, tilePos.y + 1, tilePos.z));
        TileBase near12 = tileMap.GetTile(new Vector3Int(tilePos.x + 1, tilePos.y + 2, tilePos.z));
        TileBase near13 = tileMap.GetTile(new Vector3Int(tilePos.x + 1, tilePos.y + 3, tilePos.z));
        TileBase[] nearTopRights = { near11, near12, near13 };


        TileBase near_11 = tileMap.GetTile(new Vector3Int(tilePos.x - 1, tilePos.y + 1, tilePos.z));
        TileBase near_12 = tileMap.GetTile(new Vector3Int(tilePos.x - 1, tilePos.y + 2, tilePos.z));
        TileBase near_13 = tileMap.GetTile(new Vector3Int(tilePos.x - 1, tilePos.y + 3, tilePos.z));
        TileBase[] nearTopLefts = { near_11, near_12, near_13 };


        //左边和右边瓦片
        TileBase near10 = tileMap.GetTile(new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z));
        TileBase near_10 = tileMap.GetTile(new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z));

        //如果上方有障碍，无法站立，不是平台。
        for (int i = 0; i < height; i++)
        {
            if (nearTops[i])
            {
                return null;
            }
        }
        //如右无瓦片
        if (!near10)
        {
            edgePoint[1] = 1;
            edgePoint[3] = 1;

        }
        //右上
        for (int i = 0; i < height; i++)
        {
            if (nearTopRights[i])
            {
                edgePoint[1] = 1;
                edgePoint[3] = 0;
                break;
            }
        }
        //如左无瓦片
        if (!near_10)
        {
            edgePoint[0] = 1;
            edgePoint[2] = -1;
        }
        //左上
        for (int i = 0; i < height; i++)
        {
            if (nearTopLefts[i])
            {
                edgePoint[0] = 1;
                edgePoint[2] = 0;
                break;
            }
        }

        return edgePoint;
    }

    public void GetAllNearPlat()
    {
        Vector3Int sv = grid.WorldToCell(transform.position);
        r = 10;

        //BoundsInt bounds = new BoundsInt(sv.x - r, sv.y - r, 0, r*2, r*2, 1);
        //TileBase[] tiles = tileMap.GetTilesBlock(bounds);

        rightTop = new Vector2Int(sv.x + r, sv.y + r);
        //Vector3Int rightEdgePoint = new Vector3Int(-65535, -65535, -65535);

        allNearPlat = new List<Plat>();



        Dictionary<int, Dictionary<Vector3Int, int[]>> edges = new Dictionary<int, Dictionary<Vector3Int, int[]>>();
        Dictionary<Vector3Int, int[]> edge_dic = new Dictionary<Vector3Int, int[]>();
        for (int i = 0; i < r * r * 4; i++)
        {

            Vector3Int tilePos = new Vector3Int(rightTop.x - (i % (2 * r)), rightTop.y + 1 - (i / (2 * r)), 0);

            //for debug
            if (tilePos.x == -10 && tilePos.y == 2)
            {
                int e = 1;
            }


            int[] edge = SetEdgePoint(tilePos, 1.8f);
            if (edge != null && (edge[0] != 0 || edge[1] != 0))
            {
                edge_dic[tilePos] = edge;
            }
            if (tilePos.x == rightTop.x - 2 * r + 1 && edge_dic.Count != 0)
            {
                edges[tilePos.y] = edge_dic;
                edge_dic = new Dictionary<Vector3Int, int[]>();
            }
        }
        foreach (var item in edges)
        {
            List<Vector3Int> v3 = new List<Vector3Int>(item.Value.Keys);

            if (item.Value[v3[0]][0] == 1 && item.Value[v3[0]][1] == 0)
            {
                //向该瓦片右侧寻找右边缘点
                for (int ii = 1; ii < 100; ii++)
                {
                    Vector3Int tilePos2 = new Vector3Int(v3[0].x + ii, v3[0].y, v3[0].z);
                    int[] edge2 = SetEdgePoint(tilePos2, 1.8f);
                    if ((edge2 != null && (edge2[0] != 0 || edge2[1] != 0)) || ii == 99)
                    {
                        allNearPlat.Add(new Plat(v3[0], tilePos2, item.Value[v3[0]], edge2));
                        v3.Remove(v3[0]);
                        break;
                    }
                }
            }

            if (v3.Count != 0)
                if (item.Value[v3[v3.Count - 1]][0] == 0 && item.Value[v3[v3.Count - 1]][1] == 1)
                {
                    //向该瓦片左侧寻找左边缘点
                    for (int ii = 1; ii < 100; ii++)
                    {
                        Vector3Int tilePos2 = new Vector3Int(v3[v3.Count - 1].x - ii, v3[v3.Count - 1].y, v3[v3.Count - 1].z);
                        int[] edge2 = SetEdgePoint(tilePos2, 1.8f);
                        if ((edge2 != null && (edge2[0] != 0 || edge2[1] != 0)) || ii == 99)
                        {
                            allNearPlat.Add(new Plat(tilePos2, v3[v3.Count - 1], edge2, item.Value[v3[v3.Count - 1]]));
                            v3.Remove(v3[v3.Count - 1]);
                            break;
                        }
                    }
                }


            for (int i = 0; i < v3.Count; i++)
            {
                if (item.Value[v3[i]][0] == 1 && item.Value[v3[i]][1] == 1)
                {
                    allNearPlat.Add(new Plat(v3[i], v3[i], item.Value[v3[i]]));
                    continue;
                }
                else
                {
                    allNearPlat.Add(new Plat(v3[i + 1], v3[i], item.Value[v3[i + 1]], item.Value[v3[i]]));
                    i++;
                    continue;
                }

            }
        }


    }

    private Vector2Int FindPlatPoint()
    {
        return new Vector2Int();
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("tag");
        grid = g.GetComponent<Grid>();
        tileMap = g.GetComponentInChildren<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allNearPlat != null)
            foreach (Plat p in allNearPlat)
            {
                Debug.DrawLine(new Vector3(p.left.x, p.left.y, 0), new Vector3(p.right.x, p.right.y, 0), Color.gray);
            }
        Debug.DrawLine(new Vector3(rightTop.x, rightTop.y, 0), new Vector3(rightTop.x - 2 * r, rightTop.y - 2 * r, 0), Color.red);
    }
}
