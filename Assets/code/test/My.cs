using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My{

    public void MyStudy(string t1,string t2)
    {
        FSPDebuger.LogTrack(5,t1,t2);/*study*/
        //Debug.Log("study");
    }

    public  void MySex(int sex)
   {
         FSPDebuger.LogTrack(4 , sex.ToString());/*Sex*/   
        //Debug.Log("Sex");
    }

    public void MyWeight(string weight)
    {
        FSPDebuger.LogTrack(3 , weight);/*weight*/
        //Debug.Log("weight");
    }

    public void MyHeight(string height)
    {
        FSPDebuger.LogTrack(2 , height);/*height*/
        //Debug.Log("height");
    }


    public void Myfavorite(int name, int sex, int age)
    {
         FSPDebuger.LogTrack(1,name.ToString(),sex.ToString(),age.ToString());/*favorite*/
        //Debug.Log("favorite");
    }   
}
