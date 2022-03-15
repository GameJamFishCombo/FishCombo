using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : Wave
{
    public enum SpawnState {Spawning, Waiting, Counting}
    static int waveNum = 1;
    public Wave[] waves;
    

}
