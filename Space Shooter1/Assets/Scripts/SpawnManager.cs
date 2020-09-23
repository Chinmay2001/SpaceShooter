using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] powerUps;

    private UIManager _uimanager;
    private float _timer = 3.0f;
    private float score;
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        _uimanager = GameObject.Find("Canvas").GetComponent < UIManager >();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        //waits for 5 secs and then the next line is called
        while (_stopSpawning == false)
        {
            Vector3 pos = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0f);
            GameObject newEnemy =  Instantiate(_enemyPrefab, pos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            score = _uimanager.currentscore;
            if(score%100 == 0 && _timer > 1.0f)
            {
                _timer -= 0.1f;
            }

            yield return new WaitForSeconds(_timer);
        }

        //WE WILL NEVER GET OUT OF THE LOOP
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(10.0f);

        while (_stopSpawning == false)
        {

            Vector3 pos = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0f);
            int randomPowerUp = Random.Range(0, 3);
            GameObject powerup = Instantiate(powerUps[randomPowerUp], pos, Quaternion.identity);
            float randomtime = Random.Range(8.0f, 16.0f);
            yield return new WaitForSeconds(randomtime);
        }        
    }
    public void PlayerDied()
    {
        _stopSpawning = true;
    }
}
