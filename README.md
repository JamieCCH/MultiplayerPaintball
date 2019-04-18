# MultiplayerPaintball
[Game3110]FinalProject


3 Player Online Paintball 

Launch your game, choose whether you are hosting or joining a game.  (i would prefer p2p, but will accept a dedicated server version as well)

If you are hosting, you will either select a colour or be assigned one, it's up to you the developer to make that decision.

You will see yourself in the arena, in the colour that you have been assigned.

The colours are Red, Green and Blue.

The game can only start when there are three players.

Launch another instance of your game, select join instead of host.

You should appear in the game world as a unique colour and see the host and the other player if you are the third to join.

If you are the fourth to join you should be in spectator mode or just get a screen that says the game is full.

Once all three players are in the lobby, a countdown should start. Once the countdown is complete the actual game starts.

The lobby will have a fixed camera, one that covers the entire arena so you can see all the players spawning in.

Once the actual game starts, it switches to a third person or first person view.

Players are spawned away from each other and have the ability to fire their paintball gun.

This is a timed game, matches can be anywhere between 3-7 min.

The time should be displayed to the player, can be the last minute if you like, but make sure the player is aware at some point when the game is going to end.

Scoring

awarded based on successful shots, When one player fires their weapon and hits another they are awarded a point.  The other player should have something on their self that indicates they were hit by that colour.  That indication can last however long you want it to.
Pickups

spawn ammo packs around the world, make sure once a player picks it up it is removed from the world
Gameover

Once the game time expires, make sure the players can no longer shoot.  Display the results
Bonus

Error handling
making sure people that drop out do not crash the game
host migration, if the host leaves transfer host duties to other player
