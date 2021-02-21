using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    // スコア関連
    public Text scoreText;
    private int score;
    public int currentScore;
    public int clearScore = 1000;

    // タイマー関連
    public Text timerText;
    public float gameTime = 180.0f;
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    void Update()
    {
        TimeManagement();
    }

    private void TimeManagement()
    {
        gameTime -= Time.deltaTime;
        seconds = (int)gameTime;
        timerText.text = seconds.ToString();

        if(seconds == 0)
        {
            GameOver();
        }
    }

    // ゲーム開始前の状態に戻す
    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;
    }

    // スコアの追加
    public void AddScore()
    {
        score += 100;
        currentScore += score;
        scoreText.text = "Score: " + currentScore.ToString();
        if(currentScore >= clearScore)
        {
            GameClear();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameClear()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
