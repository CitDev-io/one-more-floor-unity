# one-more-floor-unity
Unity project for ONE MORE FLOOR by Citizen Developers

### Testing
C# Testing needs to occur in Unity's `Test Runner` window since Unity uses C# as a facade transpiled to CPP. 

# Developer Orientation

## Codebase Partition
MONO Scripts are Unity. UI and Video Game logic.
CORE Scripts are OMF. No access to Mono, just state/events/resolvers

## Game Bootstrapper
All games I write can be bootstrapped by this system.  It doesn't inherit from a BASE class but should and will eventually for other projects to leverage.

SceneBootstrapper Mono should be present in every scene. This ensures the `GameController_DDOL` is loaded in as a singleton and ready to interact with all citizens of the scene.

#### `GameController_DDOL` is your tippy top DDOL.
you can add public variables to pass around between scenes. yep. just public!

This comes with lots of possible issues but allow much faster prototyping. Handle with care.


## Scene Requirements
- Include the SceneBootstrapper prefab
- A Scene manager that is the top orchestrator for this scene. 

## GridGameManager
Tilegame orchestrator

**8-31-24** :
  - handling UI float texts (should be delegated)
  - Spawns Game Cartridge, Board
  - hooks up events to citizens
  - adds listeners to all cartridge events
  - Grabs hooks for GameController
  - Grabs hooks for Scene's GridInputManager
  - Creates it's own LineRenderer
  - Stores refs for Menu Nodes
  - Maintains switchboard from cartridge events to citizens
  - Triggers sound effects in switchboard
  - Inline GameOver Routine logic


## CORE
  - TileGame is the Game Cartridge entrypoint
  - Prioritizes mindful interfacing between classes 
  - Four Utility Groups, matches folder structure
    - Events: Hi! Create any Event you want and call .Invoke() on me
        - Events resolve in-line synchronously. "Handle" event subscribers should hangup fast
    - State
    - Chain: Tile selection mgmt
    - Input

## Events: TileGameEventSystem
    - Passed-around Invoker, if you have ref to this: create an event and TGES.Invoke() it to put it on the stack
    - Must register events here as a developer
    - This is the obj you grab to subscribe as well: "On" events are public
    - `TileGameEvent` base class for event invoking, blank. each impl can do whatever it wants
    - Events/TileGameEvents for custom impls
    - Handle With Care: Should have an interface to allow TGES to be ref'd only as a EventEmitter. **Current look of TGES shows that this isn't easily teased out without also managing the manifest of events in one additional place, worthy refactor, skip for faster prototyping, Handle with Care**

## Events: IEventInvoker
    - IF you are someone that only intends to publish events the TGES can be ref'd as a IEventInvoker, exposing only the invoke method
## Events: IEventEmitter
    - Doesn't exist, but could. Provides extra protection from publishing where only consumption was intended.
## Events: IDomainEvent (added to a TileGameEvent)
    - Visitor pattern enforced
    - **Domain Events are Game State Reducers**
        - ex: ChainAcceptedEvent, MonsterTurnEvent
    - THOUGHT: So TGES is a `IEventInvoker` but cannot visit domain events..? GAMESTATE of all things is also an `IEventInvoker` but it CAN visit domain events.
    - 8-31-24: Only one reffing GameState as a EventInvoker is the ChainBuilder.
        - It scoops out the domain events and enqueues them
        - It then dequeues and visits
        - The Visit returns a list of TileGameEvents
        - these events are collected to a top-list which will be sent to the `Surface Invoker` or `TGES` ref'd
        - of these events returned, if any are domainEvents, they will be added to the Queue (not stack)
        - after each visit, WinLoss conditions are checked
            - if an end has been reached, discard remaining domain events. add winlose event to list and return
    


## Input: UserInputHandler
    - so far: just a switchboard to fire ChainBuilder events

## Chain: ChainBuilder
    - give it a list of tiles on the board (why?)
    - give it an event invoker ref (it wants to call, not listen)
    - CALL ME as `IChainBuilder` if you want your ref to me to only be my public method that you can affect the chain
    - contains all of the logic to validate a chain, understand and reset chains on moving indications
    - fires events for the UI to pay attention to the changing state of the chain

