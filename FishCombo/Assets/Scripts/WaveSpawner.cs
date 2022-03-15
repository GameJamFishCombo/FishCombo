using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : Wave
{
    public enum SpawnState {Spawning, Waiting, Counting}
    static int waveNum = 1;
    [SerializeField] public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.Counting;
    public Grid grid;


    void Start() {
        StartCoroutine(SetWaveNumber());

        waveCountdown = timeBetweenWaves;
    }

    void Update() {
        if(state == SpawnState.Waiting) {
            if(!EnemyIsAlive()) {WaveCompleted();}
        } else {
            return;
        }
    }

    IEnumerator SetWaveNumber(){
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    void WaveCompleted() {
        Debug.Log("Wave completed!");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        //if the next wave is out of bounds of array
        //basically, final wave was completed, trigger cutscene
        if(nextWave + 1 > waves.Length - 1) {
            //idk something
        } else {
            nextWave++;
        }
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f) {
            searchCountdown = 1f;

            if(GameObject.FindGameObjectWithTag("Enemy") == null) { return false; }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave) {
        if(wave.enemy.Length != wave.Enemies.Length) {
            Debug.LogError("Unable to Spawn Wave: " + wave.name + ". Enemy types array and enemy count array sizes do not match.");
        } else if(wave.enemy.Length == wave.Enemies.Length) {
            Debug.Log("Spawning Wave: " + wave.name);
            state = SpawnState.Spawning;

            for(int i = 0; i < wave.enemy.Length; i++) {
                for(int j = 0; j < wave.Enemies[j]; j++) {
                    grid.SpawnEnemy(wave.enemy[j]);
                    yield return new WaitForSeconds(1f / wave.rate);
                }
            }

            Debug.Log("Done Spawning");
            state = SpawnState.Waiting;
            yield break;
        }
    }
}
