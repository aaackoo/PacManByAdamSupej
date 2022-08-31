# PacmanByAdamSupej
### Adam Šupej, I. year, Bc. study, Informatics IPP
### Summer semester, 2021/22
### Programování 2, NPRG031
## Task
My game is inspired by the original arcade Pacman game. I created this game using Unity and it's scripts in C#. My Pacman game is not a complete replica of it's predecessor, a few elements are missing such as cherries, more lives for pacman etc., and I decided to change some elements because I liked it more that way. 

## Game course
As soon as a player opens the game, he is already playing. The player can control Pacman's movement by arrow keys, he has to collect all pellets (ingame food- small balls) while trying to avoid all 4 ghosts. 
Pacman game starts with ghost's being in a Scatter mode where each ghost targets one tile outside of map's boundaries and tries to follow this tile for 7 seconds. Right afterwards they change into Chase mode (they are moving based on their A.I. to their targeted tile) for 20 seconds. Once Pacman eats powerpellet (bigger pellet), ghosts become frightened of Pacman for 6 seconds, remember their previous state and move randomly at every waypoint. After being frightened they return to their previous state. Ghosts are forgiven to turn backwards during the whole game. Game restarts wether a ghost catches Pacman or when the player collects all pellets from the game.

## Algorithms
The only algorithms I used in my game are for GHOST movement. They are based on original game, where I had inspired myself. At every waypoint ghost draws a straigh 2-dimensional vector from his possible turnings to his desired target. Target is different for every ghost:
 * Blinky's target is Pacman itself, 
 * Pinky's target is Pacman + 4 tiles ahead of him,
 * Inky's target is a vector drawn from Blinky's position to Pacman, but multiplied by 2, 
 * Clyde's target is same as Blinky's (Pacman's current position), but if the vector that is drawn is longer than 10 units, he goes to his Scatter mode and ignores Pacman.

## Main program
In my program, I used just one Scene for the main Game. I created BoxCollider2D for map "walls" so Pacman cannot pass through them and also BoxCollider2D for pellets but with trigger mode. For Pacman I decided to use circle colliders with Rigidbody2D, because it suits him much more. With ghosts I also used circle colliders with Rigidbody2D, but again with trigger mode. To control Text I used pure Canvas with Text. To teleport I used again BoxCollider2D with trigger mode ticked. Ghost waypoints do not have any kind of physics, they just know their position and host a script.

All animations (Pacman animations-Integer variable for setting his direction, and ghost animations-2 bool variables for setting wether he is blinking/frightened) in my program are controlled by Unity Animator and its variables are being updated from my scripts.

I divided my program into more scripts.
 * Script for controlling Pacman. This script has many functions nescessary for controlling the movement of our Pacman, setting animations in animator, updating Pacman's state, controlling if something is in his way and can safely change his direction or move further for displaying the Pacman and soft moving from one tile to the second. Also it has some design functions such as playing various sound effects when Pacman eats normal pellet or powerpellet, I implemented a timer function for controlling the state of our game and also for restarting the game when Pacman dies/wins.

 * Script for controlling ghost movement. This script is the most extensive. It controls ghost's movement, controls ghost's A.I. which I have already explained in algorithmic part above. It controls and updates ghost states (including respawning, etc.), and also features a timer for changing Scatter to Chase mode.

 * Script for managing ghost waypoints. This script is only for setting variables (where ghost can move next from current waypoint and which waypoints are its neighbours) in Unity's inspector and for ghost's to work with those variables and waypoints.

 * Script for managing pellets. This script controls only pellet collision with player, it deletes pellets upon contact, alerts Pacman which pellet he has eaten and "says" how many points to add to score counter instance.

 * Script for score counting. This script controls score counting. It adds points to current score based on script mentioned above and also controls highscore (I used PlayerPrefs) and wether it should update or not. It also controls winning and loosing text.

 * Script for teleporting. This is a basic script to determine if a player/ghost entered a portal on one side of map and teleports it to the other side.

## Alternate realization
Algorithms used for ghost movement can be improved to be more reliable in every condition, since I used algorithms inspired by the original game. For example a BFS for finding the shortest path to a targeted tile would be much more consistent in finding the best path for each ghost, but the game would be much harder in my opinion. Also many details like Pacman's speed can be updated, ghost's speed and many others...

## Input data representation
The only input data are arrow keys on keyboard which control the Pacman's movement.

## Output data representation
Pacman game has only one possible game-screen state where we can see the whole RUN. We see our map with pellets, ghosts, current score counter, highscore and most importantly, Pacman.

## Course of work
I was working on this game for a full week for around 10 hours a day. This was my first bigger unity project so I was learning a lot of stuff in between. 

## What has not been done
I think I have implemented everything I wanted to, but according to original game, I didn't implement fruits (cherries, strawberries,...), original game also has more lives for Pacman, but I decided not to implement this feature and make it a bit harder for the player.

## Conclusion
I had a lot of fun during the process of making this game, I have learned A LOT of new stuff from unity's game making, which I am really happy for, since this was my first bigger project in UNITY.