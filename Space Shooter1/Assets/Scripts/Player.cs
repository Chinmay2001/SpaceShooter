using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using EZCameraShake;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private float _speedmultiplier = 2f;
 
    public GameObject laserPrefab;
    public GameObject tripleshotPrefab;
    public GameObject shieldPrefab;
    public GameObject[] wingdamage;

    [SerializeField]
    private float _FireRate = 2.0f;
    private float _canFire = 0f;
    [SerializeField]
    private int _lives = 3;
    //[SerializeField]
    private bool _tripleshot = false;

    private bool _speedup = false;

    private bool _shield = false;

    private SpawnManager _spawn;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    private GameManager _gameManager;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _playerExplosion;
    void Start()
    {
        transform.position = new Vector3(0f, 0f, 0f);

        _spawn = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_spawn == null)
        {
            Debug.LogError("Kuch Toh Gadbad Hai.");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UIManager Gadbad");
        }
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager Error");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio SOurce Error");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    void Update()
    {
        PlayerMovement();

#if UNITY_ANDROID
        if(CrossPlatformInputManager.GetButton("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#else
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire) // if(event.key == ' ')
        {
            FireLaser();
        }
#endif

        //if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        //{
        //    FireLaser();
        //}
    }

    void PlayerMovement()
    {
#if UNITY_ANDROID
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

#else
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

#endif
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime);
        
        float x = transform.position.x;
        float y = transform.position.y;
        
        transform.position = new Vector3(x, Mathf.Clamp(y, -4.5f, 0f), 0f);

        if (x >= 11f) //(JetPlane.getX() >= 11)
        {
            transform.position = new Vector3(-11f, y, 0f); //JetPlane.setX(-11)
        }
        if (x <= -11f)
        {
            transform.position = new Vector3(11f, y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _FireRate;

        if(_tripleshot == true)
        {
            Instantiate(tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0f, 1.0f, 0f), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void TripleShotActive()
    {
        _tripleshot = true;
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {
        while(_tripleshot == true)
        {
            yield return new WaitForSeconds(6.0f);
            _tripleshot = false;
        }
    }

    public void SpeedUpActive()
    {
        _speedup = true;
        speed *= _speedmultiplier;
        StartCoroutine(SpeedUpCoolDown());
    }

    IEnumerator SpeedUpCoolDown()
    {
        while(_speedup == true)
        {
            yield return new WaitForSeconds(5.0f);
            speed /= _speedmultiplier;
            _speedup = false;
        }
    }

    public void ShieldActive()
    {
        _shield = true;
        shieldPrefab.SetActive(true);
        
    }



    public void Damage()
    {
        if (_shield == true)
        {
            _shield = false;
            shieldPrefab.SetActive(false);
            return;
        }

        _lives--;
        if(_lives == 2)
        {
            wingdamage[0].SetActive(true);
        }
        else if(_lives == 1)
        {
            wingdamage[1].SetActive(true);
        }
        _uiManager.LivesUpdate(_lives);

        if(_lives < 1)
        {
            _spawn.PlayerDied();
            _uiManager.GameOver();
            _gameManager.GameOver();
            _uiManager.CheckforBestScore();
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            Instantiate(_playerExplosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
    }

    public void ScoreAddition(int point)
    {
        _score += point;
        _uiManager.ScoreUpdate(_score);
    }
}
