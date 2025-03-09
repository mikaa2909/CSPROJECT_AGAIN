using Microsoft.Xna.Framework;
using System;

namespace MAZEGAME
{
    public class OrangeEnemy : Enemy
    {
        public OrangeEnemy(int x, int y, Direction direction, Rectangle sprite) : base(x,y,direction, sprite){
        }

        public override void updateEnemy(Tile[,] tileArray) {
            Vector2 toBePosition = calculateBasedOnDirection(currentDirection, positionX, positionY);
            if (isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray)) {
                positionX = (int) toBePosition.X;
                positionY = (int) toBePosition.Y;
            } else {
                Random random = new Random();
                Direction randomDirection;

                do {
                    Array values = Enum.GetValues(typeof(Direction));
                    randomDirection = (Direction)values.GetValue(random.Next(values.Length));
                    toBePosition = calculateBasedOnDirection(randomDirection, positionX, positionY);
                } while (!isTileMoveable((int) toBePosition.X, (int) toBePosition.Y, tileArray));
                currentDirection = randomDirection;
                positionX = (int) toBePosition.X;
                positionY = (int) toBePosition.Y;
            }
            updateDirection();
        }

        private void updateDirection() {
            if (currentDirection == Direction.Down)
            {
                currentGhost = new Rectangle(1707, 339, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentGhost = new Rectangle(1611, 339, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentGhost = new Rectangle(1419, 339, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentGhost = new Rectangle(1515, 339, 42, 42);
            }

        }
    }
}