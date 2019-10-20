using System.Collections;
using System.Collections.Generic;
using System;
using ProtoBuf;


public class CircleQueue<T>
{
    //队列数组
    private T[] datas;
    //维护队列大小为300条日志左右
    private static int MAXSIZE = 600;
    //队首索引
    private int front;
    //队尾索引
    private int rear;

    private int cur;

    private void InitQueue()
    {
        datas = new T[MAXSIZE];
        front = rear = cur = 0;
    }

    /// <summary>
    /// 入队列
    /// </summary>
    /// <param name="value"></param>
    public void Enqueue(T value)
    {
        if (datas == null)
        {
            InitQueue();
        }
        //队满判断
        if ((rear + 1) % MAXSIZE == front)
        {
            if (cur == front)
            {
                //如果在队满情况下，cur游标是跟front指向同一个位置（此时是在rear指针的下一个位置）则在元素入队时需要进行游标前移
                cur = (cur + 1) % MAXSIZE;
            }
            //队满后覆盖之前的数据
            front = (front + 1) % MAXSIZE;            
        }
        datas[rear] = value;
        //尾指针后移
        rear = (rear + 1) % MAXSIZE;
    }

    /// <summary>
    /// 出队
    /// </summary>
    /// <returns></returns>
    public T DeQueue()
    {
        T value = default(T);
        //队空判断
        if (front == rear)
        {
            return value;
        }
        value = datas[front];
        //头指针前移
        front = (front + 1) % MAXSIZE;
        return value;
    }

    /// <summary>
    /// 判空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return front == rear;
    }

    /// <summary>
    /// 求长度
    /// </summary>
    /// <returns></returns>
    public int QueueLength()
    {
        return (rear + MAXSIZE - front) % MAXSIZE;
    }

    /// <summary>
    /// 获取对头元素
    /// </summary>
    /// <returns></returns>
    public T GetFront()
    {
        if (IsEmpty())
        {
            return default(T);
        }
        return datas[front];
    }

    /// <summary>
    /// 获取下一个元素
    /// </summary>
    /// <returns></returns>
    public T GetNext()
    {
        T value = default(T);
        if (IsEmpty())
        {
            return default(T);
        }
        value = datas[cur];
        cur = (cur + 1) % MAXSIZE;
        return value;
    }

    /// <summary>
    /// 重置cur游标
    /// </summary>
    public void ResetCur()
    {
        cur = front;
    }
}
