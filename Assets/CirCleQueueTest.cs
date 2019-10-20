using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CirCleQueueTest : MonoBehaviour {
    int curIndex = 0;
    public CircleQueue<int> que = new CircleQueue<int>();
	// Use this for initialization
	void Start () {
        //EnQueData();
        // PrintData();
        // Debug.Log("----------------");
        // EnQueData2();
        // PrintData();
        // Debug.Log("----------------");
        // //EnQueData3();
        //PrintData();
        Type intType = typeof(int);
        Debug.Log(intType.Name);
        if (intType.Name.CompareTo("Int32") == 0)
        {
            Debug.Log("int");
        }
        else
        {
            Debug.Log("fp");
        }
    }
	
	// Update is called once per frame
	void Update () {
       // curIndex++;
       // EnQueData();
        /*
        if (curIndex%50 == 0)
        {
            PrintData();
        }
        */
	}
    void EnQueData()
    {
        for (int i = 0; i < 100; i++)
        {
            que.Enqueue(i);
        }
    }
    void EnQueData2()
    {
        for (int i = 0; i < 70; i++)
        {
            que.Enqueue(i+100);
        }
    }
    void EnQueData3()
    {
        for (int i = 0; i < 200; i++)
        {
            que.Enqueue(i + 79);
        }
    }
    void PrintData()
    {
        int cqLength = que.QueueLength();
        if (cqLength!=0)
        {
            //重置一下游标
            que.ResetCur();
            for (int i = 0; i < cqLength; i++)
            {
                Debug.Log(que.GetNext());
            }
        }
    }
}
