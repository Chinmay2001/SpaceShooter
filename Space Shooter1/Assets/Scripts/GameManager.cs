using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _creditPanel;



    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID

        //if (Input.GetMouseButtonDown(0) && _isGameOver == true)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
#else
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown(KeyCode.Q) && _isGameOver == true)
        {
            _creditPanel.SetActive(true);
            
        }
        if(Input.GetKeyDown(KeyCode.Escape) && _isGameOver == false)
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
#endif
        //if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Credits()
    {
        _creditPanel.SetActive(true);
    }

}
