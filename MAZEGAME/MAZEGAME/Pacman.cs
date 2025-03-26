using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MAZEGAME
{
    public class Pacman : MoveableCharacter
    {
    private int positionX;
    private int positionY;
    private Direction currentDirection;
    private Direction nextDirection;
    private Rectangle currentPacMan;
    private int pacManIteration;
    private int noOfLivesLeft;
    private Rectangle[] pacManUps = new Rectangle[3];
    private Rectangle[] pacManDowns = new Rectangle[3];
    private Rectangle[] pacManRights = new Rectangle[3];
    private Rectangle[] pacManLefts = new Rectangle[3];

        public Pacman() {
            setInitalState();

            noOfLivesLeft = 3;

            // Loads the different sprites that the player would have 
            // for different directions and with the mouth open to 
            // varying degrees

            pacManDowns[0] = new Rectangle(1371, 147, 39, 39);
            pacManDowns[1] = new Rectangle(1419, 147, 39, 39);
            pacManDowns[2] = new Rectangle(1467, 3, 39, 39);

            pacManUps[0] = new Rectangle(1371, 99, 39, 39);
            pacManUps[1] = new Rectangle(1419, 99, 39, 39);
            pacManUps[2] = new Rectangle(1467, 3, 39, 39);

            pacManLefts[0] = new Rectangle(1371, 51, 39, 39);
            pacManLefts[1] = new Rectangle(1419, 51, 39, 39);
            pacManLefts[2] = new Rectangle(1467, 3, 39, 39);

            pacManRights[0] = new Rectangle(1371, 3, 39, 39);
            pacManRights[1] = new Rectangle(1419, 3, 39, 39);
            pacManRights[2] = new Rectangle(1467, 3, 39, 39);
        }

        public void setInitalState() {
            // Sets the inital values of the variables of the player
            positionX = 13;
            positionY = 23;
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            // Position of pacman in the sprite sheet
            currentPacMan = new Rectangle(1419, 3, 39, 39);
            // Inital of pacman (mouth wide open) 
            pacManIteration = 0;
        }

        // Returns the x position in the tile array of the player
        public int getX() {
            return positionX;
        }

        // Returns the y position in the tile array of the player
        public int getY() {
            return positionY;
        }

        // Returns the current direction the player is facing
        public Direction getCurrentDirection() {
            return currentDirection;
        }

        // Moves the player on every update
        public void updatePlayer(Tile[,] tileArray) {
            // Calculates what the next position will be based on the next direction
            Vector2 toBePosition = calculateBasedOnDirection(nextDirection, positionX, positionY);

            // If this position is a valid move then the next direction of pacman will be that direction
            if (isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray)) {
                currentDirection = nextDirection;
            }

            // Calculate the position based on the current direction and set the sprite facing that direction
            toBePosition = calculateBasedOnDirection(currentDirection, positionX, positionY);
            setCurrentPacman();

            // If we can move to this position then move
            if (isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray)) {
                positionX = (int) toBePosition.X;
                positionY = (int) toBePosition.Y;
            }

            // Makes the mouth open and close
            pacManIteration = (pacManIteration + 1) % 3;
        }

        // Sets the next direction the player wants to move to based on the keyboard key pressed
        public void changeDirection(KeyboardState kState) {
            
            if (kState.IsKeyDown(Keys.Up))
            {
                nextDirection = Direction.Up;
            }
            else if (kState.IsKeyDown(Keys.Down))
            {
                nextDirection = Direction.Down;
            }
            else if (kState.IsKeyDown(Keys.Right))
            {
                nextDirection = Direction.Right;
            }
            else if (kState.IsKeyDown(Keys.Left))
            {
                nextDirection = Direction.Left;
            }
        }

        // Sets the sprite to be used facing in the direction wanted
        private void setCurrentPacman() {
            if (currentDirection == Direction.Down)
            {
                currentPacMan = pacManDowns[pacManIteration];
            }
            else if (currentDirection == Direction.Up)
            {
                currentPacMan = pacManUps[pacManIteration];
            }
            else if (currentDirection == Direction.Right)
            {
                currentPacMan = pacManRights[pacManIteration];
            }
            else if (currentDirection == Direction.Left)
            {
                currentPacMan = pacManLefts[pacManIteration];
            }
        }

        // Return the current sprite to be used
        public Rectangle getCurrentPacman() {
            return currentPacMan;
        }

        // Gets the number of lives the player has left
        public int getLivesLeft() {
            return noOfLivesLeft;
        }

        // Decrease the number of lives the player has
        public void decreaseLife() {
            noOfLivesLeft -=1;
        }
    }
}