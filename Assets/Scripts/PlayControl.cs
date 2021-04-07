using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    private Transform self;

    /// <summary>
    /// 外部组件
    /// </summary>
    private MapManager map;
    private CameraControl myCamera;
    private NGUIManager myNGUI;

    //颜色
    private Color colorOne = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color colorTwo = new Color(100 / 255f, 80 / 255f, 150 / 255f);


    /// <summary>
    /// 移动坐标控制
    /// </summary>
	private int x = 2;
    private int y = 3;
    private Transform temp;

    /// <summary>
    /// 动态地图控制与陷阱控制
    /// </summary>
    /// a为地图生成个数
    /// ctControl为陷阱上下移动控制器
    private int a = 1;

    /// <summary>
    /// 开始移动
    /// </summary>
    private bool startMove = false;

    /// <summary>
    /// 掉落控制
    /// </summary>
    /// a是塌陷角标，控制地图塌陷与角色死亡交互
    private int d = 0;
    public int spikeDestroy = 0;

    /// <summary>
    /// 分数存储
    /// </summary>
    private int awardScore = 0;
    private int moveScore = 0;

    // Use this for initialization
    void Start()
    {
        awardScore = PlayerPrefs.GetInt("awardScore", 0);

        self = gameObject.GetComponent<Transform>();

        map = GameObject.Find("MapManager").GetComponent<MapManager>();
        myCamera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        myNGUI = GameObject.Find("UI Root").GetComponent<NGUIManager>();

        //一个bug，在start这个时间点，地图还未生成，造成参数范围异常的bug
        //Birth();
        //故一下输出结果为0
        //Debug.Log(map.mapSave.Count);

        UIEventListener.Get(gameObject).onClick = Func;


    }

    private void Func(GameObject go)
    {

    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("awardScore", awardScore);
        if (PlayerPrefs.GetInt("moveScore", 0) < moveScore)
        {
            PlayerPrefs.SetInt("moveScore", moveScore);
            Debug.Log("max moveScore =" + PlayerPrefs.GetInt("moveScore", 0));
        }
        myNGUI.star_awardText.text = PlayerPrefs.GetInt("awardScore", 0).ToString() + "/100";
    }

    /// <summary>
    /// 人物出生
    /// </summary>
    private void Birth()
    {
        temp = map.mapSave[x][y].GetComponent<Transform>();
        self.position = temp.position;
        self.rotation = temp.rotation;
        temp.FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
        StartCoroutine("FallingDown");
        myNGUI.game_awardText.text = PlayerPrefs.GetInt("awardScore", 0).ToString() + "/100";
    }

    public void GameStart()
    {
        Birth();
        startMove = true;
        myCamera.startFollow = true;
    }

    void Update()
    {

        //允许移动
        if (startMove)
        {
            Move();
        }

        CalcPosition();

    }

    /// <summary>
    /// 人物移动
    /// </summary>
    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //这段控制不简单，关键点xy都动
            if (y != 4 || x % 2 == 0)
            {
                x++;
            }
            if (x % 2 == 0 && y != 4)
            {
                y++;
            }

            PlayMove();

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //难度稍稍简单一点，关键点只有x动
            if (x % 2 == 0 && y != 0)
            {
                y--;
            }
            if (y != 0 || x % 2 == 0)
            {
                x++;
            }
            PlayMove();

        }
    }

    //生成路面
    private void CalcPosition()
    {
        if (26 * a - x <= 20)
        {
            map.Create(13 * a);
            a++;
            //基数增加
            map.AddBase();
        }
    }

    //地面塌陷
    IEnumerator FallingDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < map.mapSave[d].Length; i++)
            {
                //添加刚体并令其自动销毁
                Rigidbody rb = map.mapSave[d][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1, 10);
                if (map.mapSave[d][i].tag == "Earth" || map.mapSave[d][i].tag == "Sky")
                {
                    spikeDestroy++;
                }
                GameObject.Destroy(map.mapSave[d][i], 1.0f);
            }
            //游戏结束角色死亡
            if (x == d)
            {
                GameOver();
                self.gameObject.AddComponent<Rigidbody>();
            }
            d++;
        }
    }

    //角色移动
    private void PlayMove()
    {
        AddMoveScore();
        Transform myself = map.mapSave[x][y].GetComponent<Transform>();
        self.position = myself.position + new Vector3(0, 0.254f / 2, 0);
        //坑洞陷阱
        if (myself.tag == "Untagged")
        {
            GameOver();
            self.gameObject.AddComponent<Rigidbody>();
        }

        //蜗牛轨迹
        if (myself.tag == "Title")
        {
            MeshRenderer mr = myself.FindChild("normal_a2").GetComponent<MeshRenderer>();
            if (x % 2 == 0)
            {
                mr.material.color = colorTwo;
            }
            else
            {
                mr.material.color = colorOne;
            }
        }
        else if (myself.tag == "Sky")
        {
            MeshRenderer mr = myself.FindChild("smashing_spikes_a2").GetComponent<MeshRenderer>();
            if (x % 2 == 0)
            {
                mr.material.color = colorTwo;
            }
            else
            {
                mr.material.color = colorOne;
            }
        }
        else if (myself.tag == "Earth")
        {
            MeshRenderer mr = myself.FindChild("moving_spikes_a2").GetComponent<MeshRenderer>();
            if (x % 2 == 0)
            {
                mr.material.color = colorTwo;
            }
            else
            {
                mr.material.color = colorOne;
            }
        }
    }

    //陷阱与玩家交互的死亡判断
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Spikes_Attack")
        {
            GameOver();
        }
        else if (coll.tag == "Award")
        {
            Destroy(coll.GetComponent<Transform>().parent.gameObject);
            AddAwardScore();
        }
    }

    //奖励品分数增加
    public void AddAwardScore()
    {
        awardScore++;
        PlayerPrefs.SetInt("awardScore", awardScore);
        Debug.Log(PlayerPrefs.GetInt("awardScore", 0));
        myNGUI.game_awardText.text = PlayerPrefs.GetInt("awardScore", 0).ToString() + "/100";
    }

    //移动分数增加
    public void AddMoveScore()
    {
        moveScore++;
        myNGUI.game_moveText.text = moveScore.ToString();
    }

    //游戏结束
    private void GameOver()
    {
        startMove = false;
        myCamera.startFollow = false;
        StopCoroutine("FallingDown");
        SaveScore();
        moveScore = 0;
        awardScore = PlayerPrefs.GetInt("awardScore", 0);
        StartCoroutine("RestartGame");
    }

    //重新开始游戏
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        ResetAll();
    }

    private void ResetPlayer()
    {
        GameObject.Destroy(gameObject.GetComponent<Rigidbody>(), 0);
        x = 2;
        y = 3;
        d = 0;
        a = 1;
    }

    private void ResetAll()
    {
        myNGUI.Reset();
        ResetPlayer();
        map.ResetMap();
        myCamera.ResetCamera();
    }
}
