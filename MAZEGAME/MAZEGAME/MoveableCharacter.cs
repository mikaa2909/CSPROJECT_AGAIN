using Microsoft.Xna.Framework;

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
    }
}