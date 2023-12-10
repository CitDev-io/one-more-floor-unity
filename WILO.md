# WILO

- Adventurer, Ranger, Druid, Rogue, Paladin, Wizard/Mage, Warlock, Cleric/Priest

### 8-13-23
- Revert UI tiles back to GameObject tiles
- distinguish between surfaceEvents and DomainEvents
- some SE fired by the core, some can be DE's converted by the UI side handler
  - events can output more events after state has changed
  - can batch event resolution before win/lose conditions, pass list
- Collect Tiles logic converted to process outside of GameState by visiting each event


IDEAS:
* GameTile movement can be an enqueued move-to-coords system - s/b able to go in any direction
* single tile workshop scene: drop a tile at 1,1,0 and tell it to behave certain ways
* configurable chain rules, select which are active. have a workshop to test.


### 8-12-23
- Continued refactors
- Answered question: How do i add a new tile type?


### 8-9-23
- Continued doing refactors
- Game Logic (phase resolution) is living on the GameState object right now
- if we let GameEvents modify state, it doesn't have to be immutable. it just has to be assemblable and modifiable
- game phase resolution can just be events that resolve but we'd need something that prevents user input from occuring out of cycle
- need something that evaluates collected tiles and turns that into events
- some events won't modify state. they will be for exposing things to the UI layer
  - gold collected, what positions could be used for the UI layer to display the gold popping into the purse
  - playerTurnEnded can signify when to disable user inputs since it won't be able to affect gamestate
- SURFACE EVENTS vs STATEMOD EVENTS
  - "Events" should really be a SurfaceEventEmitter that is exposed to the UI layer
    - events that we truly care about should not be fired through there
    - SEE should be listening to the goings-on events of the app and it should translate StateMod Events into SurfaceEvents
- CONSIDER: Storing a ref to a Tile is different than storing the x/y of a tile and grabbing it. One allows us to rebuild state during transitions, the other does not.
   - impl of this comes down to: State.Board.Tiles() instead of State.Board.Tiles worth it.
- Learn more about the:
    - Observer Pattern which i think we already are fully doing, right?
    - Event Bus or Middleware for routing events to appropriate handlers


### 8-5-23
- Refactored the bajesus out of the CORE objects
- Everything is still working afaict
- I'm wholly uninterested in anything that isn't base vanilla bottom bedrock mechanics for the game. just collect shit and show it


- Need to rewalk through the upgrades that turned the UI into a single object in the scene.
- Can this even work? Is it just as static as it seems?
- Can spine assets play nice here?


- UI side could probably use a refactor fest too

- Core could still use some optimizations around COMBAT, Tile Collections, Game State, and Phase Mgmt (see CORE/TileGame.cs)




big questions:

#1
what's the final answer on how we'll be displaying the grid?


#2
what all can we do with github actions?


#3
can we sidecar a test project for the CORE files?

