using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MAZEGAME;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D sprites;
    private Texture2D font;
    private Pacman pacman;
    private Enemy enemy;

    private int timeElapsed;
    private int tileWidth;
    private int tileHeight;
    private int noOfPellets;
    private int score;
    private Tile[,] tileArray = new Tile[28, 31];

    private int[,] mapDesign = new int[,]{
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,3,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,2,2,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 0,0,0,0,0,0,0,5,5,5,1,2,2,2,2,2,2,1,5,5,5,0,0,0,0,0,0,0},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,0,0,1,1,0,0,0,0,0,0,0,5,5,0,0,0,0,0,0,0,1,1,0,0,3,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 680;
        _graphics.PreferredBackBufferHeight = 780;
        _graphics.ApplyChanges();

        tileWidth = 680 / 28;
        tileHeight = (780 + 27) / 31;
        pacman = new Pacman();
        enemy = new OrangeEnemy(13, 5, Direction.Right, new Rectangle(1371, 339, 42, 42));
        timeElapsed = 0;
        noOfPellets = 256;
        score = 0;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {   
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sprites = Content.Load<Texture2D>("spriteSheet");
        font = Content.Load<Texture2D>("font");

        // Different pacman mouth open/closed in all directions

        SetUpTileArray();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

        KeyboardState kState = Keyboard.GetState();
        pacman.changeDirection(kState);

        Tile currentTile = tileArray[pacman.getX(), pacman.getY()];
        if (currentTile.tileType == Tile.TileType.Pellet) {
            tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentTile.position.X, currentTile.position.Y), Tile.TileType.None);
            noOfPellets -= 1;
            score += 10;
        } else if (currentTile.tileType == Tile.TileType.PowerPellet) {
            tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentTile.position.X, currentTile.position.Y), Tile.TileType.None);
            noOfPellets -= 1;
            score += 10;
        }               

        // Don't update pacman every update but every three updates (makes the animation slower)
        if (timeElapsed == 8) 
        {
            pacman.updatePlayer(tileArray);
            enemy.updateEnemy(tileArray);
            timeElapsed = 0;
        }

        timeElapsed += 1;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        // Draw the maze
        _spriteBatch.Draw(sprites, new Vector2(5, 32), new Rectangle(684,0,672,744), Color.White);
        drawScore();

        drawSnacks();
        _spriteBatch.Draw(sprites, tileArray[enemy.getX(), enemy.getY()].getPosition(), enemy.getCurrentGhost(), Color.White);
        _spriteBatch.Draw(sprites, tileArray[pacman.getX(), pacman.getY()].getPosition(), pacman.getCurrentPacman(), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);

    }

    private void SetUpTileArray() 
    {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (mapDesign[y, x] == 0) // pellet
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Pellet);
                }
                else if (mapDesign[y, x] == 1) // wall
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Wall);
                }
                else if (mapDesign[y, x] == 2) //  ghost house
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.GhostHouse);
                }
                else if (mapDesign[y, x] == 3) // power pellet
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.PowerPellet);
                }
                else if (mapDesign[y, x] == 5) // empty
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.None);
                }
            }
        }
    }

    private void drawSnacks() {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (tileArray[x, y].tileType == Tile.TileType.Pellet) {
                    _spriteBatch.Draw(sprites, new Vector2(tileArray[x, y].getPosition().X + tileWidth / 2 - 3, tileArray[x, y].getPosition().Y + tileHeight / 2 - 3), new Rectangle(33, 33, 6, 6), Color.White);
                } else if (tileArray[x, y].tileType == Tile.TileType.PowerPellet) {
                    _spriteBatch.Draw(sprites, new Vector2(tileArray[x, y].getPosition().X, tileArray[x, y].getPosition().Y), new Rectangle(24, 72, 24, 24), Color.White);
                }
            }
        }
    }

    private void drawScore() {
        _spriteBatch.Draw(font, new Vector2(5, 4), new Rectangle(75, 24, 21, 21), Color.White);
        _spriteBatch.Draw(font, new Vector2(28, 4), new Rectangle(51, 0, 21, 21), Color.White);
        _spriteBatch.Draw(font, new Vector2(51, 4), new Rectangle(339, 0, 21, 21), Color.White);
        _spriteBatch.Draw(font, new Vector2(74, 4), new Rectangle(51, 24, 21, 21), Color.White);
        _spriteBatch.Draw(font, new Vector2(97, 4), new Rectangle(99, 0, 21, 21), Color.White);
        _spriteBatch.Draw(font, new Vector2(120, 4), new Rectangle(267, 48, 21, 21), Color.White);
        drawNumbers();
    }

    private void drawNumbers() {
        String scoreString = score.ToString();
        int xPosition = 143;
        for (int i=0; i<scoreString.Length; i++) {
            _spriteBatch.Draw(font, new Vector2(xPosition, 4), Numbers.getRectangle(scoreString[i]), Color.White);
            xPosition += 23;
        }
    }
}
