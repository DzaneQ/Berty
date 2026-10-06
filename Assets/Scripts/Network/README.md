# Network

This section is for stuff focused around multiplayer.

## RPC methods

While MonoBehaviour replacements can be put in other sections accordingly, manager scripts containing RPC functionalities are put in this section to segregate those that need to be attached to a Network Object since adding the component during the scene running will now work properly.

Whenever a new manager script with RPC method is created, it should be set as a component for a game object named `RpcSystem`.
