using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using citdev;

namespace citdev {
    public class BoardController : MonoBehaviour
    {
        [SerializeField] GameObject tilePrefab;
        List<GameTile> tiles = new List<GameTile>();
        List<GameTile> selection = new List<GameTile>();
        RoundController _rc;
        LineRenderer _lr;
        GameController_DDOL _gc;
        [SerializeField] Transform SelectionCountDoodad; 

        int TOP_COLUMN_INDEX = 5;
        int COUNT_ROWS = 6;
        int COUNT_COLS = 6;

        bool isDragging = false;
        int draggedTiles = 0;
        bool isFrozen = false;

        public void MinimumSwordRaiseTo(int value)
        {
            foreach (GameTile sword in tiles.Where((o) => o.tileType == TileType.Sword && o.Power < value))
            {
                sword.Power = value;
                sword.label2.text = value + "";
            }
        }

        public void ConvertHeartsToSwords(int count)
        {
            int converted = 0;
            foreach(GameTile heart in tiles.Where((o) => o.tileType == TileType.Heart))
            {
                if (converted < count)
                {
                    heart.SetTileType(TileType.Sword);
                    _rc.ConvertTileToSword(heart);
                    converted += 1;
                }
            }
        }

        public void RaiseCoinValueTo(int value)
        {
            foreach (GameTile coin in tiles.Where((o) => o.tileType == TileType.Coin))
            {
                coin.Power = value;
            }
        }

        public void ToggleTileFreeze(bool freeze)
        {
            ClearSelection();
            isFrozen = freeze;
        }

        public void EnemyIconsTaunt()
        {
            foreach(GameTile monster in tiles.Where((o) => o.tileType == TileType.Monster)) {
                monster.MonsterMenace();
            }
                
        }

        private void Update()
        {
            if (isFrozen) return;
           if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isDragging = false;
                if (draggedTiles > 0)
                {
                    OnPlayerSolveAttemptComplete();
                } else
                {
                    ClearSelection();
                }
            }
        }

        private void Awake()
        {
            _rc = GameObject.FindObjectOfType<RoundController>();
            _lr = gameObject.GetComponent<LineRenderer>();
            _rc.OnRoundEnd += OnRoundEnd;
        }

        private void OnDestroy()
        {
            if (_rc != null)
                _rc.OnRoundEnd -= OnRoundEnd;
        }

        void OnRoundEnd()
        {
            ToggleTileFreeze(true);
        }

        void StartGame()
        {
            for (var rowid = 0; rowid < COUNT_ROWS; rowid++)
            {
                for (var colid = 0; colid < COUNT_COLS; colid++)
                {
                    GameObject g = GameObject.Instantiate(
                        tilePrefab,
                        new Vector2(rowid, colid),
                        Quaternion.identity
                    );
                    GameTile tile = g.GetComponent<GameTile>();
                    _rc.RecycleTileForPosition(tile, new Vector2(colid, rowid));
                    tiles.Add(tile);
                }
            }
        }

        void CollectTiles(List<GameTile> collected)
        {
            _rc.PlayerCollectedTiles(collected, this);
        }

        void OnPlayerSolveAttemptComplete()
        {
            if (isSelectionFinishable())
            {
                CollectTiles(selection);
                ClearSelection();
            } else {
                ClearSelection();
            }
        }

        bool isSelectionFinishable()
        {
            if (selection.Count < 3) return false;

            if (selection.Any(o => o.tileType == TileType.Monster)
            && !selection.Any(o => o.tileType == TileType.Sword)) {
                return false;
            }
            
            return true;
        }

        void ClearSelection()
        {
            foreach(GameTile t in selection)
            {
                t.ToggleHighlight(false);
            }
            selection.Clear();
            OnSelectionChange();
        }

        void OnSelectionChange()
        {
            _lr.positionCount = selection.Count;
            _lr.SetPositions(
                selection.Select((o) => o.transform.position).ToArray()
            );
            if (selection.Count == 0) {
                SelectionCountDoodad.gameObject.SetActive(false);
            } else {
                SelectionCountDoodad.gameObject.SetActive(true);
                SelectionCountDoodad.position = selection.ElementAt(selection.Count - 1).gameObject.transform.position;
                SelectionCountDoodad.GetComponent<DOODAD_SelectionCount>().SetText(selection.Count + "");               
            }
        }

        public List<GameTile> GetMonsters()
        {
            return tiles.Where((o) => o.tileType == TileType.Monster).ToList();
        }

        public void ClearTiles(List<GameTile> clearedTiles)
        {
            clearedTiles.OrderByDescending(o => o.row);
            foreach(GameTile t in clearedTiles)
            {
                RecascadeTile(t);
            }
        }

        void RecascadeTile(GameTile tile)
        {
            List<GameTile> aboveTiles = tiles.FindAll((o) => o.col == tile.col && o.row > tile.row);

            foreach(GameTile t in aboveTiles)
            {
                t.row -= 1;
            }

            _rc.RecycleTileForPosition(tile, new Vector2(tile.col, TOP_COLUMN_INDEX));
        }

        public void OnTileDragOver(GameTile tile)
        {
            if (isFrozen) return;

            if (!isDragging) return;
            draggedTiles += 1;
            DoUserIndicatingTile(tile);
        }

