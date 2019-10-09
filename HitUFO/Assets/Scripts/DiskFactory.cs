﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject disk;            
    private List<DiskData> used = new List<DiskData>();  
    private List<DiskData> free = new List<DiskData>();  

    public GameObject GetDisk(int round)
    {
        disk = null;
        if (free.Count > 0)
        {
            disk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            disk = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
            disk.AddComponent<DiskData>();
        }
       
        int start = 0;
        int selectedColor = Random.Range(start, round * 400);

        if (selectedColor >= 200)
        {
            round = 1;
        }
        else
        {
            round = 0;
        }


        switch (round)
        {

            case 0:
                {
                    disk.GetComponent<DiskData>().color = Color.yellow;
                    disk.GetComponent<DiskData>().speed = Random.Range(10f, 12f);
                    float startX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    disk.GetComponent<DiskData>().direction = new Vector3(startX, 1, 0);
                    disk.GetComponent<DiskData>().score = 1;
                    disk.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                }
            case 1:
                {
                    disk.GetComponent<DiskData>().color = Color.red;
                    disk.GetComponent<DiskData>().speed = Random.Range(15f, 18f);
                    float startX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    disk.GetComponent<DiskData>().direction = new Vector3(startX, 1, 0);
                    disk.GetComponent<DiskData>().score = 2;
                    disk.GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
            
        }

        used.Add(disk.GetComponent<DiskData>());
        return disk;
    }

    public void FreeDisk(GameObject disk)
    {
        for (int i = 0; i < used.Count; i++)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}