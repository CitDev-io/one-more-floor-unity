using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using citdev;

namespace citdev {
    
    public class BoardController
    {
        public TilesDelegate OnPlayerCollectedTiles;
        public TileDelegate OnTileAddedToSelection;
        public TilesDelegate OnSelectionChange;
        public TilesDelegate OnMonstersAttack;

        List<GameTile> Tiles;

        int EnemyHp = 1;
        int EnemyDmg = 1;

        List<GameTile> selection = new List<GameTile>();
        TileSelector tileSelector;
        ChainValidator chainValidator;

        public BoardController(
            List<GameTile> tiles,
            GridInputManager input,
            int enemyHp,
            int enemyDmg) {
            Tiles = tiles;
            EnemyHp = enemyHp;
            EnemyDmg = enemyDmg;
            input.OnUserStartSelection += HandleUserStartSelection;
            input.OnUserEndSelection += HandleUserEndSelection;
            input.OnUserDragIndicatingTile += HandleUserIndicatingTile;
            tileSelector = new TileSelector();
            chainValidator = new ChainValidator(Tiles, selection);
        }

        public void RunGrid() {
            foreach(GameTile tile in Tiles) {
                tile.AssignPosition(tile.col, tile.row);
                tile.RecycleAsType(tileSelector.GetNextTile());
            }
        }


        /*

            private methods


        */


        void HandleUserStartSelection(GameTile tile)
        {
            ClearSelection();
            DoUserIndicatingTile(tile);
        }

        void HandleUserEndSelection()
        {
            if (chainValidator.isSelectionFinishable())
            {
                ExecuteUserTurn();
            }

            ClearSelection();
        }

        void ExecuteUserTurn() {
            CollectTiles(selection);
            OnMonstersAttack?.Invoke(
                Tiles.Where((o) => o.tileType == TileType.Monster && o.TurnsAlive > 0).ToList()
            );
            AgeAllMonsters();
        }

        void AgeAllMonsters() {
            var monsters = Tiles.Where((o) => o.tileType == TileType.Monster);
            foreach(GameTile monster in monsters) {
                monster.TurnsAlive += 1;
            }
        }

        void HandleUserIndicatingTile(GameTile tile)
        {
            DoUserIndicatingTile(tile);
        }

        void ClearSelection()
        {
            selection.Clear();
            OnSelectionChange?.Invoke(selection);
        }

        void DoUserIndicatingTile(GameTile tile)
        {
            bool AlreadySelected = selection.Contains(tile);

            if (AlreadySelected)
            {
                HandleUserReindicatingTile(tile);
            } else {
                AddTileToSelectionIfEligible(tile);
            }
        }

        void AddTileToSelectionIfEligible(GameTile tile)
        {
            if (chainValidator.isEligibleToAddToSelection(tile))
            {
                selection.Add(tile);
                OnTileAddedToSelection?.Invoke(tile);
                OnSelectionChange?.Invoke(selection);
            }
        }

        void HandleUserReindicatingTile(GameTile tile)
        {
            int index = selection.IndexOf(tile);
            List<GameTile> tilesToUnhighlight = selection.GetRange(index + 1, selection.Count - index - 1);
            foreach (GameTile t in tilesToUnhighlight)
            {
                selection.Remove(t);
                OnSelectionChange?.Invoke(selection);
            }
        }




        void CollectTiles(List<GameTile> collected)
        {
            OnPlayerCollectedTiles?.Invoke(collected);
            ClearTiles(collected);
        }

        void ClearTiles(List<GameTile> clearedTiles)
        {
            clearedTiles.OrderByDescending(o => o.row);
            foreach(GameTile t in clearedTiles)
            {
                if (t.tileType == TileType.Monster && t.HitPoints > 0) {
                    continue;
                }
                RecascadeTile(t);
            }
        }

        void RecascadeTile(GameTile tile)
        {
            List<GameTile> aboveTiles = Tiles.FindAll((o) => o.col == tile.col && o.row > tile.row);

            foreach(GameTile t in aboveTiles)
            {
                t.row -= 1;
            }

            tile.RecycleAsType(tileSelector.GetNextTile(), true);
        }
    }

}
