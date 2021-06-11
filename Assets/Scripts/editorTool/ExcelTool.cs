using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class ExcelTool
{
    /// <summary>
    /// 读取表数据，生成对应的数组
    /// </summary>
    /// <param name="filePath">excel文件全路径</param>
    /// <returns>ItemDescribe数组</returns>
    public static ItemDescribe[] CreateItemDescribeArrayWithExcel(string filePath)
    {
        //获得表数据
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

        //根据excel的定义，第二行开始才是数据
        ItemDescribe[] array = new ItemDescribe[rowNum - 1];
        for (int i = 1; i < rowNum; i++)
        {
            ItemDescribe ItemDescribe = new ItemDescribe();
            //解析每列的数据
            ItemDescribe.ItemName = collect[i][0].ToString();
            ItemDescribe.Describe = collect[i][1].ToString();
            array[i - 1] = ItemDescribe;
        }
        return array;
    }

    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }

}
