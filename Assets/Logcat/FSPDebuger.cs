using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//定义结构体用于存储一条日志的数据
[ProtoContract]
public class LogTrackFunc
{   
  
    private ushort _id;
    [ProtoMember(1)]
    public ushort ID
    {
        get {return _id; }
        set { _id = value; }
    }
    
    private  List<string> _args;
    [ProtoMember(2)]
    public List<string> Args
    {
        get {return _args; }
        set { _args = value; }
    }    
}
/// <summary>
/// 定义的Debug类
/// </summary>
public static class FSPDebuger {

    //日志ID序列
    //public static List<ushort> ms_currLogTrackItems = new List<ushort>();
    //日志参数序列
   // public static List<string> ms_currLogTrackArgs = new List<string>();
    //帧日志数据队列
    //[ProtoContract]
    public static CircleQueue<LogTrackFunc> LogTrackLoopQueue = new CircleQueue<LogTrackFunc>();
  

    public static void LogTrack(string test)
    {

    }
   
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);
        //ms_currLogTrackArgs.Add(arg3);
        //ms_currLogTrackArgs.Add(arg4);
        //ms_currLogTrackArgs.Add(arg5);
        //ms_currLogTrackArgs.Add(arg6);
        //ms_currLogTrackArgs.Add(arg7);
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1, arg2, arg3, arg4, arg5, arg6,arg7 };
        LogTrackLoopQueue.Enqueue(ltf);
    }
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2, string arg3, string arg4, string arg5, string arg6)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);
        //ms_currLogTrackArgs.Add(arg3);
        //ms_currLogTrackArgs.Add(arg4);
        //ms_currLogTrackArgs.Add(arg5);
        //ms_currLogTrackArgs.Add(arg6);
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1, arg2, arg3, arg4, arg5 ,arg6};
        LogTrackLoopQueue.Enqueue(ltf);
    }
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2, string arg3, string arg4, string arg5)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);
        //ms_currLogTrackArgs.Add(arg3);
        //ms_currLogTrackArgs.Add(arg4);
        //ms_currLogTrackArgs.Add(arg5);  
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1, arg2, arg3, arg4,arg5 };
        LogTrackLoopQueue.Enqueue(ltf);
    }
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2, string arg3, string arg4)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);
        //ms_currLogTrackArgs.Add(arg3);
        //ms_currLogTrackArgs.Add(arg4);    
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1, arg2, arg3,arg4 };
        LogTrackLoopQueue.Enqueue(ltf);
    }
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2, string arg3)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);
        //ms_currLogTrackArgs.Add(arg3);
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1, arg2 ,arg3};
        LogTrackLoopQueue.Enqueue(ltf);
    }
    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1, string arg2)
    {
        //ms_currLogTrackItems.Add(hash);
        //ms_currLogTrackArgs.Add(arg1);
        //ms_currLogTrackArgs.Add(arg2);      
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1,arg2 };
        LogTrackLoopQueue.Enqueue(ltf);
    }

    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash, string arg1)
    {
        // ms_currLogTrackItems.Add(hash);       
        //ms_currLogTrackArgs.Add(arg1);
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = new List<string> { arg1 };
        LogTrackLoopQueue.Enqueue(ltf);
    }

    /// <summary>
    /// 输出一条日志信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public static void LogTrack(ushort hash)
    {
        //ms_currLogTrackItems.Add(hash);
        LogTrackFunc ltf = new LogTrackFunc();
        ltf.ID = hash;
        ltf.Args = null;
        LogTrackLoopQueue.Enqueue(ltf);
    }

    /*
    /// <summary>
    /// 当进入一帧后切换
    /// </summary>
    /// <param name="frameIndex"></param>
    public static void EnterTrackFrame(int frameIndex)
    {
        if (ms_currLogTrackItems.Count != 0)
        {
            LogTrackFrame temp = new LogTrackFrame();
            temp.FrameIndex = frameIndex;
            //深拷贝ID
            temp.Items = new List<ushort>(ms_currLogTrackItems.ToArray());
            //深拷贝参数 
            temp.Args = new List<int>(ms_currLogTrackArgs.ToArray());                 
            LogTrackLoopQueue.Enqueue(temp);            
            ms_currLogTrackItems.Clear();
            ms_currLogTrackArgs.Clear();
        }
    }
    

    /// <summary>
    /// 计算当前帧的函数ID累加和
    /// </summary>
    /// <returns></returns>
    public static long  SumCurFrameFunId(CircleQueue<LogTrackFrame> ltfQue)
    {
        long  sum = 0;
        if (ltfQue!=null&&ltfQue.QueueLength()!=0)
        {
            ltfQue.ResetCur();
            LogTrackFrame ltf = ltfQue.GetNext();
            int idcnt = ltf.Items.Count;                       
            for (int i = 0; i < idcnt; i++)
            {
                sum +=ltf.Items[i];
            }                                      
        }
        return sum;
    }

    /// <summary>
    ///  计算当前帧的函数ID和参数累加和
    /// </summary>
    /// <param name="ltfQue"></param>
    /// <returns></returns>
    public static long  SumCurFrameFunIdAndArgs(CircleQueue<LogTrackFrame> ltfQue)
    {
        long  sum = 0;
        if (ltfQue != null && ltfQue.QueueLength() != 0)
        {
            ltfQue.ResetCur();
            LogTrackFrame ltf = ltfQue.GetNext();
            int idcnt = ltf.Items.Count;
            int argscnt = ltf.Args.Count;
            for (int i = 0; i < idcnt; i++)
            {
                sum += ltf.Items[i];
            }
            for (int i = 0; i < argscnt; i++)
            {
                sum += ltf.Args[i];
            }
        }
        return sum;
    }
    */

}