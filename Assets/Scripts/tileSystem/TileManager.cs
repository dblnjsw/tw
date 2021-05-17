using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public List<Tile> allTiles;
    public Dictionary<string, Tile> dic_tiles;
    public Dictionary<string,GameObject> dic_reward;

    public Vector2Int leftTop;
    public Vector2Int rightBottom;
    public List<Vector3Int> masterBirthPoint;

    Dictionary<string, string> dic_tile_reward;

    Grid grid;

    Tilemap tilemap;
    CompositeCollider2D compositeCollider;

    // Start is called before the first frame update
    void Start()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
        grid = GetComponentInParent <Grid> ();
        dic_tiles = new Dictionary<string, Tile>();
        dic_tile_reward = new Dictionary<string, string>();
        foreach(Tile tile in allTiles)
        {
            dic_tile_reward[tile.name] = tile.name;
            dic_tiles[tile.name] = tile;
        }
        //更改瓦片-掉落映射表
        dic_tile_reward["glass"] = "ground";

        GameObject g_data = GameObject.FindGameObjectWithTag("DataPool");
        DataPool dataPool = g_data.GetComponent<DataPool>();
        dic_reward = dataPool.dic_rewards;

        //Vector3Int[] positions = new Vector3Int[64];
        //TileBase[] tileArray = new TileBase[positions.Length];

        //for (int index = 0; index < positions.Length; index++)
        //{
        //    positions[index] = new Vector3Int(index % 8, index / 8,0);
        //    tileArray[index] = dic_tiles["ground"];
        //}
        
        tilemap = GetComponent<Tilemap>();

        //生成地形
        genTile(dic_tiles["ground"], leftTop, rightBottom);

        //根据高度生成不同的矿石种类
        Vector2Int[] minePoints = randomVector2(50, new Vector2Int(leftTop.x, leftTop.y-10), rightBottom);     
        foreach(Vector2Int p in minePoints)
        {
            int high = (int)Random.Range(2, 4);
            int wide = (int)Random.Range(2, 4);
            Vector2Int bottom = new Vector2Int(p.x + wide, p.y - high);
            if (p.y > -30)
            {
                genTile(dic_tiles["golden"], p, bottom);
            }else if(p.y > -40){
                genTile(dic_tiles["redStone"], p, bottom);
            }
            else
            {
                genTile(dic_tiles["diamond"], p, bottom);
            }
        }

        //生成洞穴
        genCave(new Vector2Int(leftTop.x, leftTop.y - 5), rightBottom, 2000f);

        //生成山脉
        genMount(leftTop, rightBottom, 2000f);

        //生成基岩
        genTile(dic_tiles["bedrock"],new Vector2Int(leftTop.x,rightBottom.y),new Vector2Int(rightBottom.x,rightBottom.y-10));

        //flashTileMap();
    }

    Vector2Int[] randomVector2(int num, Vector2Int top, Vector2Int bottom)
    {
        Vector2Int[] result = new Vector2Int[num];

        for(int i = 0; i < num; i++)
        {
            result[i] = new Vector2Int((int)Random.Range(top.x,bottom.x), (int)Random.Range(top.y, bottom.y));
        }
        return result;
    }

    //top为左上坐标点，bottom为右下坐标点，tile为该区域要填充的瓦片
    void genTile(TileBase tile,Vector2Int top,Vector2Int bottom)
    {
        int length_x = bottom.x - top.x;
        int length_y = top.y - bottom.y;

        Vector3Int[] positions = new Vector3Int[length_x*length_y];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int(top.x+(index % length_x), top.y-(index / length_x), 0);
            tileArray[index] = tile;
        }
        tilemap.SetTiles(positions, tileArray);

    }

    void genCave(Vector2Int top, Vector2Int bottom, float sed)
    {
        int length_x = bottom.x - top.x;
        int length_y = top.y - bottom.y;
        masterBirthPoint = new List<Vector3Int>();
        
        for (int index = 0; index < length_x * length_y; index++)
        {
            Vector3Int v = new Vector3Int(top.x + (index % length_x), top.y - (index / length_x), 0);
            float f = Mathf.PerlinNoise((v.x+1000+sed)/10f, (v.y+1000+sed)/10f);
            if (f>0.55f)
            {
                tilemap.SetTile(v,null);
            }
            if (f > 0.8f)
            {
                if(Random.Range(0,100)>60)
                    masterBirthPoint.Add(v);
            }
        }
    }

    void genMount(Vector2Int top, Vector2Int bottom, float sed)
    {
        int length_x = bottom.x - top.x;
        for (int index = 0; index < length_x; index++)
        {
            int height = (int)(Mathf.PerlinNoise(1000 + (float)index / 10 + sed, 0) / 2 *10);

            for (int i = 0;i < height; i++)
            {
                if(i == height-1)
                {
                    tilemap.SetTile(new Vector3Int(top.x + index, top.y + 1 + i, 0), dic_tiles["glass"]);
                    continue;
                }
                tilemap.SetTile(new Vector3Int(top.x+index,top.y+1+i,0), dic_tiles["ground"]);
            }
        }
    }

    public void destroyTile(Vector3Int pos)
    {
        TileBase t = tilemap.GetTile(pos);
        if (t)
            if(t.name!="bedrock")
            {
                dropReward(dic_tile_reward[t.name], pos);
                tilemap.SetTile(pos, null);
                Debug.Log(MyFunc.toString(pos) + " tile destory");
            }
        
    }

    public bool createTile(Vector3Int pos, string tileName)
    {
        if (tilemap.HasTile(pos))
        {
            return false;
        }
        else
        {
            tilemap.SetTile(pos, dic_tiles[tileName]);
            return true;
        }
    }

    void dropReward(string name,Vector3Int pos)
    {
        Vector3 v = grid.CellToWorld(pos);
        GameObject.Instantiate(dic_reward["re_" + name + "_"], new Vector3(v.x+.5f+Random.Range(-0.3f,0.3f),v.y+.5f + Random.Range(-0.3f, 0.3f), v.z),Quaternion.identity);
        
    }

    void flashTileMap()
    {
        //yield return new WaitForEndOfFrame();
        compositeCollider.GenerateGeometry();

        Debug.Log("tile flash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
