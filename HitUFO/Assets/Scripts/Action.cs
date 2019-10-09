using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject        
{

    public bool enable = true;                     
    public bool destroy = false;                  

    public GameObject gameobject;
    public Transform transform;  
    public ISSActionCallback callback;

    protected SSAction() { }

    public virtual void Start() 
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class UFOFlyAction : SSAction
{

    public float gravity = -5;                        
    private Vector3 startVector;                     
    private Vector3 gravityVector = Vector3.zero;   
    private float time;                               
    private Vector3 currentAngle = Vector3.zero;           
    public bool run = true;

    private UFOFlyAction() { }
    public static UFOFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
        
        UFOFlyAction action = CreateInstance<UFOFlyAction>();
        if (direction.x == -1)
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update()
    {
        if (run)
        {
            
            time += Time.fixedDeltaTime;
            gravityVector.y = gravity * time;

            
            transform.position += (startVector + gravityVector) * Time.fixedDeltaTime;
            currentAngle.z = Mathf.Atan((startVector.y + gravityVector.y) / startVector.x) * Mathf.Rad2Deg;
            transform.eulerAngles = currentAngle;

            
            if (this.transform.position.y < -10)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
    public override void Start() { }
}

public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;   
    public int repeat = -1;         
    public int start = 0;          

    public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update(); 
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destroy = true;               
                this.callback.SSActionEvent(this); 
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;               
            action.Start();
        }
    }

    void OnDestroy()
    {
       
    }
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

public class SSActionManager : MonoBehaviour, ISSActionCallback                    
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();                     
    private List<int> waitingDelete = new List<int>();                             

    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;                       
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {

    }
}

public class FlyActionManager : SSActionManager 
{

    private UFOFlyAction fly;    

    public FirstController sceneController;

    protected void Start()
    {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentScenceController;
        sceneController.actionManager = this;
    }

    public void UFOFly(GameObject disk, float angle)
    {
        fly = UFOFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, disk.GetComponent<DiskData>().speed);
        this.RunAction(disk, fly, this);
    }

    public void Pause()
    {
        fly.run = false;
    }

    public void Run()
    {
        fly.run = true;
    }

}