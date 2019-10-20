using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

//LogPdb中日志代码的数据结构
public class LogTrackCodeInfo
{
    //日志ID
    public ushort ID { get; set; }
    //子路径
    public string SubPath { get; set; }
    //类名
    public string ClassName { get; set; }
    //函数名
    public string FuncName { get; set; }
    //注释
    public string Comments { get; set; }
    //参数个数
    public int ArgsCount { get; set; }
}

//管理LogPdb
public class LogTrackPdbFile
{
    //维护字典，对应要生成的LogPdb
    private Dictionary<ushort, LogTrackCodeInfo> logTrackCodeInfoDic = new Dictionary<ushort, LogTrackCodeInfo>();
    //记录返回的ID
    private ushort returnID = 0;

    //为字典新增条目
    //传入参数：1.子路径  2.类名  3.函数名   4.注释   5.参数个数          
    //返回：合法的日志ID
    public ushort AddItem(string subPath,string className, string funcName, string comments,int argsCount)
    {
        //从字典中取得尚未被占用的最小ID替换并返回
        while (true)
        {
            returnID++;
            if (!logTrackCodeInfoDic.ContainsKey(returnID))
            {
                break;
            }
        }
        //封装数据
        LogTrackCodeInfo info = new LogTrackCodeInfo();
        info.ID = returnID;
        info.SubPath = subPath;
        info.ClassName = className;
        info.FuncName = funcName;
        info.Comments = comments;
        info.ArgsCount = argsCount;
        //添加到字典
        logTrackCodeInfoDic.Add(info.ID, info);
        return returnID;
    }

    //写入LogPdb.json文件
    public void CreateLogPdb(string fullPath)
    {
        string logPdbText = "[";
        //将新字典转换为Json文本
        foreach (var item in logTrackCodeInfoDic)
        {
            logPdbText += JsonMapper.ToJson(item.Value);
            logPdbText += ",\n";
        }
        //去除最后的,\n
        logPdbText = logPdbText.Substring(0, logPdbText.Length - 2);
        logPdbText += "\n]";
        //创建并写入LogPdb.json文件
        File.WriteAllText(fullPath, logPdbText);
    }
}

