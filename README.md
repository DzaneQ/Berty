# Berty

Berty is a game where you play and pay with cards. The author and card designer is Hubert Gulczyński. The game has been developed in Unity by Patryk Szczęśniak.

## Demographic

The game is for players who:
- like card games with unorthodox rules;
- are into 1v1 matches, PvE or PvP;
- have Windows as the platform.

## Interactable objects

During the game, the player can interact with the following gameplay features:
- **Fields** (Grid.Field) - tile 3D models to put a card on;
- **Board cards** - card sprites on fields, can be clicked to attack;
  - **Card buttons** - arrow 3D models on board cards, can be clicked to rotate or move the card to a neighboring tile according to the arrow directions;
- **Hand cards** (UI.Card) - UI cards visible only to the player owning them to put on a field or to pay with;
- **Corner button** - UI button to end the player's turn or undo a decision.

## Card attributes

Each card has the following information:
- **Name and pic** - represents a culture, legend, occupation, view or religion.
- **Strength** (green bar) - how much health is subtracted from an attacked card.
- **Power** (blue bar):
  - determines how many hand cards the player has to discard in order to put the card on a field;
  - when being put on a field, enemy cards with higher power will attack the card if within their attack range;
  - hitting 0 converts the card to the opposing alignment replenishing the initial power.
- **Dexterity** (yellow bar):
  - the missing value determines how many hand cards the player has to discard in order to attack, move or rotate using the card;
  - hitting 0 makes the card unable to play with until it reaches the initial dexterity, regenerating by 1 every turn.
- **Health** (red bar) - hitting 0 kills the card.
- **Attack range** (left square grid) - determines which fields will be attacked when the attack is ordered, relatively to the card facing forward. Friendly fire on!
- **Defense range** (right square grid) - determines how the card reacts to the neighbor's attack:
  - X on orange - no reaction, taking the hit (same for non-neighboring cards);
  - white (not diagonal) - riposte, will attack the attacker for the strength's value;
  - B on blue - blocks the attack, no damage taken.
- **Ability** - unique for each character, may affect other cards, override or modify some stats and mechanics.
- **Role** (the colored square next to the name) - reacts to some abilities of other cards.

## Gameplay loop

2 players take turn alternately. On turn start, the player draws cards from the pile until they have 6. When the pile is empty, discarded cards become the pile cards. The player makes decisions in the loop until the turn is finished:
1. End the turn or play with a card:
    - put a hand card on a free field for a new board card;
    - attack using a board card (each card can do it once per turn);
    - rotate a board card by the right angle;
    - move a board card to a neighboring field.
2. Choose hand cards to discard paying for the decision with the card.
    - If it's a new board card, rotate the card freely to adjust the direction.
3. Confirm payment or undo the decision.

## End condition

The game ends when one of players owns 6 board cards or there are not enough cards in the pile to have 6 hand cards. The player with more board cards wins. If it's equal, the player who has the most cards in one role wins.
