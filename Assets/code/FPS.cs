using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {
    [SerializeField]
    private float fpsByDeltatime = 1f;
    private float passedtime = 0.0f;
    private int frameCount = 0;
    private float realtimeFPS = 0.0f;
    public Text showFPSText;
    // Use this for initialization
    void Start () {
        
        SetFPS();
	}
	
	// Update is called once per frame
	void Update () {
        GetFPS();
	}
    private void SetFPS()
    {
        //设置应用平台目标帧率为60
        Application.targetFrameRate = 60;
    }
    private void GetFPS()
    {
        if (showFPSText==null)
        {
            return;
        }
        frameCount++;
        passedtime += Time.deltaTime;
        if (passedtime>=fpsByDeltatime)
        {
            realtimeFPS = frameCount / passedtime;
            showFPSText.text = "FPS:  " + realtimeFPS.ToString("f1");
            passedtime = 0.0f;
            frameCount = 0;

        }
    }
}
