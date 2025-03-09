using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MAZEGAME
{
    public class Tile
    {
        public enum TileType { None, Wall, Ghost, GhostHouse, Player, Pellet, PowerPellet};
        public TileType tileType;
        public Vector2 position;

        public Tile(Vector2 position, TileType tileType)
        {
            this.tileType = tileType;
            this.position = position;
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}