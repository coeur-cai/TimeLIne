using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义数组结构体
/// </summary>
public class IDAry
{

    //当前指示位置
    private int _cur;
    //在取值时用于记录_cur初始位置值
    private int _record;
    //数组容量大小
    private int MaxSize = 10;
    //数组
    ushort[] idAry;
    //数组是否满了
    private bool isfull = false;

    /// <summary>
    /// 初始化数组
    /// </summary>
    public void InitAry()
    {
        idAry = new ushort[MaxSize];
        _cur = _record = 0;
    }
    /// <summary>
    /// 数组入数据
    /// </summary>
    /// <param name="id"></param>
    public void AddItem(ushort id)
    {
        if (idAry == null)
        {
            InitAry();
        }
        idAry[_cur] = id;
        if ((_cur + 1) % MaxSize == 0)
        {
            //数据满了
            isfull = true;
        }
        _cur = (_cur + 1) % MaxSize;
        _record = (_record + 1) % MaxSize;
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <returns></returns>
    public ushort GetItem()
    {
        if (IsEmpty())
        {
            return 0;
        }
        _cur = (_cur - 1 + MaxSize) % MaxSize;
        return idAry[_cur];
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        if (!isfull && _cur == 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取数组长度
    /// </summary>
    /// <returns></returns>
    public int Length()
    {
        if (isfull)
        {
            return MaxSize;
        }
        else
        {
            return _cur % MaxSize;
        }
    }

    /// <summary>
    /// 在取完数据后需要将_cur游标重置一下之前位置
    /// </summary>
    public void ResetCur()
    {
        _cur = _record;
    }

}
public class ArgsAry
{

    //当前指示位置
    private int _cur;
    //在取值时用于记录_cur初始位置值
    private int _record;
    //数组容量大小
    private int MaxSize = 10;
    //数组
    string[] argsAry;
    //数组是否满了
    private bool isfull = false;

    /// <summary>
    /// 初始化数组
    /// </summary>
    public void InitAry()
    {
        argsAry = new string[MaxSize];
        _cur = _record = 0;
    }
    /// <summary>
    /// 数组入数据
    /// </summary>
    /// <param name="id"></param>
    public void AddItem(string arg)
    {
        if (argsAry == null)
        {
            InitAry();
        }
        argsAry[_cur] = arg;
        if ((_cur + 1) % MaxSize == 0)
        {
            //数据满了
            isfull = true;
        }
        _cur = (_cur + 1) % MaxSize;
        _record = (_record + 1) % MaxSize;
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <returns></returns>
    public string GetItem()
    {
        if (IsEmpty())
        {
            return null;
        }
        _cur = (_cur - 1 + MaxSize) % MaxSize;
        return argsAry[_cur];
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        if (!isfull && _cur == 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取数组长度
    /// </summary>
    /// <returns></returns>
    public int Length()
    {
        if (isfull)
        {
            return MaxSize;
        }
        else
        {
            return _cur % MaxSize;
        }
    }

    /// <summary>
    /// 在取完数据后需要将_cur游标重置一下之前位置
    /// </summary>
    public void ResetCur()
    {
        _cur = _record;
    }

    /// <summary>
    /// 一次性存放多个参数值
    /// </summary>
    public void AddItems(params string[] args)
    {
        if (argsAry == null)
        {
            InitAry();
        }
        int length = args.Length;
        for (int i = 0; i < length; i++)
        {
            argsAry[_cur] = args[i];
            if ((_cur + 1) % MaxSize == 0)
            {
                //数据满了
                isfull = true;
            }
            _cur = (_cur + 1) % MaxSize;
            _record = (_record + 1) % MaxSize;
        }
    }

    /// <summary>
    /// 根据个数取元素并返回元素数组
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public string[] GetItems(int count)
    {
        if (count != 0)
        {
            string[] args = new string[count];
            if (IsEmpty())
            {
                return null;
            }
            for (int i = 0; i < count; i++)
            {
                _cur = (_cur - 1 + MaxSize) % MaxSize;
                args[i] = argsAry[_cur];
            }
            return args;
        }
        return null;
    }

}