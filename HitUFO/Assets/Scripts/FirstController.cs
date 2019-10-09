using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    readonly float roundTime = 40f;
    private float speed = 1.5f;    
    readonly int passScore = 20;

    private List<GameObject> disks = new List<GameObject>();          
    private int currentRound = 0;                                                 
    private float time = 0f;                                                
    private float currrentTime = 0f;
    private GameState gameState = GameState.START;

    public UserGUI userGUI;
    public ScoreRecorder scoreRecorder;
    public DiskFactory diskFactory;
    public FlyActionManager actionManager;


    void Start()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;

        gameState = GameState.START;
        time = 0f;
        currentRound = 0;
        currrentTime = 0;

        LoadResources();
    }

    void Update()
    {
        if (gameState == GameState.RUNNING)
        {
            for (int i = 0; i < disks.Count; i++)
            {
                if ((disks[i].transform.position.y <= -4.5) && disks[i].gameObject.activeSelf == true)
                {
                    diskFactory.FreeDisk(disks[i]);
                    disks.Remove(disks[i]);
                }
            }
            if (time > speed)
            {
                time = 0;
                SendDisk();
            }
            else
            {
                time += Time.deltaTime;
            }

            if (currrentTime > roundTime)
            {
                currrentTime = 0;
                if (currentRound < 1 && GetScore() >= passScore)
                {
                    currentRound++;
                    time = 0f;
                }
                else
                {
                    GameOver();
                }
            }
            else
            {
                currrentTime += Time.deltaTime;
            }
        }
    }

    private void GameOver()
    {
        gameState = GameState.OVER;
        currrentTime = 40;
    }

    public void LoadResources()
    {
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool isHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];


            if (hit.collider.gameObject.GetComponent<DiskData>() != null)       
            {
                for (int j = 0; j < disks.Count; j++)       
                {
                    if (hit.collider.gameObject.GetInstanceID() == disks[j].gameObject.GetInstanceID())
                    {
                        isHit = true;
                    }
                }
                if (!isHit)       
                {
                    return;
                }

                disks.Remove(hit.collider.gameObject);

                scoreRecorder.Record(hit.collider.gameObject);

                StartCoroutine(WaitingParticle(0.08f, hit, diskFactory, hit.collider.gameObject));
                break;
            }
        }
    }
    
    IEnumerator WaitingParticle(float waitTime, RaycastHit hit, DiskFactory diskFactory, GameObject obj)
    {
        yield return new WaitForSeconds(waitTime);
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        diskFactory.FreeDisk(obj);
    }

    
    private void SendDisk()
    {

        GameObject disk = diskFactory.GetDisk(currentRound);
        disks.Add(disk);
        disk.SetActive(true);
        
        float positionX = 16;
        float ranY = Random.Range(1f, 4f);
        float ranX = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        disk.GetComponent<DiskData>().direction = new Vector3(ranX, ranY, 0);
        Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * positionX, ranY, 0);
        disk.transform.position = position;
        float angle = Random.Range(15f, 20f);
        actionManager.UFOFly(disk, angle);
        
    }

    public void Restart()
    {
        time = 0f;
        currentRound = 0;
        currrentTime = 0;
        scoreRecorder.Reset();
        gameState = GameState.RUNNING;
    }

    public int GetScore()
    {
        return scoreRecorder.score;
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public float GetTime()
    {
        return currrentTime;
    }

    public int GetRound()
    {
        return currentRound;
    }

    public void Pause()
    {
        actionManager.Pause();
    }

    public void Run()
    {
        actionManager.Run();
    }
}