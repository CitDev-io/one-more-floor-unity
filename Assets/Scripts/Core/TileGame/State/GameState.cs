using System;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow.Items;

public interface IItemShopVendor {
    public List<PlayerItem> GetNewItemShopOptions();
    public ValueTuple<PlayerItem, PlayerItem> ItemShopPurchase(int index);
}

public class GameState : ITileGridProvider, ITileCollectorContext, IEventInvoker, IItemShopVendor {
    public PlayerAvatar Player;
    public TileGrid Tiles { get; private set; }
    public BoardPhase Phase { get; private set; } = BoardPhase.READY;
    public int MovesMade { get; set; } = 0;
    public int TilesCleared { get; set; } = 0;
    public bool PlayersTurn = true;
    public Encounter Encounter;
    private readonly IEventInvoker SurfaceEventInvoker;
    public List<PlayerItem> ItemShopOptions { get; private set; }
    public IItemShopVendor ItemShopVendor => this;

    public List<PlayerItem> GetNewItemShopOptions() {
        ItemShopOptions = PlayerItem.GetRandomItemsForPlayerLevel(3, Player.Level);
        return new List<PlayerItem>(ItemShopOptions);
    }

    public GameState(IEventInvoker _SurfaceEventInvoker) {
        SurfaceEventInvoker = _SurfaceEventInvoker;
        Player = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );

        Encounter = new Draft1Encounter();
        Tiles = new TileGrid(Encounter);
  
        SetPhase(BoardPhase.PLAYERTURN);
    }

    public TileGrid GetTiles() {
        return Tiles;
    }

    public int GetMovesMade() {
        return MovesMade;
    }

    public int GetTilesCleared() {
        return TilesCleared;
    }

    TileGameEvent CheckWinLoseConditions() {
        if (!Player.isAlive()) {
            return new LoseEvent(Player);
        }

        return null;
    }

    public void Invoke(TileGameEvent e) {
        List<TileGameEvent> allResultingEvents = new() { e };

        Queue<IDomainEvent> domainEvents = new ();
        if (e is IDomainEvent @event) { domainEvents.Enqueue(@event); }

        while (domainEvents.Count > 0) {
            IDomainEvent VisitingDomainEvent = domainEvents.Dequeue();

            // Allow this event to come in and modify my properties, returning surfaceEvents
            List<TileGameEvent> resultingEvents = VisitingDomainEvent.Visit(this);
            if (resultingEvents != null) {
                allResultingEvents.AddRange(resultingEvents);
                foreach (TileGameEvent resultingEvent in resultingEvents) {
                    if (resultingEvent is IDomainEvent domainEvent) {
                        domainEvents.Enqueue(domainEvent);
                    }
                }
            }

            // Check for win/lose conditions
            TileGameEvent winLoseEvent = CheckWinLoseConditions();
            if (winLoseEvent != null) {
                domainEvents.Clear();
                allResultingEvents.Add(winLoseEvent);
            }
        }

        // Pass the event along to the surface
        SurfaceEventInvoker.Invoke(allResultingEvents);
    }
    
    public void Invoke(List<TileGameEvent> events)
    {
        throw new System.NotImplementedException();
    }

    public void SetPhase(BoardPhase p) {
        Phase = p;
        // Invoke(
        //     new PhaseChangeEvent(p)
        // );
    }

    public ValueTuple<PlayerItem, PlayerItem> ItemShopPurchase(int index) {
        // if (Phase != BoardPhase.ITEMSHOP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE ITEMSHOP");

        PlayerItem ItemPurchased = ItemShopOptions[index];
        ValueTuple<PlayerItem, PlayerItem> itemSwap = Player.AddItemToInventory(ItemPurchased);
        Player.SpendDownCoins();
        ItemShopOptions = null;
        return itemSwap;
        
        // Trip();
        // Debug.("PURCHASE");
    }
}




    // public void LevelUpPurchase(List<int> indexes) {
    //     if (Phase != BoardPhase.LEVELUP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE LEVELUP");

    //     List<PlayerSkillup> skillupsPurchased = indexes.Select(o => XPShopOptions[o]).ToList();
    //     Player.AddSkillupsToInventory(skillupsPurchased);
    //     Player.SpendDownExp();
    //     XPShopOptions = null;

    //     Trip();
    // }



    // public void EnchantmentShopPurchase(int index) {
    //     if (Phase != BoardPhase.ENCHANTMENTSHOP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE SHOPPING");

    //     PlayerItem EnchantmentPurchased = EnchantmentShopOptions[index];
    //     Player.AddEnchantmentToItemInSameSlot(EnchantmentPurchased);
    //     Player.SpendDownDefensePoints();
    //     EnchantmentShopOptions = null;
        
    //     Trip();
    // }


    
    
    
    // public PlayerItem[] EnchantmentShopOptions { get; private set; }
    // public PlayerSkillup[] XPShopOptions { get; private set; }
    // ItemSelector itemSelector;
    // XPOptionSelector xpOptionSelector;
    // EnchantmentSelector enchantmentSelector;

        // itemSelector = new ItemSelector();
        // enchantmentSelector = new EnchantmentSelector();
        // xpOptionSelector = new XPOptionSelector();


        
        // ItemShopOptions = itemSelector.GetShopItemsForPlayer(Player);
        // EnchantmentShopOptions = enchantmentSelector.GetEnchantmentItemsForPlayer(Player);
        // XPShopOptions = xpOptionSelector.GetXpOptionsForPlayer(Player);