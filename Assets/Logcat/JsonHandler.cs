using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text.RegularExpressions;

//封装pdb表中的每条数据的结构体
public class LogPdbJson
{
   // private int _id;
    public ushort ID { get; set; }  

   // private string _subPath;
    public string SubPath { get; set; }

   // private int _lineIndex;
    public string ClassName { get; set; }

    public string FuncName { get; set; }

   // private string _comments;
    public string Comments { get; set; }

    public LogPdbJson(ushort _id, string _subpath, string _className,string _funName ,string _comments)
    {
        this.ID = _id;       
        this.SubPath = _subpath;
        this.ClassName = _className;
        this.FuncName = _funName;
        this.Comments = _comments;     
    }
    /*
    public LogPdbJson(int _id, int _argcnt, string _subpath, int _lineIndex, string _comments,string _argType)
    {
        this.ID = _id;
        this.ArgCount = _argcnt;
        this.SubPath = _subpath;
        this.LineIndex = _lineIndex;
        this.Comments = _comments;
        this.ArgType = _argType;
    }
    */
    public LogPdbJson()
    {

    }
}

public class JsonHandler
{
    public static JsonHandler jsHandler = new JsonHandler();
    //LogPdb表中的全部数据
    public List<LogPdbJson> allData=new List<LogPdbJson>();   
    /*
   /// <summary>
   /// 生成LogPdb表
   /// </summary>
   /// <param name="path">生成表的路径</param>
   /// <param name="allDataLog">待写入的数据</param>
    public static void SpawnLogPdbJson(string path,List<LogPdbJson> allDataLog)
    {
        if (allDataLog!=null&&allDataLog.Count!=0)
        {
            int count = allDataLog.Count;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            for (int i = 0; i < count; i++)
            {
                string str = JsonMapper.ToJson(allDataLog[i]);
                if (i == count - 1)
                {
                    sb.AppendFormat("{0}", str);
                    break;
                }
                else
                {
                    sb.AppendFormat("{0}{1}{2}", str, ",", "\r\n");
                }

            }
            sb.Append("]");
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                fs.Write(by, 0, by.Length);
            }
            jsHandler.allData.Clear();
        }                          
    }
    */
    /// <summary>
    /// 解析LogPdb表
    /// </summary>
    /// <param name="path">表的路径</param>
    /// <param name="aData">解析数据存放的集合</param>
    public static void LogPdbReverse(string LogPdbPath,List<LogPdbJson> aData)
    {        
        if (aData!=null&&File.Exists(LogPdbPath))
        {
            if (aData.Count==0)
            {               
                //加载json数据
                JsonData data = JsonMapper.ToObject(File.ReadAllText(LogPdbPath));
                for (int i = 0; i < data.Count; i++)
                {
                    aData.Add(new LogPdbJson((ushort)data[i]["ID"], (string)data[i]["SubPath"],(string)data[i]["ClassName"], (string)data[i]["FuncName"], (string)data[i]["Comments"]));
                }              
            }             
        }       
    }

