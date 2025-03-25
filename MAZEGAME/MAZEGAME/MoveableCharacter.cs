using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MAZEGAME
{
    public class MoveableCharacter
    {
        public Vector2 calculateBasedOnDirection(Direction nextDirection, int currentX, int currentY)
        {
            int toBeX = currentX;
            int toBeY = currentY;

            if (nextDirection == Direction.Down)
            {
                toBeY += 1;
            }
            else if (nextDirection == Direction.Up)
            {
                toBeY -= 1;
            }
            else if (nextDirection == Direction.Right)
            {
                toBeX += 1;
                if (toBeY == 14 && toBeX > 27)
                {
                    toBeX = 0;
                }
            }
            else if (nextDirection == Direction.Left)
            {
                toBeX -= 1;
                if (toBeY == 14 && toBeX < 0)
                {
                    toBeX = 27;
                }
            }

            return new Vector2(toBeX, toBeY);
        }

        public bool isTileMoveable(int x, int y, Tile[,] tileArray) {
            if (x > 27 || x < 0 || y < 0 || y > 30) {
                return false;
            }
            return tileArray[x, y].tileType != Tile.TileType.Wall && tileArray[x, y].tileType != Tile.TileType.GhostHouse;
        }

        public int manhattanDistance((int, int) a, (int, int) b)
        {
            return Math.Abs( a.Item1 - b.Item2) + Math.Abs(a.Item1 - b.Item2);
        }

        public (int, int) findPath((int, int) startPosition, (int, int) targetPosition, Tile[,] tileArray, Direction currentDirection) {
            int rows = tileArray.GetLength(0);
            int cols = tileArray.GetLength(1);
            var openSet = new SortedSet<(int, (int, int))>();
            openSet.Add((0, startPosition));
            var cameFrom = new Dictionary<(int, int), (int, int)>();
            var gScore = new Dictionary<(int, int), int> { [startPosition] = 0 };
            var fScore = new Dictionary<(int, int), int> { [startPosition] = manhattanDistance(startPosition, targetPosition) };
            
            while (openSet.Count > 0)
            {
                var current = openSet.Min;
                openSet.Remove(current);

                if (current.Item2 == targetPosition)
                {
                    var path = new List<(int, int)>();
                    while (cameFrom.ContainsKey(current.Item2))
                    {
                        path.Add(current.Item2);
                        current.Item2 = cameFrom[current.Item2];
                    }
                    path.Add(startPosition);
                    path.Reverse();
                    if (path.Count <= 1){
                        return path[0];
                    }
                    return path[1];
                }

                foreach (var dir in Enum.GetValues<Direction>())
                {
                    if (current.Item2 != startPosition || dir != getOppositeDirection(currentDirection)) {
                        var neighborVector = calculateBasedOnDirection(dir, current.Item2.Item1, current.Item2.Item2);
                        var neighbor = ((int) neighborVector.X, (int) neighborVector.Y);
                        if (neighbor.Item1 >= 0 && neighbor.Item1 < rows && neighbor.Item2 >= 0 && neighbor.Item2 < cols && isTileMoveable(neighbor.Item1, neighbor.Item2, tileArray))
                        {
                            int tentativeGScore = gScore[current.Item2] + 1;

                            if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                            {
                                cameFrom[neighbor] = current.Item2;
                                gScore[neighbor] = tentativeGScore;
                                int fScoreNeighbor = tentativeGScore + manhattanDistance(neighbor, targetPosition);
                                fScore[neighbor] = fScoreNeighbor;
                                openSet.Add((fScoreNeighbor, neighbor));
                            }
                        }
                    }
                }
            }
        return startPosition;
        }

        public Direction getOppositeDirection(Direction currentDir)
        {
            switch (currentDir)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Up;
                default:
                    return Direction.Down;
            }
        }
    }
}