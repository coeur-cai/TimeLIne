using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using LitJson;
using System.Runtime;
using System;



public class Test : MonoBehaviour {
    //记录帧
    private int curIndex = 1;
    private bool isInsert = false;
    private string insertLogFilePath;
    private string binaryLogDataSavePath;

    private string gzipCompressFileSavePath;
    private string gzipUnCompressFileSavePath;
    private string bzipCompressFileSavePath;
    private string bzipUnCompressFileSavePath;
    private string txtLogDataSavePath;
    private string LogPadPath;  
    private void InitData()
    {
        insertLogFilePath = Application.dataPath + "/code/";
        binaryLogDataSavePath = Application.streamingAssetsPath +"/Log.bin";
        txtLogDataSavePath = Application.persistentDataPath + "/LogData.txt";
        LogPadPath = Application.streamingAssetsPath + "/LogPdb3.json";
        gzipCompressFileSavePath = Application.streamingAssetsPath + "/gizp.gz";
        bzipCompressFileSavePath = Application.streamingAssetsPath + "/bzip.bz";
        bzipUnCompressFileSavePath = Application.streamingAssetsPath + "/bzip.bin";
        gzipUnCompressFileSavePath = Application.streamingAssetsPath + "/gzip.bin";
    }
    private void TestReadWrite()
    {       
       // isInsert = LogHandler.InsertLogtrackCode(insertLogFilePath, "My.cs");
        My my = new My();
        for (int i = 0; i < 100; i++)
        {
            my.MyStudy("math","computer");
            my.MySex(0);
            my.MyHeight("172");
            my.MyWeight("63");
            my.Myfavorite(110, 0, 26);
        }            
        StartCoroutine(StartWrite());
    }
    // Use this for initialization
    void Start() {
        // TestNumber();
        // InitData();
        // TestReadWrite();
        //TestAry();
        // byte[] by = new byte[] { 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 , 0x01, 0x02, 0x03, 0x04 };                                                  
        // 源文件225KB  Gzip压缩 1KB(514字节)   Gzip解压后225KB
        // 源文件225KB  Bzip压缩1.03KB          Bzip解压后225KB （网上说Bzip压缩量更大）
        //Bzip>Gzip>deflate>lzo>lz4>snappy
        //TestSplit();
        // CodeTransform();
        //string sb=  Test2();
        // Test3(sb);
        
    }
    IEnumerator StartWrite()
    {
        yield return new WaitForSeconds(2);
        LogDataReadWrite ldh = new LogDataReadWrite();             
        ldh.LogDataSerlializer(Application.streamingAssetsPath + "/Log.bin", FSPDebuger.LogTrackLoopQueue);
        Debug.Log("Ok");      
        yield return new WaitForEndOfFrame();
                 
    }
    private void Update()
    {                  
    }

    private void TestByOneFps()
    {
        My my = new My();
        for (int i = 0; i < 400; i++)
        {
            my.MyStudy("math", "computer");
            my.MySex(0);
            my.MyHeight("172");
            my.MyWeight("63");
            my.Myfavorite(110, 0, 26);
        }       
    }
    

    //1.Gzip压缩字节数组
    private bool GzipCompressByte(byte[] by)
    {
        return Compress.GZipCompressByte(by, gzipCompressFileSavePath);       
    }  

    //1.Gzip解压字节数组
    private bool GzipUnCompressByte()
    {
        return Compress.GzipUnCompressByte(gzipCompressFileSavePath,gzipUnCompressFileSavePath);
    }
  

    //Gzip压缩文件
    private bool GzipCompress()
    {        
        return Compress.GzipCompressFile(binaryLogDataSavePath, gzipCompressFileSavePath);
    }

    //Gzip反解压文件
    private bool GzipUnCompress()
    {       
        return Compress.GzipUnCompressToFile(gzipCompressFileSavePath, gzipUnCompressFileSavePath);
    }

    //Bzip压缩文件
    private bool BzipCompress()
    {
        return Compress.BzipCompressFile(binaryLogDataSavePath, bzipCompressFileSavePath);
    }

    //Bzip解压文件
    private bool BzipUnCompress()
    {
        return Compress.BzipUnCompressToFile(bzipCompressFileSavePath, bzipUnCompressFileSavePath);
    }

    private byte[] ToByteAry(int[] data)
    {
        byte[] byteAry = new byte[data.Length * 4];
        for (int i = 0; i < data.Length; i++)
        {
            byteAry[i * 4 + 0] = (byte)((data[i] >> 24) & 0xff);
            byteAry[i * 4 + 1] = (byte)((data[i] >> 16) & 0xff);
            byteAry[i * 4 + 2] = (byte)((data[i] >> 8) & 0xff);
            byteAry[i * 4 + 3] = (byte)(data[i] & 0xff);
        }
        return byteAry;
    }


