using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
    public class BoardGameBox : MonoBehaviour
    {
        public GridInputManager Input;
        public List<GameTile> Tiles;

        void Setup() {
            InstantiateTilesToBoard();
        }

        void Awake() {
            Input = GetComponent<GridInputManager>();
            Setup();
        }

        void InstantiateTilesToBoard(int COUNT_ROWS = 6, int COUNT_COLS = 6) {
            GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            for (var rowid = 0; rowid < COUNT_ROWS; rowid++) {
                for (var colid = 0; colid < COUNT_COLS; colid++) {
                    GameObject g = GameObject.Instantiate(
                        tilePrefab,
                        new Vector2(rowid, colid),
                        Quaternion.identity,
                        gameObject.transform
                    );
                    GameTile tile = g.GetComponent<GameTile>();
                    tile.AssignPosition(colid, rowid);
                    Input.AttachTileToGrid(tile);
                    Tiles.Add(tile);
                }
            }
        }
    }
}
