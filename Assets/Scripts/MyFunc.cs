using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunc 
{
    public static string toString(object val)
    {
        if(val is Vector3)
        {
            Vector3 v = (Vector3)val;
            string s = "(";
            s += v.x.ToString() + ",";
            s += v.y.ToString() + ",";
            s += v.z.ToString() + ")";

            return s;
        }
        else if(val is Vector3Int)
        {
            Vector3Int v = (Vector3Int)val;
            string s = "(";
            s += v.x.ToString() + ",";
            s += v.y.ToString() + ",";
            s += v.z.ToString() + ")";

            return s;
        }

        return "toString wrong!";
    }
}