    public List<LogTrackFunc> TestNumber()
    {
        /*
        int x = 0;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(x++);
            Debug.Log((ushort)(x++));
        }
        */
        string str = "0A-0A-08-0A-12-06-63-61-69-6D-69-6E-0A-09-08-03-12-05-70-61-74-68-31-0A-0E-08-01-12-04-61-72-67-31-12-04-61-72-67-32-0A-09-08-03-12-05-70-61-74-68-31-0A-02-08-02-0A-09-08-03-12-05-70-61-74-68-31";
        String[] tempArr = str.Split('-');
        byte[] decBytes = new byte[tempArr.Length];
        for (int i = 0; i < tempArr.Length; i++)
        {
            decBytes[i] = Convert.ToByte(tempArr[i], 16);

        }
        List<LogTrackFunc> list = new List<LogTrackFunc>();
        using (MemoryStream ms=new MemoryStream())
        {
            ms.Write(decBytes, 0, decBytes.Length);
            ms.Position = 0;
           list= ProtoBuf.Serializer.Deserialize<List<LogTrackFunc>>(ms);
        }
        return list;
    }
    public static ArgsAry args = new ArgsAry();
    void TestAry()
    {
        args.AddItems(new string[] { "caimin", "lihuan", "panzi", "sanmao", "lining", "weitian", "shengjunyang","yulinfeng","dazuo","pige","jingege","pengpeng","jiakun","muming","xiaoing" });
        string[] arg = args.GetItems(6);

        string[] arg1 = args.GetItems(3);


    }
    void TestSplit()
    {
        string str = "1$2$3$4$5$6$7$&caimin$liHuan$panzi$sanmao$";
        string[] idandarg = str.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        string[] idAry = idandarg[0].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
        string[] argAry = idandarg[1].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
    }
    byte[] CodeTransform()
    {
        string code = "\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0002\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008\u0003\u0008$";
        //分割id字符串和args字符串的序列化数据
        string[] idAndArgs = code.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
        ushort[] idAry;
        byte[] idByte = System.Text.Encoding.UTF8.GetBytes(code);
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(idByte, 0, idByte.Length);
            ms.Position = 0;
            idAry = ProtoBuf.Serializer.Deserialize<ushort[]>(ms);
        }
        return idByte;
    }
    string Test2()
    {
        ushort[] idary = new ushort[10];
        string[] argary = new string[10];
        for (int i = 0; i < 5; i++)
        {
            idary[i] = 1;
        }
        for (int i = 0; i < 10; i++)
        {
            argary[i] = "##";
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        using (MemoryStream ms = new MemoryStream())
        {
           
           ProtoBuf.Serializer.Serialize<ushort[]>(ms, idary);           
           ms.Position = 0;
           StreamReader r1 = new StreamReader(ms);
           sb.Append(r1.ReadToEnd());
        }
        sb.Append("$");
        using (MemoryStream ms = new MemoryStream())
        {

            ProtoBuf.Serializer.Serialize<string[]>(ms, argary);
            ms.Position = 0;
            StreamReader r2 = new StreamReader(ms);
            sb.Append(r2.ReadToEnd());
        }
        return sb.ToString();

    }
    void Test3(string str)
    {
        if (str!=null)
        {
            //分割id字符串和args字符串的序列化数据
            string[] idAndArgs = str.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
            string idAry = idAndArgs[0];
            string argAry = idAndArgs[1];

            String[] idArr = idAry.Split('-');
            byte[] decBytes = new byte[idArr.Length];
            for (int i = 0; i < idArr.Length; i++)
            {
                decBytes[i] = Convert.ToByte(idArr[i], 16);
            }
            ushort[] id;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(decBytes, 0, decBytes.Length);
                ms.Position = 0;
                id = ProtoBuf.Serializer.Deserialize<ushort[]>(ms);
            }

            String[] argArr = argAry.Split('-');
            byte[] argBytes = new byte[argArr.Length];
            for (int i = 0; i < argArr.Length; i++)
            {
                argBytes[i] = Convert.ToByte(argArr[i], 16);
            }
            string[] arg;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(argBytes, 0, argBytes.Length);
                ms.Position = 0;
                arg = ProtoBuf.Serializer.Deserialize<string []>(ms);
            }
        }
       
    }
    
    void Test4()
    {
        ushort[] id=new ushort[600];
        for (int i = 0; i < 600; i++)
        {
            id[i] = (ushort)(i + 1);
        }
        UnityEngine.Profiling.Profiler.BeginSample("------ ToString");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < id.Length; i++)
        {
            sb.Append(id[i]+"#");
        }
        UnityEngine.Profiling.Profiler.EndSample();        
    }
    void Test5(ushort[] _array)
    {
        UnityEngine.Profiling.Profiler.BeginSample("------ Test5");
        using (MemoryStream s = new MemoryStream())
        {
            UnityEngine.Profiling.Profiler.BeginSample("------ Serlizer");
            ProtoBuf.Serializer.Serialize<ushort[]>(s, _array);
            UnityEngine.Profiling.Profiler.EndSample();
            s.Position = 0;
            UnityEngine.Profiling.Profiler.BeginSample("------ Read");
            StreamReader r = new StreamReader(s);            
            string _temp = r.ReadToEnd();
            UnityEngine.Profiling.Profiler.EndSample();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    public void Onclick()
    {
        //ushort[] _array = { 2, 2, 2, 2, 22, 5, 100, 20, 50, 60, 70, 80 };
        Test4();
        //Test5(_array);
    }
}
