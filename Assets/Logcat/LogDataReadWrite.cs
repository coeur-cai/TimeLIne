using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
//引入序列化命名空间
using ProtoBuf.Serializers;
using ICSharpCode.SharpZipLib.GZip;


public class LogDataReadWrite
{
    /*
     /// <summary>
     /// 在线写入可读性日志(id和Args)
     /// </summary>
     /// <param name="path">写入文件的路径</param>
     /// <param name="cq">内存中存储的近60帧的循环队列</param>
     public void WriteLogDataToLocalIdWithArgs(string path, CircleQueue<LogTrackFrame> cq)
     {
         if (cq != null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 List<int> idList = new List<int>();
                 List<int> argList = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         idList.AddRange(ltf.Items);
                         argList.AddRange(ltf.Args);
                     }
                 }
                 string strMsg = JsonHandler.OutputAllMes(idList, argList, path);
                 using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     byte[] by = System.Text.Encoding.UTF8.GetBytes(strMsg);
                     fs.Write(by, 0, by.Length);
                 }
             }
         }
     }

     /// <summary>
     /// 在线写入可读性日志(id)
     /// </summary>
     /// <param name="path">写入文件的路径</param>
     /// <param name="cq">内存中存储的近60帧的循环队列</param>
     public void WriteLogDataToLocalId(string logPdbPath, string logSavePath, CircleQueue<LogTrackFrame> cq)
     {
         if (cq != null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 List<int> idList = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         idList.AddRange(ltf.Items);
                     }
                 }
                 string strMsg = JsonHandler.OutputAllMes(idList, logPdbPath);
                 using (FileStream fs = new FileStream(logSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     byte[] by = System.Text.Encoding.UTF8.GetBytes(strMsg);
                     fs.Write(by, 0, by.Length);
                 }
             }
         }
     }

     /// <summary>
     /// 在线写入可读性日志(id和帧数)
     /// </summary>
     /// <param name="logPdbPath"></param>
     /// <param name="logSavePath"></param>
     /// <param name="cq"></param>
     public void WriteLogDataToLocalIdAndFrame(string logPdbPath, string logSavePath, CircleQueue<LogTrackFrame> cq)
     {
         if (cq != null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 List<int> idList = new List<int>();
                 List<int> frameList = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         idList.AddRange(ltf.Items);
                         frameList.Add(ltf.FrameIndex);
                     }
                 }
                 string strMsg = JsonHandler.OutputAllMes(idList, frameList, logPdbPath);
                 using (FileStream fs = new FileStream(logSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     byte[] by = System.Text.Encoding.UTF8.GetBytes(strMsg);
                     fs.Write(by, 0, by.Length);
                 }
             }
         }
     }


     /// <summary>
     /// 在线写入日志数据（id和Args）
     /// </summary>
     /// <param name="path">文件写入的路径</param>
     /// <param name="cq">保存近60帧数据的循环队列</param>    
     public void WriteLogDataIdWithArgs(string path,CircleQueue<LogTrackFrame> cq) 
     {
         if (cq!=null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 List<int> idList = new List<int>();
                 List<int> argList = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         idList.AddRange(ltf.Items);
                         argList.AddRange(ltf.Args);
                     }
                 }
                 using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     BinaryWriter sw = new BinaryWriter(fs);
                     for (int i = 0; i < idList.Count; i++)
                     {
                         sw.Write(idList[i]);
                     }
                     //分隔符
                     sw.Write(int.MaxValue);
                     for (int i = 0; i < argList.Count; i++)
                     {
                         sw.Write(argList[i]);
                     }
                     sw.Close();
                 }
             }
         }         
     }

     /// <summary>
     /// 在线写入日志数据（id）
     /// </summary>
     /// <param name="path">文件写入的路径</param>
     /// <param name="cq">保存近60帧数据的循环队列</param>
     public void WriteLogDataId(string path, CircleQueue<LogTrackFrame> cq)
     {
         if (cq != null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 List<int> idList = new List<int>();              
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         idList.AddRange(ltf.Items);                   
                     }
                 }
                 using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     BinaryWriter sw = new BinaryWriter(fs);
                     for (int i = 0; i < idList.Count; i++)
                     {
                         sw.Write(idList[i]);
                     }                   
                     sw.Close();
                 }
             }
         }
     }    

     /// <summary>
     /// 在线写入日志数据（id和帧数）
     /// </summary>
     /// <param name="path">文件写入的路径</param>
     /// <param name="cq">保存近60帧数据的循环队列</param>
     public void WriteLogIDAndFrameIndex(string path, CircleQueue<LogTrackFrame> cq)
     {
         if (cq != null)
         {
             int cqLength = cq.QueueLength();
             if (cqLength != 0)
             {
                 cq.ResetCur();
                 List<int> idAndFrameList = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         //按照FrameIndex-Id序列-最大值格式存储
                         idAndFrameList.Add(ltf.FrameIndex);
                         idAndFrameList.AddRange(ltf.Items);
                         idAndFrameList.Add(int.MaxValue);
                     }
                 }               
                 using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                 {
                     BinaryWriter sw = new BinaryWriter(fs);
                     for (int i = 0; i < idAndFrameList.Count; i++)
                     {
                         sw.Write(idAndFrameList[i]);
                     }
                     sw.Close();
                 }
             }
         }
     }

     /// <summary>
     /// 在线写入日志数据（id和帧数、参数）
     /// </summary>
     /// <param name="path"></param>
     /// <param name="cq"></param>
     public void WriteLogIDAndFrameIndexAndArgs(string path, CircleQueue<LogTrackFrame> cq)
     {
         if (cq!=null)
         {
             int cqLength = cq.QueueLength();
             cq.ResetCur();
             if (cqLength!=0)
             {
                 List<int> idAndFrameWithArgs = new List<int>();
                 for (int i = 0; i < cqLength; i++)
                 {
                     LogTrackFrame ltf = cq.GetNext();
                     if (ltf != null)
                     {
                         //按照(帧-ID序列-最小值-Args序列-最大值)格式存储
                         idAndFrameWithArgs.Add(ltf.FrameIndex);
                         idAndFrameWithArgs.AddRange(ltf.Items);
                         idAndFrameWithArgs.Add(int.MinValue);
                         idAndFrameWithArgs.AddRange(ltf.Args);
                         Debug.Log("---------------------");
                         Debug.Log(ltf.Args.Count);
                         idAndFrameWithArgs.Add(int.MaxValue);
                     }
                 }
                 if (idAndFrameWithArgs.Count!=0)
                 {
                     using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                     {
                         BinaryWriter sw = new BinaryWriter(fs);
                         for (int i = 0; i < idAndFrameWithArgs.Count; i++)
                         {
                             sw.Write(idAndFrameWithArgs[i]);
                         }
                         sw.Close();
                     }
                 }               
             }
         }
     }


     /// <summary>
     /// 日志数据反解（id和Args）
     /// </summary>
     /// <param name="inputLogDataPath">二进制日志数据输入路径</param>
     /// <param name="outputLogDataPath">可读性的日志数据路径输出路径</param>
     public void LogDataInverseIdWithArgs(string inputLogDataPath, string outputLogDataPath,string LogPdbPath)
     {

         if (!File.Exists(inputLogDataPath))
         {
             return;
         }
         //读取的日志数据
         string strMsg;
         List<int> idAry = new List<int>();       
         List<int> argAry = new List<int>();
         //读取二进制日志数据
         using (FileStream fs = new FileStream(inputLogDataPath, FileMode.Open, FileAccess.Read))
         {
             BinaryReader br = new BinaryReader(fs);
             while (br.PeekChar() > -1)
             {
                 idAry.Add(br.ReadInt32());
             }
             br.Close();
         }
         if (idAry.Count!=0)
         {
             int i = idAry.Count - 1;
             for (; idAry[i]!= int.MaxValue; i--)
             {
                 argAry.Add(idAry[i]);                
             }
             idAry.RemoveRange(i, idAry.Count-i);
             argAry.Reverse();
         }                      
         strMsg= JsonHandler.OutputAllMes(idAry, argAry, LogPdbPath);        
         if (strMsg!=null)
         {
             //写数据
             using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
             {
                 byte[] by= Encoding.UTF8.GetBytes(strMsg);
                 fs.Write(by, 0, by.Length);
             }
         }

     }


     /// <summary>
     /// 日志数据反解（id）
     /// </summary>
     /// <param name="inputLogDataPath">二进制日志数据输入路径</param>
     /// <param name="outputLogDataPath">可读性的日志数据路径输出路径</param>
     public void LogDataInverseId(string inputLogDataPath,string outputLogDataPath,string logPdbFilepath)
     {

         if (!File.Exists(inputLogDataPath))
         {
             return;
         }
         //读取的日志数据
         string strMsg;
         List<int> idAry = new List<int>();        
         //读取二进制日志数据
         using (FileStream fs = new FileStream(inputLogDataPath, FileMode.Open, FileAccess.Read))
         {
             BinaryReader br = new BinaryReader(fs);
             while (br.PeekChar() > -1)
             {
                 idAry.Add(br.ReadInt32());
             }
             br.Close();
         }        
         strMsg = JsonHandler.OutputAllMes(idAry,logPdbFilepath);
         if (strMsg != null)
         {
             //写数据
             using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
             {
                 byte[] by = Encoding.UTF8.GetBytes(strMsg);
                 fs.Write(by, 0, by.Length);
             }
         }

     }

     /// <summary>
     /// 日志数据反解(id和帧数)
     /// </summary>
     /// <param name="inputLogDataPath">二进制数据路径</param>
     /// <param name="outputLogDataPath">输出可读性日志路径</param>
     /// <param name="logPdbFilepath">LogPdb映射表的路径</param>
     public void LogDataInverseIDAndFrameIndex(string inputLogDataPath, string outputLogDataPath, string logPdbFilepath)    
     {
         if (!File.Exists(inputLogDataPath))
         {
             return;
         }
         //读取的日志数据
         string strMsg;
         //用于存储帧索引-函数ID序列
         Dictionary<int, List<int>> frameIdDic = new Dictionary<int, List<int>>();
         //存储读取的所有数据
         List<int> binData = new List<int>();              
         //读取二进制日志数据
         using (FileStream fs = new FileStream(inputLogDataPath, FileMode.Open, FileAccess.Read))
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
         while (curIndex<binDataCnt)
         {                       
             List<int> idAndFrameAry = new List<int>();            
             for (;binData[curIndex] !=int.MaxValue ; curIndex++)
             {
                 idAndFrameAry.Add(binData[curIndex]);                
             }
             //跳过最大值（用最大值来分隔每帧的函数调用序列）
             curIndex++;
             //将数据保存至字典中(列表中第一个数是帧索引，后面则是函数ID序列)
             int frameIndex = idAndFrameAry[0];
             idAndFrameAry.RemoveAt(0);
             frameIdDic.Add(frameIndex, idAndFrameAry);
         }              
         strMsg = JsonHandler.OutputAllMes(frameIdDic, logPdbFilepath);
         if (strMsg != null)
         {
             //写数据
             using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
             {
                 byte[] by = Encoding.UTF8.GetBytes(strMsg);
                 fs.Write(by, 0, by.Length);
             }
         }

     }

     /// <summary>
     /// 日志数据反解（id和帧数、参数）
     /// </summary>
     /// <param name="inputLogDataPath"></param>
     /// <param name="outputLogDataPath"></param>
     /// <param name="logPdbFilepath"></param>
     public void LogDataInverseIDAndFrameIndexWithArgs(string inputLogDataPath, string outputLogDataPath, string logPdbFilepath)
     {
         if (!File.Exists(inputLogDataPath))
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
         using (FileStream fs = new FileStream(inputLogDataPath, FileMode.Open, FileAccess.Read))
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
             int i = idAndFrameAryWithArgs.Count-1;  
             //存储每帧的参数序列
             List<int> tempArgsList = new List<int>();
             while (idAndFrameAryWithArgs[i]!=int.MinValue)
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
         strMsg = JsonHandler.OutputAllMes(frameIdDic, argsList,logPdbFilepath);
         if (strMsg != null)
         {
             //写数据
             using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
             {
                 byte[] by = Encoding.UTF8.GetBytes(strMsg);
                 fs.Write(by, 0, by.Length);
             }
         }
     }
     */
    /*
    /// <summary>
    /// 序列化数据
    /// </summary>
    /// <param name="cq"></param>
    /// <param name="path"></param>
    public void LogDataSerlializer(string path, CircleQueue<LogTrackFrame> cq)
    {
        if (cq!=null)
        {
            int length = cq.QueueLength();
            if (length!=0)
            {
                cq.ResetCur();
                List<LogTrackFrame> ltfList = new List<LogTrackFrame>();
                for (int i = 0; i < length; i++)
                {                
                    ltfList.Add(cq.GetNext());
                }
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    ProtoBuf.Serializer.Serialize<List<LogTrackFrame>>(fs, ltfList);
                }        
            }
        }
    }

    /// <summary>
    /// 序列化后进行Gzip压缩
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cq"></param>
    public void LogDataSerlializerAndCompress(string path, CircleQueue<LogTrackFrame> cq)
    {
        if (cq != null)
        {
            int length = cq.QueueLength();
            if (length != 0)
            {
                cq.ResetCur();
                List<LogTrackFrame> ltfList = new List<LogTrackFrame>();
                for (int i = 0; i < length; i++)
                {
                    ltfList.Add(cq.GetNext());
                }
                using (MemoryStream ms=new MemoryStream())
                {                   
                    using (var gzipStream =new GZipOutputStream(File.Open(path,FileMode.Create)))
                    {
                        //将数据序列化后写至内存流中
                        ProtoBuf.Serializer.Serialize<List<LogTrackFrame>>(ms, ltfList);
                        // 重置内存流的读写位置（此步很关键）
                        ms.Position = 0;
                        //定义缓冲区大小
                        int buffersize = 1024;
                        //创建缓冲数据
                        byte[] FileData = new byte[buffersize];
                        //循环读写
                        while (buffersize > 0)
                        {
                            //先将内存流中的数据读到缓冲区中
                            buffersize = ms.Read(FileData, 0, buffersize);
                            //把缓冲区中数据写入到文件流中
                            gzipStream.Write(FileData, 0, buffersize);//写入压缩文件               
                        }                                                        
                    }
                }
               
            }
        }
    }

    /// <summary>
    /// 解压并进行反序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pdbFilePath"></param>
    /// <param name="outputLogDataPath"></param>
    public void ReLogDataSerlializerAndUncompress(string path, string pdbFilePath, string outputLogDataPath)
    {
        if (!File.Exists(path))
        {
            return;
        }
        List<LogTrackFrame> ltfList = new List<LogTrackFrame>();
        using (MemoryStream ms = new MemoryStream())
        {
            //解压
            using (var stream = new GZipInputStream(File.OpenRead(path)))
            {
                int buffersize = 2048;
                byte[] buffer = new byte[buffersize];
                while (buffersize > 0)
                {
                    buffersize = stream.Read(buffer, 0, buffersize);
                    ms.Write(buffer, 0, buffersize);
                }
            }
            ms.Position = 0;
            //反序列化
            ltfList = ProtoBuf.Serializer.Deserialize<List<LogTrackFrame>>(ms);
        }
        LogDataInverse(ltfList, pdbFilePath, outputLogDataPath);
    }

    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <param name="path">二进制文件路径</param>
    /// <param name="pdbFilePath">Pdb表路径</param>
    /// <param name="outputLogDataPath">输出路径</param>
    public void ReLogDataSerlializer(string path,string pdbFilePath, string outputLogDataPath)
    {        
        if (!File.Exists(path))
        {
            return;
        }              
        List<LogTrackFrame> ltfList = new List<LogTrackFrame>();       
        using (FileStream fs=new FileStream(path,FileMode.Open,FileAccess.Read))
        {
            ltfList=  ProtoBuf.Serializer.Deserialize<List<LogTrackFrame>>(fs);
        }
        Debug.Log("Ok");
        LogDataInverse(ltfList, pdbFilePath, outputLogDataPath);         
    }    

    /// <summary>
    /// 反序列化数据反解
    /// </summary>
    /// <param name="ltfList"></param>
    /// <param name="logpdbFilePath"></param>
    /// <param name="outputLogDataPath"></param>
    public void LogDataInverse(List<LogTrackFrame> list,string logpdbFilePath,string outputLogDataPath)
    {
        if (list != null&& list.Count!=0)
        {            
            //读取的日志数据
            string strMsg;
            int cnt = list.Count;
            //用于存储帧索引-函数ID序列
            Dictionary<int, List<ushort>> frameIdDic = new Dictionary<int, List<ushort>>();
            //存储所有参数
            List<int> argsList = new List<int>();
            for (int i = 0; i < cnt; i++)
            {                
                int frameIndex = list[i].FrameIndex;                
                List<ushort> idList = new List<ushort>();
                idList.AddRange(list[i].Items);
                if (list[i].Args!=null&&list[i].Args.Count!=0)
                {
                    argsList.AddRange(list[i].Args);
                }                
                frameIdDic.Add(frameIndex, idList);
            }
            strMsg = JsonHandler.OutputAllMes(frameIdDic, argsList, logpdbFilePath);
            if (strMsg != null)
            {
                //写数据
                using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] by = Encoding.UTF8.GetBytes(strMsg);
                    fs.Write(by, 0, by.Length);
                }
            }
        }
    }
    */
    /// <summary>
    /// 序列化数据
    /// </summary>
    /// <param name="cq"></param>
    /// <param name="path"></param>
    public void LogDataSerlializer(string path, CircleQueue<LogTrackFunc> cq)
    {
        if (cq != null)
        {
            int length = cq.QueueLength();
            if (length != 0)
            {
                //cq.ResetCur();
                /*
                List<LogTrackFunc> ltfList = new List<LogTrackFunc>();
                for (int i = 0; i < length; i++)
                {
                    ltfList.Add(cq.GetNext());
                }
                */
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                   // ProtoBuf.Serializer.Serialize<List<LogTrackFunc>>(fs, ltfList);
                    ProtoBuf.Serializer.Serialize<CircleQueue<LogTrackFunc>>(fs, cq);
                }
            }
        }
    }
    //反序列化数据
    public void ReLogDataSerlializer(string path, string pdbFilePath, string outputLogDataPath)
    {
        if (!File.Exists(path))
        {
            return;
        }
        List<LogTrackFunc> ltfList = new List<LogTrackFunc>();
        using (FileStream fs=new FileStream(path,FileMode.Open,FileAccess.Read))
        {
            ltfList = ProtoBuf.Serializer.Deserialize<List<LogTrackFunc>>(fs);
        } 
        LogDataInverse(ltfList, pdbFilePath, outputLogDataPath);
    }


    /// <summary>
    /// 序列化后进行Gzip压缩
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cq"></param>
    public void LogDataSerlializerAndCompress(string path, CircleQueue<LogTrackFunc> cq)
    {
        if (cq != null)
        {
            int length = cq.QueueLength();
            if (length != 0)
            {
                cq.ResetCur();
                List<LogTrackFunc> ltfList = new List<LogTrackFunc>();
                for (int i = 0; i < length; i++)
                {
                    ltfList.Add(cq.GetNext());
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var gzipStream = new GZipOutputStream(File.Open(path, FileMode.Create)))
                    {
                        //将数据序列化后写至内存流中
                        ProtoBuf.Serializer.Serialize<List<LogTrackFunc>>(ms, ltfList);
                        // 重置内存流的读写位置（此步很关键）
                        ms.Position = 0;
                        //定义缓冲区大小
                        int buffersize = 1024;
                        //创建缓冲数据
                        byte[] FileData = new byte[buffersize];
                        //循环读写
                        while (buffersize > 0)
                        {
                            //先将内存流中的数据读到缓冲区中
                            buffersize = ms.Read(FileData, 0, buffersize);
                            //把缓冲区中数据写入到文件流中
                            gzipStream.Write(FileData, 0, buffersize);//写入压缩文件               
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// 解压并进行反序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pdbFilePath"></param>
    /// <param name="outputLogDataPath"></param>
    public void ReLogDataSerlializerAndUncompress(string path, string pdbFilePath, string outputLogDataPath)
    {
        if (!File.Exists(path))
        {
            return;
        }
        List<LogTrackFunc> ltfList = new List<LogTrackFunc>();
        using (MemoryStream ms = new MemoryStream())
        {
            //解压
            using (var stream = new GZipInputStream(File.OpenRead(path)))
            {
                int buffersize = 2048;
                byte[] buffer = new byte[buffersize];
                while (buffersize > 0)
                {
                    buffersize = stream.Read(buffer, 0, buffersize);
                    ms.Write(buffer, 0, buffersize);
                }
            }
            ms.Position = 0;
            //反序列化
            ltfList = ProtoBuf.Serializer.Deserialize<List<LogTrackFunc>>(ms);
        }
        LogDataInverse(ltfList, pdbFilePath, outputLogDataPath);
    }
    /// <summary>
    /// 反序列化数据反解
    /// </summary>
    /// <param name="ltfList"></param>
    /// <param name="logpdbFilePath"></param>
    /// <param name="outputLogDataPath"></param>
    public void LogDataInverse(List<LogTrackFunc> list, string logpdbFilePath, string outputLogDataPath)
    {
        if (list != null && list.Count != 0)
        {
            //读取的日志数据
            string strMsg=null;
            int cnt = list.Count;            
            if (cnt!=0)
            {
                strMsg = JsonHandler.OutputAllMes(list, logpdbFilePath);
            }            
            if (strMsg != null)
            {
                //写数据
                using (FileStream fs = new FileStream(outputLogDataPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] by = Encoding.UTF8.GetBytes(strMsg);
                    fs.Write(by, 0, by.Length);
                }
            }
        }
    }
}
