using System;
using System.Collections.Generic;
using System.Timers;
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
    private List<Enemy> enemies = new List<Enemy>();

    private int timeElapsed;
    private int tileWidth;
    private int tileHeight;
    private int noOfPellets;
    private int score;
    private int countdownNo;
    private bool gameStarted;
    private bool gameOver;
    private Tile[,] tileArray = new Tile[28, 31];
    private float currentTimer = 0f; 
    private float frightenTimer  = 0f;
    private Enemy.EnemyMode currentEnemyMode;
    private Enemy.EnemyMode previousEnemyMode;
    private int waveNo;
    private float[] waveOneIntervals;

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
        _graphics.PreferredBackBufferHeight = 850;
        _graphics.ApplyChanges();

        tileWidth = 680 / 28;
        tileHeight = (780 + 27) / 31;
        pacman = new Pacman();
        enemies.Add(new OrangeEnemy());
        enemies.Add(new RedEnemy());
        enemies.Add(new PinkEnemy());
        enemies.Add(new BlueEnemy());
        timeElapsed = 0;
        noOfPellets = 256;
        score = 0;
        gameStarted = false;
        gameOver = false;
        countdownNo = 3;
        currentEnemyMode = Enemy.EnemyMode.Scatter;
        previousEnemyMode = Enemy.EnemyMode.Scatter;
        waveNo = 0;
        waveOneIntervals = [7f, 20f, 7f, 20f, 7f, 20f, 7f];
        
        base.Initialize();
    }

    protected override void LoadContent()
    {   
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sprites = Content.Load<Texture2D>("spriteSheet");
        font = Content.Load<Texture2D>("font");
        SetUpTileArray();
    }

    protected override void Update(GameTime gameTime)
    {   
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

        if (!gameStarted) {
            currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTimer >= 1f) {
                countdownNo -= 1;
                currentTimer = 0;
                if (countdownNo < 1) {
                    gameStarted = true;
                }
            }
        }
        
        if (gameStarted && !gameOver) {
            if (currentEnemyMode != Enemy.EnemyMode.Frightened && waveNo < 7) {
                currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTimer >= waveOneIntervals[waveNo]) {
                    currentTimer = 0;
                    foreach (Enemy enemy in enemies) {
                        currentEnemyMode = enemy.changeMode();
                    }
                }
            } else if (currentEnemyMode == Enemy.EnemyMode.Frightened) {
                frightenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frightenTimer > 6f && frightenTimer < 8f) {
                    foreach (Enemy enemy in enemies) {
                        enemy.setWhiteGhost();
                    }
                } else if (frightenTimer > 8f) {
                    currentEnemyMode = previousEnemyMode;
                    foreach (Enemy enemy in enemies) {
                        enemy.endFrightenedMode();
                    }
                    frightenTimer = 0f;
                }
            }
            KeyboardState kState = Keyboard.GetState();
            pacman.changeDirection(kState);

            Tile currentPacmanTile = tileArray[pacman.getX(), pacman.getY()];
            if (currentPacmanTile.tileType == Tile.TileType.Pellet) {
                tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentPacmanTile.position.X, currentPacmanTile.position.Y), Tile.TileType.None);
                noOfPellets -= 1;
                score += 10;
            } else if (currentPacmanTile.tileType == Tile.TileType.PowerPellet) {
                tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentPacmanTile.position.X, currentPacmanTile.position.Y), Tile.TileType.None);
                noOfPellets -= 1;
                score += 10;
                foreach (Enemy enemy in enemies) {
                    enemy.frighten();
                }
                frightenTimer = 0f;
                currentEnemyMode = Enemy.EnemyMode.Frightened;
            }
            foreach (Enemy enemy in enemies) {
                Tile currentEnemyTile = tileArray[enemy.getX(), enemy.getY()];
                if (currentPacmanTile.Equals(currentEnemyTile)) {
                    if (enemy.getMode() == Enemy.EnemyMode.Frightened) {
                        score += 200;
                        enemy.setInitalState();
                    } else {
                        pacman.decreaseLife();
                        if (pacman.getLivesLeft() == 0) {
                            gameOver = true;
                            return;
                        }
                        resetPositions();
                    }
                }  
            }
             
            // Don't update pacman every update but every three updates (makes the animation slower)
            if (timeElapsed == 8) 
            {
                pacman.updatePlayer(tileArray);
                foreach (Enemy enemy in enemies) {
                    enemy.updateEnemy(tileArray, (pacman.getX(), pacman.getY()), pacman.getCurrentDirection(), (enemies[1].getX(), enemies[1].getY()));
                }
                timeElapsed = 0;
            }

            timeElapsed += 1;
        }


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
        foreach (Enemy enemy in enemies) {
            _spriteBatch.Draw(sprites, tileArray[enemy.getX(), enemy.getY()].getPosition(), enemy.getCurrentGhost(), Color.White);
        }

        _spriteBatch.Draw(sprites, tileArray[pacman.getX(), pacman.getY()].getPosition(), pacman.getCurrentPacman(), Color.White);
        drawLives();

        if (!gameStarted) {
            _spriteBatch.Draw(font, new Vector2(327, 441), Numbers.getRectangle(countdownNo.ToString()[0]), Color.White);
        }

        if (gameOver) {
            drawGameOver();
        }
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

    private void drawLives() {
        int xPosition = 8;
        for (int i=0; i<pacman.getLivesLeft(); i++) {
            _spriteBatch.Draw(sprites, new Vector2(xPosition, 790), new Rectangle(1419, 3, 39, 39), Color.White);
            xPosition += 50;
        }
    }

    private void drawGameOver() {

        _spriteBatch.Draw(font, new Vector2(238, 370), new Rectangle(147, 96, 21, 21), Color.White); // G
        _spriteBatch.Draw(font, new Vector2(261, 370), new Rectangle(3, 96, 21, 21), Color.White); // A
        _spriteBatch.Draw(font, new Vector2(284, 370), new Rectangle(291, 96, 21, 21), Color.White); // M
        _spriteBatch.Draw(font, new Vector2(307, 370), new Rectangle(99, 96, 21, 21), Color.White); // E

        _spriteBatch.Draw(font, new Vector2(353, 370), new Rectangle(339, 96, 21, 21), Color.White); // O
        _spriteBatch.Draw(font, new Vector2(376, 370), new Rectangle(147, 120, 21, 21), Color.White); // V
        _spriteBatch.Draw(font, new Vector2(399, 370), new Rectangle(99, 96, 21, 21), Color.White); // E
        _spriteBatch.Draw(font, new Vector2(422, 370), new Rectangle(51, 120, 21, 21), Color.White); // R
    }

    private void resetPositions() {
        gameStarted = false;
        countdownNo = 3;
        currentTimer = 0;
        pacman.setInitalState();
        foreach (Enemy enemy in enemies) {
            enemy.setInitalState();
        }
        System.Threading.Thread.Sleep(2000);
    }
}
