using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    public GameObject enemy;
    public GameObject boss;
    public List<GameObject> spawnPoints;
    public GameObject spawnPointBoss;
    public float spawnTime;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
        InvokeRepeating("SpawnBoss", 5, 30);
    }

    // Update is called once per frame
    void Update () {

    }

    void SpawnEnemy()
    {
        int r = Random.Range(0, spawnPoints.Count);
        Instantiate(enemy, spawnPoints[r].transform.position, spawnPoints[r].transform.rotation);
    }

    void SpawnBoss()
    {
        Instantiate(boss, spawnPointBoss.transform.position, spawnPointBoss.transform.rotation);
    }
}
