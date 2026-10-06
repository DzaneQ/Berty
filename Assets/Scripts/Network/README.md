# Network

This section is for stuff focused around multiplayer.

## Scope

To minimize potential unfair play as much as possible, specific principles are supposed to be followed:
- Any data check and calculations with entities should be proceeded in the server.
- Object display and animations should be limited to clients.
- Clients must not have access to actual `CardPile` entity values especially about the opponent's table content and pile deck.

## Syncing

The game doesn't require synchronizing between clients every frame. Synchronizing is limited to when:
- a turn starts,
- a payment is confirmed,
- an ability requiring manual action is performed (KsiezniczkaBerta, GotkaBerta).

For all other actions syncing is not required meaning whatever the client does not affect the other client, such as trying to make a move and undoing it or field highlights.

## RPC methods

While MonoBehaviour replacements can be put in other sections accordingly, manager scripts containing RPC functionalities are put in this section to segregate those that need to be attached to a Network Object since adding the component during the scene running will now work properly.

Whenever a new manager script with RPC method is created, it should be set as a component for a game object named `RpcSystem`.
