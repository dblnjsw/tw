using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExcelBuild : Editor
{
    [MenuItem("CustomEditor/CreateAssetFromExcel")]
    public static void CreateitemAsset()
    {
        DataManager manager = ScriptableObject.CreateInstance<DataManager>();
        //赋值
        manager.dataArray = ExcelTool.CreateItemDescribeArrayWithExcel(ExcelConfig.excelsFolderPath + "item.xlsx");

        //确保文件夹存在
        if (!Directory.Exists(ExcelConfig.assetPath))
        {
            Directory.CreateDirectory(ExcelConfig.assetPath);
        }

        //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
        string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "data");
        //生成一个Asset文件
        AssetDatabase.CreateAsset(manager, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
