using System.Collections.Generic;
using System.Linq;

public class ChainBuilder : IChainBuilder {
    private IEventInvoker EventInvoker;
    public readonly List<Tile> CurrentChain = new ();
    ChainValidator chainValidator;

    public ChainBuilder(ITileGridProvider tileGridProvider, IEventInvoker events) {
        EventInvoker = events;
        chainValidator = new StandardChainValidator(
            tileGridProvider,
            CurrentChain
        );
    }

    public void StartChain(Tile tile) {
        ClearSelection();
        DoUserIndicatingTile(tile);
    }

    public void EndChain() {
        if (!chainValidator.isChainFinishable())
        {
            ClearSelection();
            return;
        }

        EventInvoker.Invoke(
            new ChainAcceptedEvent(CurrentChain.ToList())
        );
        ClearSelection();
    }

    public void IndicateTile(Tile tile) {
        DoUserIndicatingTile(tile);
    }

    void ClearSelection()
    {
        CurrentChain.Clear();
        ChainUpdated();
    }

    void DoUserIndicatingTile(Tile tile)
    {   
        bool AlreadyInChain = CurrentChain.Contains(tile);

        if (AlreadyInChain)
        {
            HandleUserReindicatingTile(tile);
        } else {
            AddTileToChainIfEligible(tile);
        }
    }

    void HandleUserReindicatingTile(Tile tile)
    {
        int index = CurrentChain.IndexOf(tile);
        List<Tile> tilesToUnchain = CurrentChain.GetRange(index + 1, CurrentChain.Count - index - 1);
        foreach (Tile t in tilesToUnchain)
        {
            CurrentChain.Remove(t);
            ChainUpdated();
        }
    }

    void AddTileToChainIfEligible(Tile tile)
    {
        if (chainValidator.isEligibleToAddToChain(tile))
        {
            CurrentChain.Add(tile);
            
            
            // foreach(Tile t in CurrentChain) { t.SetSelectedDmg(1); } // this line is sus af

            EventInvoker.Invoke(
                new TileAddedToSelectionEvent(tile)
            );
            ChainUpdated();
        }
    }

    void ChainUpdated() {
        // var dmgSelected = 0;
        // if (chainValidator.isChainFinishable()) {
        //     dmgSelected = 5;// TileGridProvider.Tiles.TileList._encounter._player.CalcDamageDone(CurrentChain.Where((t) => t.tileType == TileType.Sword).Count()); // not sure this is the right place for this. what would be responsible for looking at the current chain and assessing potential power?? its unnecessary in the game logic, but useful in the UI only.
            // event system can chirp out sword count highlighted and let the UI do the math
        // }
        // foreach(Tile t in TileGridProvider.Tiles.TileList) {
        //     t.SetSelectedDmg(5);//CurrentChain.Contains(t) ? dmgSelected : 0); // this line is sus af
        // }
        EventInvoker.Invoke(
            new SelectionChangeEvent(CurrentChain)
        );
    }
}