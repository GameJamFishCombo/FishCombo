using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    None, Basic, Fast, Player 
}

public class Units : MonoBehaviour
{
    public int team;
    public int currX, currY;
    public UnitType type;

    private Vector3 desiredPos, desiredScale;
}
