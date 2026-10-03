# Grid

Grid represents 3x3 set of fields on the board.

`BoardGrid` entity has data about all fields in the game along with methods allowing to:
- identify fields by coordinates,
- compare two fields,
- count fields or cards,
- find a board card.

## Coordinates

Each field can be identified by coordinates depending on how they're positioned.
- They're `Vector2Int` type.
- Each coordinate in range from -1 to 1.
- X-axis is incremental from left to right when looking at the board.
- Y-axis is incremental from bottom to top when looking at the board.

Relative coordinates show how the fields would look like when rotated by certain number of right angles. They help with calculation for rotated cards following rules of trigonometry.
Example: a card on the top-right corner (`coord: 1,1`) rotated clockwise by a right angle will be positioned on the top-left corner (`relativeCoord: -1, 1`) when looking at the card facing forward.
