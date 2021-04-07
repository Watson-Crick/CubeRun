using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{

    private GameObject myFloor;
    private GameObject myWall;
    private GameObject myHole;
    private GameObject mySpike_1;
    private GameObject mySpike_2;
    private GameObject myAward;

    /// <summary>
    /// 地图数据存储。
    /// </summary>
    public List<GameObject[]> mapSave = new List<GameObject[]>();
    public List<GameObject> spikeTrap = new List<GameObject>();

    //用于将生成物加入父类
    private Transform myTransform;

    /// <summary>
    /// 部分数值
    /// </summary>
    private int proHole = 0;
    private int proSpike = 0;
    private int proAward = 1;
    private float buttomLength = Mathf.Sqrt(2) * 0.254f;

    /// <summary>
    /// 颜色
    /// </summary>
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);

    //角色控制脚本
    private PlayControl pc;

    // Use this for initialization
    void Start()
    {
        //动态获取资源
        myFloor = Resources.Load("tile_white") as GameObject;
        myWall = Resources.Load("wall2") as GameObject;
        myHole = Resources.Load("NULL") as GameObject;
        mySpike_1 = Resources.Load("moving_spikes") as GameObject;
        mySpike_2 = Resources.Load("smashing_spikes") as GameObject;
        myAward = Resources.Load("gem 2") as GameObject;

        //获取角色控制脚本
        pc = GameObject.Find("cube_box").GetComponent<PlayControl>();
        myTransform = gameObject.GetComponent<Transform>();

        Create(0);
    }

    /// <summary>
    /// 生成地图
    /// </summary>
    /// <param name="a">判断地图开始行数</param>
	public void Create(int a)
    {
        for (int i = a; i < 13 + a; i++)
        {
            GameObject[] row_1 = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                //地面位置
                Vector3 dir = new Vector3(j * buttomLength, 0, i * buttomLength);
                //旋转方向
                Vector3 rot = new Vector3(-90, 0, 45);
                GameObject temp = null;
                //墙壁生成
                if (j == 0 || j == 5)
                {
                    temp = GameObject.Instantiate(myWall, dir, Quaternion.Euler(rot), myTransform);
                    temp.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    int pro = Trap();
                    if (pro == 0)
                    {
                        temp = GameObject.Instantiate(myFloor, dir, Quaternion.Euler(rot), myTransform);
                        temp.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                        temp.GetComponent<MeshRenderer>().material.color = colorOne;
                        if (Award() == 1 && i >= 5)
                        {
                            Vector3 awardDir = new Vector3(dir.x, dir.y + 0.06f, dir.z);
                            GameObject go = GameObject.Instantiate(myAward, awardDir, Quaternion.identity, myTransform);
                        }
                    }
                    else if (pro == 1)
                    {
                        temp = GameObject.Instantiate(myHole, dir, Quaternion.Euler(rot), myTransform);
                        temp.GetComponent<Transform>().position = dir;
                        temp.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pro == 2)
                    {
                        temp = GameObject.Instantiate(mySpike_1, dir, Quaternion.Euler(rot), myTransform);
                        spikeTrap.Add(temp);
                    }
                    else if (pro == 3)
                    {
                        temp = GameObject.Instantiate(mySpike_2, dir, Quaternion.Euler(rot), myTransform);
                        spikeTrap.Add(temp);
                    }
                }
                row_1[j] = temp;
            }
            mapSave.Add(row_1);
            GameObject[] row_2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                //地面位置
                Vector3 dir = new Vector3(j * buttomLength + buttomLength / 2, 0, i * buttomLength + buttomLength / 2);
                //旋转方向
                Vector3 rot = new Vector3(-90, 0, 45);
                GameObject temp = null;

                int pro = Trap();
                if (pro == 0)
                {
                    temp = GameObject.Instantiate(myFloor, dir, Quaternion.Euler(rot), myTransform);
                    temp.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                    temp.GetComponent<MeshRenderer>().material.color = colorTwo;
                    if (Award() == 1 && i >= 5)
                    {
                        Vector3 awardDir = new Vector3(dir.x, dir.y + 0.06f, dir.z);
                        GameObject go = GameObject.Instantiate(myAward, awardDir, Quaternion.identity, myTransform);
                    }
                }
                else if (pro == 1)
                {
                    temp = GameObject.Instantiate(myHole, dir, Quaternion.Euler(rot), myTransform);
                    temp.GetComponent<Transform>().position = dir;
                    temp.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pro == 2)
                {
                    temp = GameObject.Instantiate(mySpike_1, dir, Quaternion.Euler(rot), myTransform);
                    spikeTrap.Add(temp);
                }
                else if (pro == 3)
                {
                    temp = GameObject.Instantiate(mySpike_2, dir, Quaternion.Euler(rot), myTransform);
                    spikeTrap.Add(temp);
                }
                row_2[j] = temp;
            }
            mapSave.Add(row_2);
        }
    }

    /// <summary>
    /// 陷阱生成概率计算
    /// </summary>
    /// <returns></returns>
    private int Trap()
    {
        int temp = Random.Range(0, 100);
        //返回1生成空洞
        if (temp <= proHole)
        {
            return 1;
        }
        else if (temp > 31 && temp < proSpike + 30)
        {
            return 2;
        }
        else if (temp > 61 && temp < proSpike + 60)
        {
            return 3;
        }
        return 0;
    }

    //奖励概率计算
    private int Award()
    {
        int temp = Random.Range(0, 100);
        if (temp >= proAward)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// 概率叠加
    /// </summary>
    public void AddBase()
    {
        proHole += 2;
        proSpike += 2;
    }

    public void ResetMap()
    {
        Transform[] son = myTransform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < son.Length; i++)
        {
            GameObject.Destroy(son[i].gameObject, 0);
        }
        proHole = 0;
        proSpike = 0;
        proAward = 1;
        mapSave.Clear();
        Create(0);
    }
}
