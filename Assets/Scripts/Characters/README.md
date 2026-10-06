# Characters

This section is about initializing characters according to their attributes and handling their special abilities.

## Skill effect requirements

For a skill to take effect, it has to meet certain requirements. Those are implemented in listeners of board cards in `Berty.BoardCards.Listeners.CardWitnessListener`, where each board card reacts depending on the ability of the card that did an action or the witness' ability when:
- a new card is placed,
- a card is moved,
- a card's alignment is changed,
- a character dies,
- special requirements (turn start, stat value changed).

In one of those above, the ability takes effect, some of those being global or requiring to be a neighbor of the card subject.

## Skill effect

When specific requirement is met, the listener calls `ApplySkillEffectManager` to take effect. Some effects can be done to a character only once (resistance system) to avoid them being too overpowered. Furthermore, the effect can only be applied to certain cards depending on the alignment or role.

## Stat modification

Some abilities are a subject to stat modification, handled by `ModifyStatChangeManager` so that the stat change can be weakened, strengthened, prevented depending on specific criteria.

## Custom change

Some abilities have effects that cannot be handled with above mechanics, those require to be hard-coded in specific parts, asking whether a skill of specific `CharacterEnum` is viable, that can be found with `Ctrl + Shift + F` within the solution. Some custom skills are implemented:
- with a separate manager in this section,
- with extra attribute in the entity (`bottomCard` in card pile for BertWho, `backupCard` in board card for `TrenerPokebertow`),
- overriding what happens when character's stat drops to 0 in `Berty.BoardCards.Behaviours.BoardCardEntityHandler`,
- extra card states (KsiezniczkaBerta, RycerzBerti),
- statuses.

## Statuses

Yet another "some abilities" cause statuses to appear if they take effect for custom period of time such as:
- the card causing an effect as long as staying on board,
- manual effect handling.

Whenever a status is added or removed, it should be handled with respective events received in `StatusListener`.
