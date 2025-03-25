using System;
using Microsoft.Xna.Framework;

namespace MAZEGAME
{
    public class RedEnemy : Enemy
    {
        public RedEnemy() : base(){
        }

        public override void setInitalState()
        {
            positionX = 13;
            positionY = 11;
            currentDirection = Direction.Right;
            currentGhost = new Rectangle(1371, 195, 42, 42);
            currentMode = EnemyMode.Scatter;
        }

        public override (int, int) getScatterTargetPosition(Tile[,] tileArray) {
            Random rng = new Random();
            int x, y;
            do {
                x = rng.Next(13); 
                y = rng.Next(9); 
            } while (!isTileMoveable(x, y, tileArray) || (x == positionX && y == positionY));
            return (x, y);
        }

        public override (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition) {
            int targetX = pacmanPosition.Item1;
            int targetY = pacmanPosition.Item2;

            if (pacmanDirection == Direction.Down)
            {
                if (targetY + 2 < 31) {
                    targetY += 2;
                }
            }
            else if (pacmanDirection == Direction.Up)
            {
                if (targetY - 2 >= 0) {
                    targetY -= 2;
                }
            }
            else if (pacmanDirection == Direction.Right)
            {
                if (targetX + 2 < 28 || targetY == 14) {
                    targetX += 2;
                }
                if (targetY == 14 && targetX > 27)
                {
                    targetX = 0;
                }
            }
            else if (pacmanDirection == Direction.Left)
            {
                if (targetX - 2 >= 0 || targetY == 14) {
                    targetX -= 2;
                }
                targetX -= 2;
                if (targetY == 14 && targetX < 0)
                {
                    targetX = 27;
                }
            }

            if (isTileMoveable(targetX, targetY, tileArray)) {
                return (targetX, targetY);
            }

            return (pacmanPosition.Item1, pacmanPosition.Item2);
        }

        public override void updateDirection() {
            if (currentDirection == Direction.Down)
            {
                currentGhost = new Rectangle(1659, 195, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentGhost = new Rectangle(1563, 195, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentGhost = new Rectangle(1371, 195, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentGhost = new Rectangle(1467, 195, 42, 42);
            }
        }
    }
}