using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUIManager : MonoBehaviour
{

    private PlayControl myPlayer;

    private GameObject startUI;
    private GameObject gameUI;
    private GameObject stopUI;

    private UILabel start_moveText;
    public UILabel star_awardText;
    public UILabel game_moveText;
    public UILabel game_awardText;
    //private UILabel stop

    private GameObject myButton;

    // Use this for initialization
    void Start()
    {
        myButton = GameObject.Find("myButton");

        startUI = GameObject.Find("Start_UI");
        gameUI = GameObject.Find("Game_UI");
        stopUI = GameObject.Find("Stop_UI");

        myPlayer = GameObject.Find("cube_box").GetComponent<PlayControl>();

        start_moveText = GameObject.Find("moveBestScore").GetComponent<UILabel>();
        star_awardText = GameObject.Find("awardNowScore").GetComponent<UILabel>();
        game_awardText = GameObject.Find("awardGameScore").GetComponent<UILabel>();
        game_moveText = GameObject.Find("moveNowScore").GetComponent<UILabel>();

        if (PlayerPrefs.GetInt("awardScore", 0) > 100)
        {
            PlayerPrefs.SetInt("awardScore", 100);
        }
        star_awardText.text = PlayerPrefs.GetInt("awardScore", 0).ToString() + "/100";
        start_moveText.text = PlayerPrefs.GetInt("moveScore", 0).ToString();
        game_moveText.text = "0";

        startUI.SetActive(true);
        gameUI.SetActive(false);
        stopUI.SetActive(false);

        UIEventListener.Get(myButton).onClick = StartButton;
    }

    //按下按钮开始游戏
    private void StartButton(GameObject go)
    {
        startUI.SetActive(false);
        gameUI.SetActive(true);
        myPlayer.GameStart();
    }

    //重新开始
    public void Reset()
    {
        startUI.SetActive(true);
        gameUI.SetActive(false);
    }
}
