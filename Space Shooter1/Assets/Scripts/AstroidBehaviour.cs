using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using EZCameraShake;

public class AstroidBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private UIManager _uIManager;


    public void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager Error");
        }
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            _uIManager.Intro();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
