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
            positionX = 13;
            positionY = 23;
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            // Position of pacman in the sprite sheet
            currentPacMan = new Rectangle(1419, 3, 39, 39);
            // Inital of pacman (mouth wide open) 
            pacManIteration = 0;
        }

        public int getX() {
            return positionX;
        }

        public int getY() {
            return positionY;
        }

        public void updatePlayer(Tile[,] tileArray) {
            Vector2 toBePosition = calculateBasedOnDirection(nextDirection, positionX, positionY);
            if (isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray)) {
                currentDirection = nextDirection;
            }
            toBePosition = calculateBasedOnDirection(currentDirection, positionX, positionY);
            setCurrentPacman();
            if (isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray)) {
                positionX = (int) toBePosition.X;
                positionY = (int) toBePosition.Y;
            }
            pacManIteration = (pacManIteration + 1) % 3;
        }

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

        public Rectangle getCurrentPacman() {
            return currentPacMan;
        }

        public bool decreaseLife() {
            noOfLivesLeft -=1;
            return noOfLivesLeft == 0;
        }
    }
}