## Chain: ChainValidator (abstract)
    - is nested within a chain builder. when we have variable validations, we'll need to pass or infer the validator-type to the chain builder so it can instantiate the right validator class
    - contains private local logic and base logic
    - public methods used by chainbuilder: `.isChainFinishable()` and `isEligibleToAddToChain(Tile t)`
    - two overridable methods available to inheritors: `moreIsChainFinishable()` and `moreIsChainable(Tile,Tile)`
        - `moreIsChainFinishable` should be **true** if you don't have additional reasons to deny it
        - `moreIsChainable` should be **false** if you don't have additional reasons to allow it
    - Standard validator implements methods with direct returns

## State
    - This biggest part. Full State and Reducers.
    - CALL ME as `ITileGridProvider` if you just want a ref to the current Tile set on the board
    - CALL ME as `IEventInvoker` if you want someone that can pre-process `IDomainEvent`s (but honestly this should be another interface to be certain, right?)


## Tile Game Event Flow
One major domain event drives the game state more than anything: `ChainAcceptedEvent`. We managed all of the Chain Events to build one, but one's been accepted now! Consider that this is the majority of "what happens" in a match tile game.
- When ChainBuilder signs off on the user input and accepts it (by invoking the event on its ref of GameState):
    - We increment the moves made this game
    - We count: how many coins did you get? how many of, well, each tile? write the event and add it to our list
    - We award the points, the gold, the shields collected.. state is updated directly. (that's gonna need to change)
    - We do combat! check the collected chain and modify whatever state you need: stun the monster, deal damage, make a kill, get experience and loot. Just don't forget to codify it as an TileGameEvent instance and add it to the list!
    - At the end of the event list we trigger the next event to happen (monsterturnevent)
        - that IS a Domain Event. it will be visited immediately following.
        - think about the event stack here, we're doing game logic. doing it irreversibly and triggering chained events from that and applying them as well.
        - all along, we've been collecting up a list of what happened, but none of these have yet been reported to the UI.. but at the very end its all dropped at once

## UI Event Capture
Since a big pile of events all come in at once, the UI (over on Mono side) is responsible for capturing them and deciding on the timing that the user should see an event performance and the updated value/state.
- Simple UI spying on GameState directly will cause it to update potentially too soon!
- If UI elements rely on updating in response to an event, it can be queued and fired.
- Need to rephrase this section. Best move is to use callbacks (Actions) that make the state change and let the UI choose the timing of pushing the button on the queue.
    
## Neat-o Debug Trick
Yes, it's dumb. But it's totally within the spirit of the system. You can fire a `DebugLogEvent` on the CORE side of the code and it'll get caught and rendered by Unity's console. Significant because CORE is a no-mono space and is agnostic of UI implementation.
Or just spell it out as UnityEngine.Debug.Log and remove before prod build.


## Need:
- New Phase Between ChainAccpted and MonsterTurn
- Tile Introduction! Bring new tiles onto the board. if its a big deal creature, give him a close-up.. then let the monsters do their thing. I know the timing could be weird considering effects caused by someone who would need a close-up. It can be taken care of thematically either way that you'd like to rule that. 
- Consider that all of the events that are made to codify what you do here in the event... the actual method of applying the event against state should happen in the event itself as a domain visit.
    - And still happen synchronously in the order identified
    - Make ChainAccepted determine the order of collection evaluation domain events and isolate into game logic decided resolution phases. codify this, it'll be important for debugging.
    - Current State: (inline, not in phases yet)
        1. COIN/POTION/SWORDS Collected Events
        2. SHIELD Collection Event, (armor or bash)
        3. DAMAGE APPLY Event (xpgain, monsterkill, dmg)
        4. COLLECTED TILES Event
        5. MONSTER TURN Event
- Writing straight changes to state feels like i shouldn't, but we're deep in the CORE game logic and this is where it goes. We resolve effects as they appear in order. Clarity as to this order is everything in design and debug.
- This resolution system might require some effects to check and see if the conditions still merit application. that'll be weird.




State Notes:

- TileGrid is a fresh board
- TileSelector chooses a tile from its Options list of TileTypes
- TileTypes is the enum registering the possibilities to be given to a tileselector



- Encounter
    - TileSelector: choose next tile
    - MonsterSelector: gives monster stat sheet
    - ColsxRows for board
    - NextMonsterProvider, NextTileProvider




# Unhooked Stuff
- EnchantmentSelector: provides a random enchanted item
- Shops
- ItemSelector


## An Encounter 
is the object that holds specific logic for:
- how the board should be set up
- how we choose next tiles
- how we choose next monster

it could be:
- mono specific values too as long as we interface to the base object what it wants
    - background
    - sound fx set
    - uI upgrades
- a scriptable object, allowing serialized settings per encounter


