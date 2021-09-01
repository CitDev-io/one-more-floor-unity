using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace citdev {
    
    public class BoardController
    {
        public TilesDelegate OnPlayerCollectedTiles;
        public TileDelegate OnTileAddedToSelection;
        public TilesDelegate OnSelectionChange;
        public TilesDelegate OnMonstersAttack;

        BoardContext ctx;

        int EnemyHp = 1;
        int EnemyDmg = 1;

        List<GameTile> selection = new List<GameTile>();
        TileSelector tileSelector;
        ChainValidator chainValidator;

        public BoardController(BoardContext bctx) {
            bctx.Box.Input.OnUserStartSelection += HandleUserStartSelection;
            bctx.Box.Input.OnUserEndSelection += HandleUserEndSelection;
            bctx.Box.Input.OnUserDragIndicatingTile += HandleUserIndicatingTile;
            ctx = bctx;
            SetEnemyStatsByRound(bctx.Stage);
            tileSelector = new TileSelector(bctx.PC.TileOptions);
            switch(bctx.PC.Name.ToLower()) {
                case "warrior":
                    chainValidator = new WarriorChainValidator(
                        bctx.Box.Tiles,
                        selection
                    );
                    break;
                case "rogue":
                    chainValidator = new RogueChainValidator(
                        bctx.Box.Tiles,
                        selection
                    );
                    break;
                default:
                    Debug.Log("NOT ESTABLISHED WHAT CLASS YOU ARE");
                    break;
            }
        }

        void SetEnemyStatsByRound(int round)
        {

            EnemyHp = Mathf.Min((int) Mathf.Ceil(round / 3f) + 1, 4);
            EnemyDmg = Mathf.Min((int) Mathf.Ceil(round / 4f), 3);
        }

        public void StartBoard() {
            foreach(GameTile tile in ctx.Box.Tiles) {
                tile.AssignPosition(tile.col, tile.row);
                tile.MaxHitPoints = EnemyHp;
                tile.HitPoints = EnemyHp;
                tile.Damage = EnemyDmg;
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
                ctx.Box.Tiles.Where((o) => o.tileType == TileType.Monster
                && o.TurnsAlive > 0
                && o.StunnedRounds <= 0
            ).ToList()
            );
            AgeAllMonsters();
        }

        void AgeAllMonsters() {
            var monsters = ctx.Box.Tiles.Where((o) => o.tileType == TileType.Monster);
            foreach(GameTile monster in monsters) {
                monster.TurnsAlive += 1;
                monster.ResolveStunRound();
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
            List<GameTile> aboveTiles = ctx.Box.Tiles.FindAll((o) => o.col == tile.col && o.row > tile.row);

            foreach(GameTile t in aboveTiles)
            {
                t.row -= 1;
            }

            tile.RecycleAsType(tileSelector.GetNextTile(), true);
        }
    }

}
