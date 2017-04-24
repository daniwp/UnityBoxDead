using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    public GameObject enemy;
    public List<GameObject> spawnPoints;
    public float spawnTime;


	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
    }

    void SpawnEnemy()
    {
        int r = Random.Range(0, spawnPoints.Count);
        Instantiate(enemy, spawnPoints[r].transform.position, spawnPoints[r].transform.rotation);
    }
}
