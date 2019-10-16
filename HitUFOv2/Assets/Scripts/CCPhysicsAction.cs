using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPhysicsAction : SSAction
{
    public float speedx;
    // Use this for initialization
    public override void Start()
    {
        if (!this.gameObject.GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 9.8f * 0.6f, ForceMode.Acceleration);
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(speedx, 0, 0), ForceMode.VelocityChange);
    }

    private CCPhysicsAction()
    {

    }
    public static CCPhysicsAction getAction(float speedx)
    {
        CCPhysicsAction action = CreateInstance<CCPhysicsAction>();
        action.speedx = speedx;
        return action;
    }

    // Update is called once per frame
    override public void Update()
    {
        if (transform.position.y <= 3)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }
}