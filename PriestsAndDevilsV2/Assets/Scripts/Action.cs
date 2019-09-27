using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boatMove(boatController boat)
    {
        if (boat.size == "right")
        {
            boat.updateBoatMove.setAim(boat.leftPos);
            boat.size = "left";
        }
        else
        {
            boat.updateBoatMove.setAim(boat.rightPos);
            boat.size = "right";
        }
    }

    public void getOnBoat(boatController boatCtrl,peopleController peopleCtrl)
    {
        peopleCtrl.status = "boat";
        peopleCtrl.people.transform.parent = boatCtrl.getBoat().transform;
        peopleCtrl.people.transform.localPosition = boatCtrl.getBoatPos(peopleCtrl.getName());

    }

    public void getOffBoat(environmentController envCtrl, peopleController peopleCtrl)
    {
        peopleCtrl.status = "shore";
        peopleCtrl.people.transform.parent = null;
        peopleCtrl.people.transform.position = envCtrl.getPosVec(peopleCtrl.size, peopleCtrl.number);
    }

}
