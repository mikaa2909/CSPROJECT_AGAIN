using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MAZEGAME
{
    public abstract class Enemy : MoveableCharacter
    {

        public enum EnemyMode{Chase, Scatter, Frightened}

        public int positionX;
        public int positionY;
        public Direction currentDirection;
        public Rectangle currentGhost;
        public EnemyMode currentMode;
        public EnemyMode previousModeBeforeFrightened;
        public Rectangle frightenedEnemy;
        public bool isInGhostHouse;

        public Enemy() {
            setInitalState();
            frightenedEnemy = new Rectangle(1755, 195, 42, 42);
        }

        public abstract void setInitalState();

        public void updateEnemy(Tile[,] tileArray, (int, int) pacmanPosition, Direction pacmanDirection, (int, int) redEnemyPosition) {
            (int, int) targetPosition;
            (int, int) toBePosition;
            if (currentMode == EnemyMode.Chase) {
                targetPosition = getChaseTargetPosition(pacmanPosition, pacmanDirection, tileArray, redEnemyPosition);
                toBePosition = findPath((positionX, positionY), targetPosition, tileArray, currentDirection);
            } else if (currentMode == EnemyMode.Scatter){
                targetPosition = getScatterTargetPosition(tileArray);
                toBePosition = findPath((positionX, positionY), targetPosition, tileArray, currentDirection);
            } else {
                toBePosition = getFrightenedPosition(tileArray);
            }
            currentDirection = getDirectionBasedOnNextPosition(positionX, positionY, toBePosition.Item1, toBePosition.Item2);
            positionX = toBePosition.Item1;
            positionY = toBePosition.Item2;
            if (currentMode != EnemyMode.Frightened) {
                updateDirection();
            }
        }

        public int getX() {
            return positionX;
        }

        public int getY() {
            return positionY;
        }

        public Rectangle getCurrentGhost() {
            return currentGhost;
        }

        public EnemyMode getMode() {
            return currentMode;
        }

        public void frighten() {
            if (currentMode != EnemyMode.Frightened) {
                previousModeBeforeFrightened = currentMode;
            }
            currentMode = EnemyMode.Frightened;
            currentGhost = frightenedEnemy;
            currentDirection = getOppositeDirection(currentDirection);
        }

        public void setWhiteGhost() {
            if (currentMode == EnemyMode.Frightened) {
                currentGhost = new Rectangle(1851, 195, 42, 42);
            }
        }

        public void endFrightenedMode() {
            currentMode = previousModeBeforeFrightened;
        }

        public EnemyMode changeMode() {
            if (currentMode == EnemyMode.Chase) {
                currentMode = EnemyMode.Scatter;
            } else {
                currentMode = EnemyMode.Chase;
            }
            return currentMode;
        }

        public abstract (int, int) getScatterTargetPosition(Tile[,] tileArray);
        public abstract (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition);

        public abstract void updateDirection();

        public (int, int) getFrightenedPosition(Tile[,] tileArray) {
            List<(int, int)> possibleNextPositions = new List<(int, int)>();
            foreach (var dir in Enum.GetValues<Direction>()) {
                if (dir != getOppositeDirection(currentDirection)) {
                    Vector2 position = calculateBasedOnDirection(dir, positionX, positionY);
                    if (isTileMoveable((int)position.X, (int)position.Y, tileArray)) {
                        possibleNextPositions.Add(((int)position.X, (int)position.Y));
                    }
                }
            }

            Random random = new Random();
            int index = random.Next(possibleNextPositions.Count);
            return possibleNextPositions[index];
        }

        public Direction getDirectionBasedOnNextPosition(int currentX, int currentY, int toBeX, int toBeY) {
            var vector = (currentX - toBeX, currentY - toBeY);
            if (vector == (-1, 0)) {
                return Direction.Right;
            } else if (vector == (1, 0)) {
                return Direction.Left;
            } else if (vector == (0, -1)) {
                return Direction.Down;
            } else {
                return Direction.Up;
            }
        }
    }
}