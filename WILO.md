# WILO

### 9-14
let's get a level going and we can build out the layer above and between floors. Don't refactor early. Just build it:
- Monster HP/DMG scale on a curve from 1 turn made to 220 turns made
- 3 scheduled mini-bosses.
- final boss
- portals drop
- player needs to scale


### 9-4 
- wrote a cool thing with chat that moves a rectTransform from A to B over time T. Can add archHeight A and offset angle of the arch O. if archheight is 0, no wasted calculations. This is a whole ass coroutine built into a single call. MoveTo(B, T, A) that can be yielded... this literally should be on the RectTransform class. no?
- or could just be an outside actor, pass it a RectTransform and puppeteer it around. 
- then the actor can't store and own the B/T/A numbers
- if its owned on the actor, it could keep a list of pre-determined moves or accept a target at runtime. i like that best.
- don't refactor too soon. move on.
- need to consider a better apparatus for screen shake but its awesome, deal with the impl
- BTA defined in public variables, also default values there... now my weights that i NEED to keep are being stored in the scene and not the code anywhere. that's a bitch. look into this and consider ScriptableObject for permanency during tweak sessions
- thought; you could define enemy waves as a curve and modify in the editor
- ok not done with the performmonstersattack.
  - fade the shield when it doesn't get to bonk
  - fade the attacker when it gets rejected
  - time to get the bottom labels synced with the actors
  - nearly finished. next to consider the collection performances and the attack performance
  - maybe fade background for the monsters attack phase?
- get the in-editor weights safely out of the scene
- we have some reusable scripts, `MoveRectTransformAlongArch` and `Drift`
- we cracked some eggs, a lot to clean up
- refactors include:
  - moving performance ienumerators in GGM into a more isolated, obvious file
  - inheritable mono with `MoveRectTransformAlongArch` and `Drift`
  - better interface with screen shake. love effect, hate impl
  - BTAs in MRTAA should probably be serialized better
  - Stage Roster orchestrator!! (parental layers for canvas rip)



### 9-2 WILO
- Back in the buffer
- Working on `PerformMonstersAttack` Performance Coroutine currently living in GGM.
- Monsters pop their numbers, its totalled
- The armor shows up with accurate count
- After delay the Armor Left and Dmg-to-HP values are showing

- Need to do a bonk, chg numbers at bonk
- flying armor result text to the left of shield, pierced, total shield rolls
    - x 5 SHIELD
    - x 2 BROKEN SHIELD
    - x 1 REPAIR SHIELD
- the bonk should get a reaction:
    - if 0 dmg left, it stops quickly and rolls off and falls off screen
    - if dmg left, bounce off and slam the HP bar, bouncing off screen
- ** if no shields, should bonk from the top rope, skip bonking armor. maybe armor fades trying to move in the way of the dmg but is gone before it arrives


THOUGHT: all movements could be animations as long as we know the bounds, root, and have mountable bones to attach UI elements like text boxes


### Hello from 9-1-2024
first draft readme to help with brain buffer next time..


### 2-25-24
ok, worth noting: any time the user is taking some action against game state, we should be
getting that through the inputmanager. the plumbing is good if we keep these all together.

onchainstart, onchainend, selectPurchaseItem etc etc



### 2-19-24
Game is in the mental buffer for the first time in 6 months.

Looks like i did a pretty good job stripping it down to the metal.

- SurfaceEvents vs DomainEvents aren't super obvious. I suspect DE's need to fire in order and SE's are just for UI purposes, but I wonder if DE's skipping the line is what we truly want.

- In a recent game, i had events fire EARLY, execute, LATE and i don't think i used it. Might come up here, worth remembering.

- Chicken and the Egg game is on full display with dependency mapping in the TileGame. I think everything is well done here, though.

- Really dunno what to make of DomainEvents ngl

- I think this is in a good place to start in on.

- Phase resolution needs love. it's a bit of a domino system and doesn't leave obvious room for variation.

- First thing's first: Get the upgrade windows firing and adjusting the stat sheet!


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


12-8-24
thought: 
scaling feels good at the base level. don't currently have player scaling, but can imagine its effect. increasing monster dmg scale. 
time to introduce player scaling:
- Experience Level Up
- Gold Level Up
- Armor Level Up

wilo: sub-scene riffing. have a subscene bootstrapper for things like camera and eventsystem but what about building a UI directly into the menu scene to allow devs to manually set what is in the user's inventory? what the options are? what about manufacturing them on the spot? then they could maybe hit a button and save it as a SO. 
i think building tools inside of the game for the dev is the move. we'll learn it quickly and it'll literally hand the game over to the team to balance and design.


12-24-24
WILO: story 106 enables 105+

we don't need to revamp the whole system. i kind of like Domain vs Game Events. it allows the event engine to double as the Processor.

Domain Events can chain amongst themselves.

What we need to bring in from our last couple of games is the idea of Action-Invoked State Updates (#106).  Instead of putting it on the "Events" as we know them from OMF, we're going to incorporate them into sub-systems of the event chain like `CollectionResult`. This has all of the information the UI could ever need to show a collection event. This is the perfect place to provide an Action hook to the actual state update.

In the future, we'll have to consider that if we insist the UI commits state updates, there will need to be an auto-commit adapter for headless runs.


###  12-26-24