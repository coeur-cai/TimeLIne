using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static  class BinaryLogDataInverse
{
    private static string binaryLogDataFilePath = "";
    private static string inverseFileOutPutFolderPath ="";
    private static string logPdbFilepath = Application.streamingAssetsPath+ "/newLogPdb.json";
    private static string filename = "/LogData.txt";

    [MenuItem("Tools/程序工具/高性能数字化日志/数据反解")]
    public static void SelectbinaryLogDataFile()
    {
        string filePath = EditorUtility.OpenFilePanel("选择文件", Application.streamingAssetsPath, "");
        if (File.Exists(filePath))
        {
            binaryLogDataFilePath = filePath;
            filename= Path.GetFileNameWithoutExtension(binaryLogDataFilePath);
            inverseFileOutPutFolderPath= Application.streamingAssetsPath + "/"+filename+".txt";                    
            LogDataReadWrite ldrw = new LogDataReadWrite();
            ldrw.ReLogDataSerlializerAndUncompress(binaryLogDataFilePath, logPdbFilepath, inverseFileOutPutFolderPath);          
            Debug.Log("sucess!");
            AssetDatabase.Refresh();
        }
    }
    /*
    [MenuItem("Tools/程序工具/高性能数字化日志/压缩")]
    public static void CompressFile()
    {
        string filePath = EditorUtility.OpenFilePanel("选择文件", Application.persistentDataPath, "");
        if (File.Exists(filePath))
        {
            binaryLogDataFilePath = filePath;
            filename = Path.GetFileNameWithoutExtension(binaryLogDataFilePath);
            inverseFileOutPutFolderPath = Application.streamingAssetsPath + "/" + filename + ".gz";
            bool tag= Compress.GzipCompressFile(binaryLogDataFilePath, inverseFileOutPutFolderPath); // 源文件18KB  Gzip526字节 解压后18Kb
           // bool tag = Compress.BzipCompressFile(binaryLogDataFilePath, inverseFileOutPutFolderPath);  //源文件18KB  Bzip712字节 
            if (tag)
            {
                Debug.Log("压缩成功！");
            }
            AssetDatabase.Refresh();
        }            
    }
   
    [MenuItem("Tools/程序工具/高性能数字化日志/解压")]
    public static void UnCompressFile()
    {
        string filePath = EditorUtility.OpenFilePanel("选择文件", Application.persistentDataPath, "");
        if (File.Exists(filePath))
        {           
            filename = Path.GetFileNameWithoutExtension(filePath);
            inverseFileOutPutFolderPath = Application.persistentDataPath + "/" + filename + ".gz";
            Debug.Log(inverseFileOutPutFolderPath);
            string inputPath = Application.streamingAssetsPath + "/" + filename + ".bin";
            Debug.Log(inputPath);
            bool tag = Compress.GzipUnCompressToFile(inverseFileOutPutFolderPath, inputPath);          
            if (tag)
            {
                Debug.Log("解压成功！");
            }
            AssetDatabase.Refresh();
        }
    }
    */
    //序列化后无压缩86KB    序列化压缩后9.11KB   解析后文件为  压缩比约为89%
    /*
    /// <summary>
    /// 反解数据(frameIndex和Id)
    /// </summary>
    public static void LogDataInverseId()
    {      
        if (!File.Exists(binaryLogDataFilePath))
        {
            Debug.LogError("Binary Data File Not Loaded");
            return;
        }
        //读取的日志数据
        string strMsg;
        //用于存储帧索引-函数ID序列
        Dictionary<int, List<int>> frameIdDic = new Dictionary<int, List<int>>();
        List<int> binData = new List<int>();
        //读取二进制日志数据
        using (FileStream fs = new FileStream(binaryLogDataFilePath, FileMode.Open, FileAccess.Read))
        {
            BinaryReader br = new BinaryReader(fs);
            while (br.PeekChar() > -1)
            {
                binData.Add(br.ReadInt32());
            }
            br.Close();
        }
        int curIndex = 0;
        int binDataCnt = binData.Count;
        //对读取的集合进行分割处理
        while (curIndex < binDataCnt)
        {
            List<int> idAndFrameAry = new List<int>();
            for (; binData[curIndex] != int.MaxValue; curIndex++)
            {
                idAndFrameAry.Add(binData[curIndex]);
            }
            //跳过最大值
            curIndex++;
            //将数据保存至字典中
            int frameIndex = idAndFrameAry[0];
            idAndFrameAry.RemoveAt(0);
            frameIdDic.Add(frameIndex, idAndFrameAry);
        }
        strMsg = JsonHandler.OutputAllMes(frameIdDic, logPdbFilepath);
        if (strMsg != null)
        {
            //写数据
            using (FileStream fs = new FileStream(inverseFileOutPutFolderPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by =System.Text.Encoding.UTF8.GetBytes(strMsg);
                fs.Write(by, 0, by.Length);
            }
        }
        Debug.Log("反解完毕");
        //刷新
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 反解数据（id和帧数、参数）
    /// </summary>
    /// <param name="inputLogDataPath"></param>
    /// <param name="outputLogDataPath"></param>
    /// <param name="logPdbFilepath"></param>
    public static void LogDataInverseIDAndFrameIndexWithArgs()
    {
        if (!File.Exists(binaryLogDataFilePath))
        {
            return;
        }
        //读取的日志数据
        string strMsg;
        //用于存储帧索引-函数ID序列
        Dictionary<int, List<int>> frameIdDic = new Dictionary<int, List<int>>();
        //存储读取的所有数据
        List<int> binData = new List<int>();
        //存储所有参数
        List<int> argsList = new List<int>();
        //读取二进制日志数据
        using (FileStream fs = new FileStream(binaryLogDataFilePath, FileMode.Open, FileAccess.Read))
        {
            BinaryReader br = new BinaryReader(fs);
            while (br.PeekChar() > -1)
            {
                binData.Add(br.ReadInt32());
            }
            br.Close();
        }
        //记录遍历的位置
        int curIndex = 0;
        int binDataCnt = binData.Count;
        //对读取的集合进行分割处理
        while (curIndex < binDataCnt)
        {
            List<int> idAndFrameAryWithArgs = new List<int>();
            for (; binData[curIndex] != int.MaxValue; curIndex++)
            {
                idAndFrameAryWithArgs.Add(binData[curIndex]);
            }
            //跳过最大值（用最大值来分隔每帧的函数调用序列）
            curIndex++;
            //列表中第一个数是帧索引，后面则是函数ID序列和参数序列
            int frameIndex = idAndFrameAryWithArgs[0];
            //移除帧索引
            idAndFrameAryWithArgs.RemoveAt(0);
            //分隔Id序列和Args序列(最小值分隔)
            //用于记录遍历的位置
            int i = idAndFrameAryWithArgs.Count - 1;
            //存储每帧的参数序列
            List<int> tempArgsList = new List<int>();
            while (idAndFrameAryWithArgs[i] != int.MinValue)
            {
                tempArgsList.Add(idAndFrameAryWithArgs[i]);
                i--;
            }
            //移除Args序列
            idAndFrameAryWithArgs.RemoveRange(i, idAndFrameAryWithArgs.Count - i);
            //需要反转一下
            tempArgsList.Reverse();
            //存储至Args集合中
            argsList.AddRange(tempArgsList);
            frameIdDic.Add(frameIndex, idAndFrameAryWithArgs);
        }
        Debug.Log(argsList.Count);        
        strMsg = JsonHandler.OutputAllMes(frameIdDic, argsList, logPdbFilepath);
        if (strMsg != null)
        {
            //写数据
            using (FileStream fs = new FileStream(inverseFileOutPutFolderPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = System.Text.Encoding.UTF8.GetBytes(strMsg);
                fs.Write(by, 0, by.Length);
            }
        }
        Debug.Log("反解完毕");
        //刷新
        AssetDatabase.Refresh();
    }
    */
}

