using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.BZip2;


public class Compress {    	

    /// <summary>
    /// Gzip壓縮字節數組
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="zipfilename"></param>
    /// <returns></returns>
    public static bool  GZipCompressByte(byte[] bytes,string zipfilename) //56-49
    {
        bool blResult;//表示压缩是否成功的返回结果
        using (var stream=new GZipOutputStream(File.Open(zipfilename,FileMode.Create)))
        {
             stream.Write(bytes, 0, bytes.Length);            
             blResult = true;
        }
        return blResult;
    }
   
    /// <summary>
    /// Gzip解壓字節數組
    /// </summary>
    /// <param name="zipfilename"></param>
    /// <returns></returns>
    public static bool  GzipUnCompressByte(string zipfilename,string unzipfilename)
    {
        bool blResult;//表示压缩是否成功的返回结果
        using (FileStream fs=File.Create(unzipfilename))
        {
            using (GZipInputStream gzipinput=new GZipInputStream(File.OpenRead(zipfilename)))
            {
                int buffersize = 1024;
                byte[] by = new byte[buffersize];
                while (buffersize>0)
                {
                    buffersize = gzipinput.Read(by, 0, buffersize);
                    fs.Write(by, 0, buffersize);                    
                }
                blResult = true;
            }
        }
        return blResult;
    }
    
    /// <summary>
    /// Gzip压缩文件
    /// </summary>
    /// <param name="sourcename">源文件</param>
    /// <param name="zipfilename">壓縮後文件</param>
    /// <returns></returns>
    public static bool GzipCompressFile(string sourcename, string zipfilename) //232-86   2.74KB-105字節    38.3KB-251字節
    {
        bool blResult;//表示压缩是否成功的返回结果
                      //为源文件创建读取文件的流实例
        FileStream srcFile = File.OpenRead(sourcename);
        using (var stream = new GZipOutputStream(File.Open(zipfilename, FileMode.Create)))
        {
            byte[] FileData = new byte[srcFile.Length];//创建缓冲数据
            srcFile.Read(FileData, 0, (int)srcFile.Length);//读取源文件
            stream.Write(FileData, 0, FileData.Length);//写入压缩文件
            blResult = true;
        }
        return blResult;
    }

    /// <summary>
    /// Gzip解压
    /// </summary>
    /// <param name="zipfilename">压缩源文件</param>
    /// <param name="unzipfilename">解压后文件</param>
    /// <returns></returns>
    public static bool GzipUnCompressToFile(string zipfilename, string unzipfilename)
    {
        bool blResult;//表示解压是否成功的返回结果
        if (!File.Exists(zipfilename))
        {
            return false;
        }
        using (FileStream destFile = File.Open(unzipfilename, FileMode.Create))
        {
            using (var stream = new GZipInputStream(File.OpenRead(zipfilename)))
            {
                int buffersize = 2048;
                byte[] buffer = new byte[buffersize];
                while (buffersize > 0)
                {
                    buffersize = stream.Read(buffer, 0, buffersize);
                    destFile.Write(buffer, 0, buffersize);
                }
                blResult = true;
            }
        }
        return blResult;
    }

    /// <summary>
    /// Bzip壓縮文件
    /// </summary>
    /// <param name="sourcename">源文件</param>
    /// <param name="zipfilename">压缩后文件</param>
    /// <returns></returns>
    public static bool BzipCompressFile(string sourcename, string zipfilename) //232-89     2.74KB-114字節  38.3KB-412字節
    {
        bool blResult;//表示压缩是否成功的返回结果
                      //为源文件创建读取文件的流实例
        FileStream srcFile = File.OpenRead(sourcename);
        using (var stream = File.Open(zipfilename, FileMode.Create))
        {
            //压缩级别为1-9,1是最低，9是最高的
            BZip2.Compress(srcFile, stream, true, 9);
            blResult = true;
        }
        return blResult;
    }
    
    /// <summary>
    /// Bzip解壓
    /// </summary>
    /// <param name="zipfilename">压缩源文件</param>
    /// <param name="unzipfilename">解压后文件</param>
    /// <returns></returns>
    public static bool BzipUnCompressToFile(string zipfilename, string unzipfilename)
    {
        bool blResult;//表示解压是否成功的返回结果   
        if (!File.Exists(zipfilename))
        {
            return false;
        }
        FileStream stream = File.OpenRead(zipfilename);
        using (FileStream destFile = File.Open(unzipfilename, FileMode.Create))
        {
            //true表示解压完成后关闭两个流
            BZip2.Decompress(stream, destFile, true);
            blResult = true;
        }
        return blResult;
    }
  
}
