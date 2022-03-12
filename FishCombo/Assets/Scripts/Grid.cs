using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Art Assets")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;

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

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover"))) {
            //get the indexes of the tile i've hit
            Vector2Int hitPos = LookupTileIndex(info.transform.gameObject);

            if(currHover == -Vector2Int.one) {
                currHover = hitPos;
                tiles[hitPos.x, hitPos.y].layer = LayerMask.NameToLayer("Hover");
            }

            if(currHover != hitPos) {
                tiles[currHover.x, currHover.y].layer = LayerMask.NameToLayer("Tile");
                currHover = hitPos;
                tiles[hitPos.x, hitPos.y].layer = LayerMask.NameToLayer("Hover");
            }
        } else {
            if(currHover != -Vector2Int.one) {
                tiles[currHover.x, currHover.y].layer = LayerMask.NameToLayer("Tile");
                currHover = -Vector2Int.one;
            }
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
        GameObject tileObj = new GameObject(string.Format("X:{0} Y:{1}", x, y));
        tileObj.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObj.AddComponent<MeshFilter>().mesh = mesh;
        tileObj.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y+1) * tileSize);
        vertices[2] = new Vector3((x+1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x+1) * tileSize, 0, (y+1) * tileSize);

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObj.layer = LayerMask.NameToLayer("Tile");
        tileObj.AddComponent<BoxCollider>();

        return tileObj;
    }

    //Spawn player
    private void SpawnAllPieces() {
        unitPiece = new Units[TILE_COUNT_X, TITLE_COUNT_Y];
        unitPiece[0,0] = SpawnSinglePiece(UnitType.Player, 0);
        unitPiece[7,3] = SpawnSinglePiece(UnitType.Basic, 1);
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
                    PositionSinglePieces(x,y,true);
    }
    private void PositionSinglePieces (int x, int y, bool force) {
        force = false;
        unitPiece[x,y].currX = x;
        unitPiece[x,y].currY = y;
        unitPiece[x,y].transform.position = GetTileCenter(x, y);

        
    }

    private Vector3 GetTileCenter(int x, int y) {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }


    private Vector2Int LookupTileIndex(GameObject hitInfo) {
        for (int x = 0; x < TILE_COUNT_X; x++) { 
            for (int y = 0; y < TITLE_COUNT_Y; y++) {
                if (tiles[x,y] == hitInfo) {return new Vector2Int(x,y);}
            }
        }

        return -Vector2Int.one; //-1 -1; Invalid
    }
}
