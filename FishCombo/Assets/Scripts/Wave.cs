using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave : MonoBehaviour
{
    public string name;
    [Header("Enemy Prefabs to spawn")]
    public Transform[] enemy;
    [Header("Number of enemies per type")]
    public int[] Enemies;
    public float rate;


}
