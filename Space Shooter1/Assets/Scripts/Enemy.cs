using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private Player _player;
    private Animator _animator;
    private BoxCollider2D collide;
    [SerializeField]
    private AudioClip _enemyExplosion;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyFireprefab;
    private float _firerate = 3.0f;
    private float _canfire = -1.0f;
    private bool _isenemyhit = false;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }
        collide = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource Error");
        }
        else
        {
            _audioSource.clip = _enemyExplosion;
        }
    }
    // Update is called once per frame
    void Update()
    {
        EnemyMovement();

        if(_isenemyhit == false)
        {
            if (Time.time > _canfire)
            {
                _firerate = Random.Range(3.0f, 7.0f);
                _canfire = Time.time + _firerate;
                GameObject enemylaser = Instantiate(_enemyFireprefab, transform.position, Quaternion.identity);
                LaserBehaviour[] lasers = enemylaser.GetComponentsInChildren<LaserBehaviour>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
                //Debug.Break();
            }
        }
        
    }

    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        _isenemyhit = true;
        if(other.tag == "Laser")
        {

            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.ScoreAddition(10);

            }
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
           
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            speed /= 2;
            collide.enabled = false;
            Destroy(this.gameObject, 4.2f);
        }
        if(other.tag == "Player") 
        {
           
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            speed /= 2;
            collide.enabled = false;
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            Destroy(this.gameObject, 4.2f);

            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            //other.transform.GetComponent<Player>().Damage();
            //other.GetComponent<Player>().Damage();
        }
    }
}
