using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score, _bestScore;
    //public Text _bestScore;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _intro;
    public int bestScore;
    public int currentscore = 0;
    [SerializeField]
    private Text _retry;
    [SerializeField]
    private GameObject _pauseGame;
    [SerializeField]
    private GameObject _tryAgain;
    [SerializeField]
    private GameObject _quit;
    [SerializeField]
    private Canvas _controls;
 
    //Canvas control = _controller.GetComponent<Canvas>();

    // Start is called before the first frame update
    void Start()
    {
        _livesImage.sprite = _livesSprite[3];
        _score.text = "Score "+ 0;
        _gameOver.gameObject.SetActive(false);
        _intro.gameObject.SetActive(true);
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestScore.text = "Best " + bestScore;
        ScreenText();
#if UNITY_ANDROID
        _retry.alignment = TextAnchor.UpperCenter;
        _controls.enabled = true;
#endif
    }

    public void ScoreUpdate(int scores)
    {
        _score.text = "Score " + scores.ToString();
        currentscore = scores;
        Debug.Log(currentscore);
    }

    public void ScreenText()
    {
#if UNITY_ANDROID
        //_intro.rectTransform.position =_intro.rectTransform.position + new Vector3(0, 70.0f, 0);
        _intro.text = "Use the STICK to Move Around" + "\n" + "Tap on the Right Side of the screen to FIRE"  + "\n" + "Destroy the ASTEROID to begin" ;
        _retry.text = "Loser XD!!";
#endif
    }


    public void CheckforBestScore()
    {
        if(currentscore > bestScore)
        {
            bestScore = currentscore;
            PlayerPrefs.SetInt("HighScore", bestScore);
            _bestScore.text = "Best: " + bestScore;
        }
    }

    public void LivesUpdate(int lives)
    {
        _livesImage.sprite = _livesSprite[lives];
    }

    public void GameOver()
    {
        _gameOver.gameObject.SetActive(true);
#if UNITY_ANDROID
        _tryAgain.SetActive(true);
        _quit.SetActive(true);
        _controls.enabled = false;
#endif

    }

    public void Intro()
    {
        _intro.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        _pauseGame.SetActive(false);

    }

    public void Quit()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }


}