    /// <summary>
    /// 每条日志结构体对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static LogPdbJson GetItemData(ushort _id,string LogPadbPath)
    {      
        //解析LogPdb表
        LogPdbReverse(LogPadbPath, jsHandler.allData);     
        for (int i = 0; i < jsHandler.allData.Count; i++)
        {
            if (_id== jsHandler.allData[i].ID)
            {
                return jsHandler.allData[i];
            }
        }
        return null;
    }
    /*
    /// <summary>
    /// 每条日志参数个数
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static int GetArgCount(LogPdbJson lpj)
    {
        int argCount = 0;
        if (lpj!=null)
        {
            argCount = lpj.ArgCount;           
        }
        return argCount;
    }
   
    /// <summary>
    /// 日志格式处理（对象和参数）
    /// </summary>
    /// <param name="lpj">每条日志数据结构体</param>
    /// <param name="argAry">每条日志的参数列表</param>
    /// <returns></returns>
    public static string HandlerLogMeg(LogPdbJson lpj,List<int> argAry)
    {
        string debuLogStr = "";
        if (lpj != null)
        {            
            debuLogStr = string.Format(@"ID:{0}---{1}($)(#){2}{3}:{4}", lpj.ID, lpj.Comments, "\r\n",lpj.SubPath, lpj.LineIndex);            
            if (argAry == null||argAry.Count==0)
            {
                //替换参数值
                debuLogStr = debuLogStr.Replace("$", "");
                //替换参数类型
                debuLogStr = debuLogStr.Replace("#", "");
            }
            else
            {
                int argCount = argAry.Count;               
                if (argCount == 1)
                {
                    debuLogStr = debuLogStr.Replace("$", argAry[0].ToString());
                    debuLogStr = debuLogStr.Replace("#", lpj.ArgType.Remove(lpj.ArgType.Length - 1, 1));
                                   
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < argCount; i++)
                    {
                        sb.AppendFormat("{0},",argAry[i]);
                    }
                    //移除最后一逗号
                    sb= sb.Remove(sb.Length - 1, 1);
                    debuLogStr = debuLogStr.Replace("$", sb.ToString());
                    //处理参数类型
                    debuLogStr = debuLogStr.Replace("#", lpj.ArgType.Replace("&", ",").Remove(lpj.ArgType.Length - 1, 1));                    
                }
                if (debuLogStr.IndexOf('&')!=-1)
                {
                    debuLogStr = debuLogStr.Replace("&", ",");
                }
            }            
        }
        return debuLogStr;
    }
    
    /// <summary>
    /// 日志格式处理（对象）
    /// </summary>
    /// <param name="lpj">每条日志数据结构体</param>
    /// <param name="argAry">每条日志的参数列表</param>
    /// <returns></returns>
    public static string HandlerLogMeg(LogPdbJson lpj)
    {
        string debuLogStr = "";
        if (lpj != null)
        {
            debuLogStr = string.Format(@"ID:{0}---{1}($){2}{3}:{4}", lpj.ID, lpj.Comments, "\r\n", lpj.SubPath, lpj.LineIndex);
            if (lpj.ArgCount == 0)
            {
                debuLogStr = debuLogStr.Replace("$", "");
            }
            else
            {
                int argCount = lpj.ArgCount;
                if (argCount == 1)
                {
                    //移除最后一个&                  
                    debuLogStr = debuLogStr.Replace("$", lpj.ArgType.Remove(lpj.ArgType.Length - 1, 1));
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(lpj.ArgType.Replace("&", ",").Remove(lpj.ArgType.Length - 1, 1));                                  
                    debuLogStr = debuLogStr.Replace("$", sb.ToString());
                }
            }
        }
        return debuLogStr;
    }
    */
    /*
    /// <summary>
    ///可读性日志数据输出(Id和frame数组)
    /// </summary>
    /// <param name="idAry">日志id数组</param>
    /// <param name="argAry">参数列表数组</param>
    /// <returns></returns>
    public static string OutputAllMes(Dictionary<int,List<int>> frameAndIdDic,string path)
    {
        string str = null;   
        if (frameAndIdDic!=null&&frameAndIdDic.Count!=0)
        {        
            System.Text.StringBuilder sbAll = new System.Text.StringBuilder();
            //遍历每一帧
            foreach (var item in frameAndIdDic)
            {
                int frameindex = item.Key;
                List<int> idList = new List<int>();
                idList.AddRange(item.Value);
                int idLength = idList.Count;
                //遍历每一个id
                System.Text.StringBuilder sbFrame = new System.Text.StringBuilder();
                for (int i = 0; i < idLength; i++)
                {
                    //获取指定日志数据对象
                    LogPdbJson lpj = GetItemData(idList[i], path);                   
                    string logDataStr = HandlerLogMeg(lpj);
                    sbFrame.AppendFormat("{0}{1}{2}",logDataStr,"\r\n","\r\n");
                }
                sbAll.AppendFormat("********FrameIndex:{0}---Time:{1}********{2}{3}{4}{5}", frameindex, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "\r\n", sbFrame,"\r\n","\r\n");
            }
            str = sbAll.ToString();
            //清空之前解析LogPdb存放的数据   
            jsHandler.allData.Clear();
        }
        return str;
    }

    /// <summary>
    /// 可读性日志数据输出(Id和Frame、Args)
    /// </summary>
    /// <param name="frameAndIdDic">存放Frame-ID序列的字典</param>
    /// <param name="argsList">参数列表</param>
    /// <param name="path">LogPdb映射表路径</param>
    /// <returns></returns>
    public static string OutputAllMes(Dictionary<int, List<int>> frameAndIdDic, List<int> argsList, string path)
    {
        string str = null;
        if (frameAndIdDic != null && argsList != null)
        {
            System.Text.StringBuilder sbAll = new System.Text.StringBuilder();
            //遍历每一帧
            foreach (var item in frameAndIdDic)
            {
                int frameindex = item.Key;
                List<int> idList = new List<int>();
                idList.AddRange(item.Value);
                int idLength = idList.Count;
                //遍历每一个id
                System.Text.StringBuilder sbFrame = new System.Text.StringBuilder();
                //用于记录参数遍历的当前位置
                int curindex = 0;
                for (int i = 0; i < idLength; i++)
                {
                    //获取指定日志数据对象
                    LogPdbJson lpj = GetItemData(idList[i], path);
                    //获取参数个数
                    int argCnt = lpj.ArgCount;
                    //存储每一个函数ID对应的参数列表
                    List<int> argsListById = new List<int>();
                    //每次从上次记录的位置开始遍历
                    int j = curindex;
                    for (; j<curindex+ argCnt; j++)
                    {
                        argsListById.Add(argsList[j]);
                    }
                    //更新遍历的位置
                    curindex = j;
                    //输出每条日志信息
                    string logDataStr = HandlerLogMeg(lpj, argsListById);
                    sbFrame.AppendFormat("{0}{1}{2}", logDataStr, "\r\n", "\r\n");
                }
                sbAll.AppendFormat("********FrameIndex:{0}---Time:{1}********{2}{3}{4}{5}", frameindex, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "\r\n", sbFrame, "\r\n", "\r\n");
            }
            str = sbAll.ToString();
            //清空之前解析LogPdb存放的数据   
            jsHandler.allData.Clear();
        }
        return str;
    }

    /// <summary>
    /// 可读性日志数据输出(Id和Frame、Args)
    /// </summary>
    /// <param name="frameAndIdDic"></param>
    /// <param name="argsList"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string OutputAllMes(Dictionary<int, List<ushort>> frameAndIdDic, List<int> argsList, string path)
    {
        string str = null;
        if (frameAndIdDic != null && argsList != null)
        {
            System.Text.StringBuilder sbAll = new System.Text.StringBuilder();
            //遍历每一帧
            foreach (var item in frameAndIdDic)
            {
                int frameindex = item.Key;
                List<ushort> idList = new List<ushort>();
                idList.AddRange(item.Value);
                int idLength = idList.Count;
                //遍历每一个id
                System.Text.StringBuilder sbFrame = new System.Text.StringBuilder();
                //用于记录参数遍历的当前位置
                int curindex = 0;
                for (int i = 0; i < idLength; i++)
                {
                    //获取指定日志数据对象
                    LogPdbJson lpj = GetItemData(idList[i], path);
                    //获取参数个数
                    int argCnt = lpj.ArgCount;
                    //存储每一个函数ID对应的参数列表
                    List<int> argsListById = new List<int>();
                    //每次从上次记录的位置开始遍历
                    int j = curindex;
                    for (; j < curindex + argCnt; j++)
                    {
                        argsListById.Add(argsList[j]);
                    }
                    //更新遍历的位置
                    curindex = j;
                    //输出每条日志信息
                    string logDataStr = HandlerLogMeg(lpj, argsListById);
                    sbFrame.AppendFormat("{0}{1}{2}", logDataStr, "\r\n", "\r\n");
                }
                sbAll.AppendFormat("********FrameIndex:{0}---Time:{1}********{2}{3}{4}{5}", frameindex, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "\r\n", sbFrame, "\r\n", "\r\n");
            }
            str = sbAll.ToString();
            //清空之前解析LogPdb存放的数据   
            jsHandler.allData.Clear();
        }
        return str;
    }
    /*
    /// <summary>
    ///可读性日志数据输出(Id)
    /// </summary>
    /// <param name="idAry">日志id数组</param>
    /// <param name="argAry">参数列表数组</param>
    /// <returns></returns>
    public static string OutputAllMes( List<int> idAry,string LogPdbPath)
    {       
        string str = null;     
        if (idAry != null)
        {
            int idLength = idAry.Count;                      
            //遍历每一个id
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < idLength; i++)
            {              
                //获取指定日志数据对象
                LogPdbJson lpj = GetItemData(idAry[i], LogPdbPath);
                //返回一条数据
                string logDataStr = HandlerLogMeg(lpj);
                Debug.Log(logDataStr);
                sb.AppendFormat("{0}{1}{2}", logDataStr, "\r\n", "\r\n");
            }
           
            str = sb.ToString();
        }
        //清空之前解析LogPdb存放的数据   
        jsHandler = null;
        return str;
    }
    */
    public static string OutputAllMes(List<LogTrackFunc> list,string logPdbPath)
    {
        string str = null;
        if (list != null&& list.Count!=0)
        {
            int cnt = list.Count;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < cnt; i++)
            {
                //获取LogPdb表的日志对象
                LogPdbJson lpj = GetItemData(list[i].ID, logPdbPath);                                
                //输出每条日志信息
                string logDataStr = HandlerLogMeg(lpj, list[i].Args);
                sb.AppendFormat("{0}{1}{2}", logDataStr, "\r\n", "\r\n");
            }
            str = sb.ToString();
        }
        //清空之前解析LogPdb存放的数据   
        jsHandler.allData.Clear();
        return str;

    }
    public static string HandlerLogMeg(LogPdbJson lpj, List<string> argAry)
    {
        string debuLogStr = "";
        if (lpj != null)
        {
            debuLogStr = string.Format(@"ID:{0}---{1}:{2}($)({3}){4}{5}", lpj.ID, lpj.ClassName, lpj.FuncName,lpj.Comments,"\r\n", lpj.SubPath);
            if (argAry == null || argAry.Count == 0)
            {
                //替换参数值
                debuLogStr = debuLogStr.Replace("$", "");                
            }
            else
            {
                int argCount = argAry.Count;
                if (argCount == 1)
                {
                    debuLogStr = debuLogStr.Replace("$", argAry[0]);                   
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < argCount; i++)
                    {
                        sb.AppendFormat("{0},", argAry[i]);
                    }
                    //移除最后一逗号
                    sb = sb.Remove(sb.Length - 1, 1);
                    debuLogStr = debuLogStr.Replace("$", sb.ToString());                   
                }               
            }
        }
        return debuLogStr;
    }
}