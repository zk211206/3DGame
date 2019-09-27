using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // Start is called before the first frame update

    public string status;

    void Awake()
    {
        status = "playing";
    }

    void Start()
    {
        status = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Judge()
    {
        status = "playing";
    }

    public string getStatus()
    {
        return status;
    }

    public void setStatus(string s)
    {
        status = s;
    }

}
