using System.Collections.Generic;
using UnityEngine;

public delegate void ActorTilesDelegate(List<GameTile> t);

public partial class GameBridge : MonoBehaviour
{
    [SerializeField] Transform SelectionCountDoodad;
    LineRenderer _lr;

    GameController_DDOL _gc;
    GridInputManager _gim;
    List<GameTile> GameTiles = new List<GameTile>();
    public TileGame Board;

    
    // [SerializeField] GameObject EnchantmentShopMenu;

    // Make a subclass or partial class = ShoppingCart
    // it has purchasing flags and result signatures


    // public void SelectionEnchantmentShopOptionAtIndex(int index) {
    //     Debug.Log("ENCHANTMENT");
    //     Board.EnchantmentShopPurchase(index);
    // }

    // public void SelectionXPShopOptionsAtIndexes(List<int> indexes) {
    //     Debug.Log("XP STUFF");
    //     Board.LevelUpPurchase(indexes);abcdefghijklmnopqrstuvwxyz
    // }

    void Awake() {
        _lr = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        AssignNewStateObjects();

        WireUpTiles();

        OnStart_EventsAndAnimation();
    }

    void OnDestroy() {
        if (Board == null) return;

        OnDestroy_EventsAndAnimation();
    }

    void AssignNewStateObjects() {
        _gc = FindObjectOfType<GameController_DDOL>();
        _gim = gameObject.GetComponent<GridInputManager>();
        Board = new TileGame();
    }

    void WireUpTiles() {
        foreach (Tile tile in Board.State.Tiles.TileList) {
            GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            GameObject go = Instantiate(
                tilePrefab,
                tile.GridPosition(),
                Quaternion.identity
            );
            GameTile gt = go.GetComponent<GameTile>();
            gt.AttachToTile(tile);
            _gim.AttachTileToGrid(gt);
            GameTiles.Add(gt);
        }
    }
}
