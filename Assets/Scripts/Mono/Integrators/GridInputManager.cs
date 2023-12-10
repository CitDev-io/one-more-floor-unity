using System.Collections.Generic;
using UnityEngine;

public class GridInputManager : MonoBehaviour
{
    public TileDelegate OnUserStartSelection;
    public NoParamDelegate OnUserEndSelection;
    public TileDelegate OnUserDragIndicatingTile;

    bool Dragging = false;
    List<GameTile> subbedTiles = new List<GameTile>();

    public void AttachTileToGrid(GameTile tile) {
        tile.OnTileClick += HandleTileClick;
        tile.OnTileHoverEnter += HandleTileHoverEnter;
        subbedTiles.Add(tile);
    }

    void OnDestroy() {
        foreach(GameTile tile in subbedTiles) {
            if (tile != null) {
                tile.OnTileClick -= HandleTileClick;
                tile.OnTileHoverEnter -= HandleTileHoverEnter;
            }
        }
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            Dragging = false;
            OnUserEndSelection?.Invoke();
        }
    }

    void HandleTileClick(GameTile tile) {
        Dragging = true;
        OnUserStartSelection?.Invoke(tile._tile);
    }

    void HandleTileHoverEnter(GameTile tile) {
        if (!Dragging) return;

        OnUserDragIndicatingTile?.Invoke(tile._tile);
    }
}