        public void OnTileClick(GameTile tile)
        {
            if (isFrozen) return;

            isDragging = true;
            draggedTiles = 0;
            if (!isEligibleToAddToSelection(tile) && !selection.Contains(tile))
            {
                ClearSelection();
            }
            DoUserIndicatingTile(tile);
        }
        
        void HandleUserReindicatingTile(GameTile tile)
        {
            bool endingManualClicks =
                isSelectionFinishable()
                && draggedTiles == 0
                && doesTileMatchLastInSelection(tile);
            if (endingManualClicks)
            {
                OnPlayerSolveAttemptComplete();
                return;
            }
            int index = selection.IndexOf(tile);

            bool reclickingLast = (index + 1 == selection.Count);
            if (reclickingLast)
            {
                return;
            }

            List<GameTile> tilesToUnhighlight = selection.GetRange(index + 1, selection.Count - index - 1);
            foreach (GameTile t in tilesToUnhighlight)
            {
                selection.Remove(t);
                OnSelectionChange();
            }
        }

        void AddTileToSelectionIfEligible(GameTile tile)
        {
            if (isEligibleToAddToSelection(tile))
            {
                selection.Add(tile);

                switch (tile.tileType) {
                    case TileType.Coin:
                        _gc.PlaySound("Coin_Select");
                        break;
                    case TileType.Heart:
                        _gc.PlaySound("Heart_Select");
                        break;
                    case TileType.Shield:
                        _gc.PlaySound("Shield_Select");
                        break;

                    default:
                        _gc.PlaySound("Sword_Select");
                        break;
                }

                OnSelectionChange();
            }
        }

        void DoUserIndicatingTile(GameTile tile)
        {
            bool AlreadySelected = selection.Contains(tile);

            if (AlreadySelected)
            {
                HandleUserReindicatingTile(tile);
                return;
            }

            AddTileToSelectionIfEligible(tile);
        }

        bool isEligibleToAddToSelection(GameTile tile)
        {
            if (selection.Count == 0)
            {
                return true;
            }

            GameTile lastTile = selection.ElementAt(selection.Count - 1);
            if (isChainable(tile, lastTile))
            {
                if (isAdjacent(tile, lastTile)) {
                    return true;
                }

                if (hasDirectPath(tile, lastTile)) {
                    List<GameTile> tilesInBetween = GetInBetweenTiles(tile, lastTile);
                    tilesInBetween.Reverse();
                    if (areAssumedChainable(tile, tilesInBetween)) {
                        selection.AddRange(tilesInBetween);
                        return true;
                    }
                }
            }

            return false;
        }

        List<GameTile> GetInBetweenTiles(GameTile first, GameTile next) {
            if (!hasDirectPath(first, next)) throw new System.ArgumentException("should never have called this");
    
            Vector2Int diff = GetTileDiff(first, next);


            Vector2Int normalized = new Vector2Int(System.Math.Sign(diff.x), System.Math.Sign(diff.y));

            Vector2Int stopPoint = new Vector2Int(next.col, next.row);
            Vector2Int pointer = new Vector2Int(first.col, first.row) + normalized;
            List<GameTile> betweeners = new List<GameTile>();
            int i = 0;
            while (pointer != stopPoint && i < 6) {
                var checkTile = tiles.FirstOrDefault(o => o.col == pointer.x && o.row == pointer.y);
                betweeners.Add(checkTile);
                pointer += normalized;
                i += 1; // because i'm scared
            }

            return betweeners;
        }

        bool hasDirectPath(GameTile first, GameTile next) {
            Vector2Int diff = GetTileDiff(first, next);

            if (diff.x == 0 || diff.y == 0) return true;
            if (Mathf.Abs(diff.x) == Mathf.Abs(diff.y)) return true;

            return false;
        }


        bool areAssumedChainable(GameTile first, List<GameTile> betweeners) {
            return betweeners.All(o => isChainable(first, o) && !selection.Contains(o));
        }

        bool isChainable(GameTile first, GameTile next)
        {
            TileType[] attackChainTiles = new TileType[] { TileType.Sword, TileType.Monster };
            if (first.tileType == next.tileType) return true;

            if (
                attackChainTiles.Contains(first.tileType)
                && attackChainTiles.Contains(next.tileType)
            ) {
                return true;
            }

            return false;
        }

        bool isAdjacent(GameTile tile1, GameTile tile2)
        {
            Vector2Int diff = GetAbsTileDiff(tile1, tile2);
            return (diff.y == 1 || diff.x == 1) && diff.x + diff.y <= 2;
        }

        Vector2Int GetAbsTileDiff(GameTile first, GameTile next) {
            return new Vector2Int(Mathf.Abs(next.col - first.col), Mathf.Abs(next.row - first.row));
        }

        Vector2Int GetTileDiff(GameTile first, GameTile next) {
            return new Vector2Int(next.col - first.col, next.row - first.row);
        }

        bool doesTileMatchLastInSelection(GameTile tile)
        {
            if (selection.Count == 0) return false;

            var lastTile = selection.ElementAt(selection.Count - 1);

            return lastTile == tile;
        }

        void Start()
        {
            _gc = FindObjectOfType<GameController_DDOL>();
            StartCoroutine(StartAfterDelay());
        }

        IEnumerator StartAfterDelay()
        {
            yield return new WaitForSeconds(0.4f);
            StartGame();
        }
    }

}
