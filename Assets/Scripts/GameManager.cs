using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public TMP_Text playerScoreText;
    public TMP_Text playerLivesText;
    [SerializeField] GameObject win;
    [SerializeField] GameObject lose;
    [SerializeField] GameObject img;
    [SerializeField] GameObject pause;
    int score = 0;
    public int lives = 3;

    private void Start()
    {
        playerScoreText.text = "Score: " + score.ToString();
        playerLivesText.text = "Lives :" + lives.ToString();
    }
    private void Update()
    {
        
        if (score == 9)
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            img.SetActive(true);
            win.SetActive(true);
            lose.SetActive(false);
        }else if(lives < 1)
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            img.SetActive(true);
            win.SetActive(false);
            lose.SetActive(true);
        }
    }
    public void IncreaseScore()
    {
        score++;
        playerScoreText.text = "Score: " + score.ToString();
    }

    void PlayerLives()
    {
        lives--;
        playerLivesText.text = "Lives :" + lives.ToString();
    }

    public void ResurrectPlayer()
    {
        if (lives > 0)
        {
            PlayerLives();

        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level_1");
        Time.timeScale = 1f;
        img.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
