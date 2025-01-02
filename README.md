# win98labyrinth
A .NET 9 MAUI implementation of 1986 Ravensburger(r) board game

## how to Run
Build and run the solution on windows.

## how to Play
4 players: red, green, blue, yellow.
Each player has an equal amout of treasure objectives (alphabet letters from A to X).
On each turn, the player tries to move to the treasure in the labyrinth.
First, insert the path tile lying next to the game board and then move the piece on the board.
There are 12 arrows along the edge of the board. They are marking the rows where the path tile can be inserted into the maze.
THe only exception: the path tile cannot be inserted back into the board at the same place where it was pushed out.
If the pushed path tile has a playing piece on it, this piece is put on the opposite side of the board on the path tile that was just placed.
The game is over as soon as a player has found all the treasures and returned to its starting position.