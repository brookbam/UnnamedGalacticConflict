using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSD : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    //[SerializeField] AudioClip startSound;
    //[SerializeField] [Range(0, 1)] float startSoundVolume = 0.2f;
    [SerializeField] float timeBetweenWaves = 2f;

    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        do
        {
            yield return StartCoroutine(WaitForSDTime());
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
    }


    private IEnumerator WaitForSDTime()
    {
        while (!enemySpawner.GetSDTime())
        {
            yield return null;
        }
            
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathingSD>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
