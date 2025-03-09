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

        public Enemy(int x, int y, Direction direction, Rectangle sprite) {
            positionX = x;
            positionY = y;
            currentDirection = direction;
            currentGhost = sprite;
            currentMode = EnemyMode.Scatter;
        }

        public abstract void updateEnemy(Tile[,] tileArray);

        public int getX() {
            return positionX;
        }

        public int getY() {
            return positionY;
        }

        public Rectangle getCurrentGhost() {
            return currentGhost;
        }

        public void frighten() {
            previousModeBeforeFrightened = currentMode;
            currentMode = EnemyMode.Frightened;
        }

        public void changeMode() {
            if (currentMode == EnemyMode.Chase) {
                currentMode = EnemyMode.Scatter;
            } else {
                currentMode = EnemyMode.Chase;
            }
        }
    }
}