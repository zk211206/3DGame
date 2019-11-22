using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour
{
        public float bloodValue;

    private float tmpValue;

    void Start()
    {
        tmpValue = 0.0f;
        bloodValue = 0.0f;
    }

    void OnGUI()
    {    
        //点击加血
        if (GUI.Button(new Rect(250, 20, 40, 20), "+"))
        {
            tmpValue = 1.0f;
        }

        //点击减血
        if (GUI.Button(new Rect(250, 50, 40, 20), "-"))
        {
            tmpValue = 0.0f;
        }

        bloodValue = Mathf.Lerp(bloodValue, tmpValue, 0.01f);

        GUI.color = Color.red; //血条，设置为红色
        GUI.HorizontalScrollbar(new Rect(20, 20, 200, 20), 0.0f, bloodValue,0.0f, 1.0f, GUI.skin.GetStyle("HorizontalScrollbar"));

    }
}
