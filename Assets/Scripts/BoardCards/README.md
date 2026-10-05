# Board card

Board cards are squared sprites put on fields. They have actual stats, they can move between fields, rotate, attack and be attacked. They also have an ability that may affect other board cards and can be affected by other cards' abilities.

## Bars

Actual board card stats are displayed by the length of their bars on the sprite outline.

## State

With State Machine design pattern, state for board cards have been implemented representing how the player can interact with the card. The states can be split into several categories.

There are two main states:
- **Active State** - the player can move (to neighboring field), rotate or attack (once per turn) using the card. Applied to owned board cards when requested to make a decision.
- **Idle State** - the player cannot use the card.

When the player commits paid action and payment with hand cards is requested, all cards are in Idle State except for one board card used for the paid action that needs to be clicked to confirm payment and has one of the following states:
- **New Card State** - the new board card previous being a selected hand card. Can rotate freely. The player can undo the decision by clicking the corner button, then the board card disappears and the player has it as a hand card.
- **New Transform State** - the board card has been moved or rotated. The player can undo the decision by clicking on the navigation button of the opposite result.
- **Attacking State** - the board card is preparing to attack all fields on the attack range. The player can undo the decision by clicking the corner button.

Some character abilities cause board cards to enter a special state:
- **Effectable State** - caused by KsiezniczkaBerta's ability on the player's owned cards. One of those needs to be clicked to grant extra stat points.
- **Telekinesis State** - caused by RycerzBerti's ability on the opponent's cards, allowing them to be moved by the player.
