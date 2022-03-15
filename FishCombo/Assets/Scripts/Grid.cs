using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Art Assets")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private GameObject newTile;

    [Header("Prefabs & Mats")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMats;

    private const int TILE_COUNT_X = 8;
    private const int TITLE_COUNT_Y = 4;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currHover;
    private Units[,] unitPiece;
    private Vector3 bounds;

    private void Awake() {
        GenerateAllTiles(tileSize, TILE_COUNT_X, TITLE_COUNT_Y);
        SpawnAllPieces();
        PositionAllPieces();
    }

    public void Update() {
        if(!currentCamera) {
            currentCamera = Camera.main;
            return;
        }

    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY) {
        tiles = new GameObject[tileCountX, tileCountY];
        
        for (int x = 0; x < tileCountX; x++) { 
            for (int y = 0; y < tileCountY; y++) {
                tiles[x,y] = GenerateSingleTile(tileSize, x, y);
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y) {
        Vector3 location = new Vector3(x,0,y);
        GameObject defaultTile = Instantiate(newTile, location, Quaternion.identity);

        defaultTile.name = string.Format("X:{0} Y:{1}", x, y);
        defaultTile.layer = LayerMask.NameToLayer("Tile");

        return defaultTile;
    }

    //Spawn player
    private void SpawnAllPieces() {
        unitPiece = new Units[TILE_COUNT_X, TITLE_COUNT_Y];
        unitPiece[0,0] = SpawnSinglePiece(UnitType.Player, 0);
    }

    private Units SpawnSinglePiece(UnitType type, int team) {
        Units u = Instantiate(prefabs[(int)type - 1], transform).GetComponent<Units>();
        u.type = type;
        u.team = team;
        u.GetComponent<MeshRenderer>().material = teamMats[team];

        return u;
    }

    private void PositionAllPieces() {
        for (int x = 0; x < TILE_COUNT_X; x++) 
            for (int y = 0; y < TITLE_COUNT_Y; y++)
                if(unitPiece[x,y] != null)
                    PositionSinglePieces(x,y);
    }
    private void PositionSinglePieces (int x, int y) {
        unitPiece[x,y].currX = x;
        unitPiece[x,y].currY = y;
        unitPiece[x,y].transform.position = GetTileCenter(x, y);

        
    }

    private Vector3 GetTileCenter(int x, int y) {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize, 0, tileSize);
    }


    private Vector2Int LookupTileIndex(GameObject hitInfo) {
        for (int x = 0; x < TILE_COUNT_X; x++) { 
            for (int y = 0; y < TITLE_COUNT_Y; y++) {
                if (tiles[x,y] == hitInfo) {return new Vector2Int(x,y);}
            }
        }

        return -Vector2Int.one;
    }

    public void SpawnEnemy(Transform enemy) {
        unitPiece = new Units[TILE_COUNT_X, TITLE_COUNT_Y];
        int randX = 0, randZ = 0;
        randX = (int)Random.Range(3,6);
        randZ = (int)Random.Range(0,3);

        unitPiece[randX, randZ] = SpawnSinglePiece(UnitType.Basic, 1);
        PositionAllPieces();

        Debug.Log("Spawning Enemy: " + enemy.name);
    }

    public bool inBounds(Vector3 vec, string tag) {
        if(tag == "Player") {
            if(vec.x < 0 || vec.x > 3 || vec.z < 0  || vec.z > 3) {
                return true;
            }

            return false;

        } else if (tag == "Enemy") {
            if(vec.x < 4 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
                return true;
            }

            return false;
        } else { //these are projectiles
            if(vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
                return true;
            }

            return false;
        }

        return false;
    }
}
