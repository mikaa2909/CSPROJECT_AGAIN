using System;
using System.Collections.Generic;
using System.Linq;
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
    private int pelletsEaten;
    private int score;
    private int countdownNo;
    private bool gameStarted;
    private bool gameOver;
    private bool playerWon;
    private bool doResetPositions = false;
    private Tile[,] tileArray = new Tile[28, 31];
    private float currentTimer = 0f; 
    private float frightenTimer  = 0f;
    private float blueReleaseTimer = 0f;
    private float orangeReleaseTimer = 0f;
    private Enemy.EnemyMode currentEnemyMode;
    private Enemy.EnemyMode previousEnemyMode;
    private int waveNo;
    private int currentLevel;
    private List<List<float>> waveIntervalsPerLevel = new List<List<float>>();

    private int[,] mapDesign = new int[,]{
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0},
            { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
            { 0,2,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,2,0},
            { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
            { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0},
            { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0},
            { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0},
            { 0,0,0,0,0,0,1,0,0,0,0,0,4,0,0,4,0,0,0,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,0,0,0,4,0,0,4,0,0,0,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,4,4,4,4,4,4,4,4,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,0,0,0,3,3,0,0,0,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,0,3,3,3,3,3,3,0,4,0,0,1,0,0,0,0,0,0},
            { 1,1,1,1,1,1,1,4,4,4,0,3,3,3,3,3,3,0,4,4,4,1,1,1,1,1,1,1},
            { 0,0,0,0,0,0,1,0,0,4,0,3,3,3,3,3,3,0,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,0,0,0,0,0,0,0,0,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,4,4,4,4,4,4,4,4,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,0,0,0,0,0,0,0,0,4,0,0,1,0,0,0,0,0,0},
            { 0,0,0,0,0,0,1,0,0,4,0,0,0,0,0,0,0,0,4,0,0,1,0,0,0,0,0,0},
            { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0},
            { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
            { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
            { 0,2,1,1,0,0,1,1,1,1,1,1,1,4,4,1,1,1,1,1,1,1,0,0,1,1,2,0},
            { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0},
            { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0},
            { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0},
            { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0},
            { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0},
            { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}

        };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Initialise game state
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
        playerWon = false;
        gameOver = false;
        countdownNo = 3;
        currentEnemyMode = Enemy.EnemyMode.Scatter;
        previousEnemyMode = Enemy.EnemyMode.Scatter;
        waveNo = 0;
        currentLevel = 1;
        waveIntervalsPerLevel.Add([7f, 20f, 7f, 20f, 7f, 20f, 7f]); // level 1
        waveIntervalsPerLevel.Add([7f, 20f, 7f, 20f, 3f, 22f, 3f]); // level 2
        waveIntervalsPerLevel.Add([7f, 20f, 3f, 22f, 2f, 23f, 1f]); // level 3
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

        // Make the beginning countdown change number after every one second and then start game
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
        
        // Where the main logic of the game happens once it is underway 
        if (gameStarted && !gameOver) {

            // If the game is in a state to reset, reset all the positions to the original settings
            if (doResetPositions) {
                resetPositions();
            }

            changeMode(gameTime);

            // Register the press of a key, to indicate the next direction the player wants to take
            KeyboardState kState = Keyboard.GetState();
            pacman.changeDirection(kState);

            eatPellets();
            checkToReleaseBlue(gameTime);
            checkToReleaseOrange(gameTime);
            
            // If all pellets are consumed, the next level begins and the map is reset
            // If the third level is complete, the game is over and the player has won
            if (noOfPellets == 0) {
                currentLevel ++;
                if (currentLevel == 4) {
                    gameOver = true;
                    playerWon = true;
                    return;
                }
                noOfPellets = 256;
                SetUpTileArray(); 
                resetPositions();
                return;
            }

            checkIfEnemyPlayerCollision();
             
            // Don't update pacman every update but every couple of updates (makes the animation slower)
            if (timeElapsed == 10) 
            {
                if (!doResetPositions) {
                    // Move the players and enemies
                    pacman.updatePlayer(tileArray);
                    foreach (Enemy enemy in enemies) {
                        enemy.updateEnemy(tileArray, (pacman.getX(), pacman.getY()), pacman.getCurrentDirection(), (enemies[1].getX(), enemies[1].getY()));
                    }
                    timeElapsed = 0;
                }
            }

            timeElapsed += 1;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        // Draw the maze
        _spriteBatch.Draw(sprites, new Vector2(5, 32), new Rectangle(684,0,672,744), Color.White);
        drawScore();
        drawPellets();

        // Draw the enemies
        foreach (Enemy enemy in enemies) {
            _spriteBatch.Draw(sprites, tileArray[enemy.getX(), enemy.getY()].getPosition(), enemy.getCurrentGhost(), Color.White);
        }

        // Draw the player
        _spriteBatch.Draw(sprites, tileArray[pacman.getX(), pacman.getY()].getPosition(), pacman.getCurrentPacman(), Color.White);
        drawLives();

        // Draw the countdown
        if (!gameStarted) {
            _spriteBatch.Draw(font, new Vector2(327, 441), Numbers.getRectangle(countdownNo.ToString()[0]), Color.White);
        }

        // Draw text when game has finished
        if (gameOver) {
            if (playerWon) {
                drawGameWon();
            } else {
                drawGameOver();
            }
        }
        _spriteBatch.End();

        base.Draw(gameTime);

    }

    // If the player is on a tile with a pellet, the player eats the pellet
    private void eatPellets() {
        Tile currentPacmanTile = tileArray[pacman.getX(), pacman.getY()];
        if (currentPacmanTile.tileType == Tile.TileType.Pellet) {
            // If the player is on a tile with a pellet, remove it from the tile and increase the score
            tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentPacmanTile.position.X, currentPacmanTile.position.Y), Tile.TileType.None);
            noOfPellets --;
            pelletsEaten++;
            score += 10;
        } else if (currentPacmanTile.tileType == Tile.TileType.PowerPellet) {
            // If the player is on a tile with a power pellet, remove it from the tile, increase the score
            // and all enemies should enter frightened mode, the frighten timer now begins/resets
            tileArray[pacman.getX(), pacman.getY()] = new Tile(new Vector2(currentPacmanTile.position.X, currentPacmanTile.position.Y), Tile.TileType.None);
            noOfPellets --;
            pelletsEaten++;
            score += 10;
            foreach (Enemy enemy in enemies) {
                enemy.frighten();
            }
            frightenTimer = 0f;
            currentEnemyMode = Enemy.EnemyMode.Frightened;
        }
    }

    private void checkIfEnemyPlayerCollision() {
        // Check if the enemy and the player have collided
        foreach (Enemy enemy in enemies) {
            if (checkIfCollision(enemy)) {
                if (enemy.getMode() == Enemy.EnemyMode.Frightened) { 
                    // If the player does collide with the enemy when the enemies are in frightened mode, 
                    // the enemy is sent to its inital positions and the score increases by 200
                    score += 200;
                    enemy.setInitalState();
                } else {
                    // If the player collides with the enemy in normal modes, the player loses a life
                    // and the position of the player and enemies are reset.
                    // If all lives are lost, the game is over
                    pacman.decreaseLife();
                    if (pacman.getLivesLeft() == 0) {
                        gameOver = true;
                        return;
                    }
                    doResetPositions = true;
                    break;
                }
            }  
        }
    }

    // Changes mode if the associated timer is up
    private void changeMode(GameTime gameTime) {
        // Logic to change the mode of the enemies after certain intervals determined by the intervals in waveIntervalsPerLevel 
        // (only changes when a enemy is not in frightened mode and the wave number is below 7 which is the highest)
        if (currentEnemyMode != Enemy.EnemyMode.Frightened && waveNo < 7) {
            currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // If the interval has passed then the enemies all have their modes changed 
            // The interval is dependant on the current wave that is happening and the waves are dependant on the current level 
            if (currentTimer >= waveIntervalsPerLevel[currentLevel-1][waveNo]) {
                currentTimer = 0;
                
                //Switch between chasing and scattering
                Enemy.EnemyMode changeTo = currentEnemyMode == Enemy.EnemyMode.Scatter ? Enemy.EnemyMode.Chase : Enemy.EnemyMode.Scatter;
                foreach (Enemy enemy in enemies) {
                    enemy.changeMode(changeTo);
                }
                currentEnemyMode = changeTo;
            }
        } else if (currentEnemyMode == Enemy.EnemyMode.Frightened) {
            // If the enemies are in frightened mode, the frightened timer is increased
            frightenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frightenTimer > 6f && frightenTimer < 8f) { 
                // When 6 seconds is over, the enemies should turn from blue to white to indicate the frightened time is coming
                // to an end
                foreach (Enemy enemy in enemies) {
                    enemy.setWhiteGhost();
                }
            } else if (frightenTimer > 8f) { 
                // When 8 seconds are over, enemies should return to the previous mode and timer reset
                currentEnemyMode = previousEnemyMode;
                foreach (Enemy enemy in enemies) {
                    enemy.endFrightenedMode();
                }
                frightenTimer = 0f;
            }
        }
    }

    // Checks if the player and enemy has collided. It does this by using the concept of 
    // axis aligned bounding boxes.
    private bool checkIfCollision(Enemy enemy) {
        int rows = tileArray.GetLength(0);
        int cols = tileArray.GetLength(1);

        // Get all the direction around an object
        (int, int)[] directions = [(-1, 0), (0, -1), (0, 0), (0, 1), (1, 0)];
        List<(int, int)> enemyPositions = new List<(int, int)>();
        List<(int, int)> playerPositions = new List<(int, int)>();

        int enemyPositionX = enemy.getX();
        int enemyPositionY = enemy.getY(); 
        int playerPositionX = pacman.getX();
        int playerPositionY = pacman.getY();

        // Get all the possible positions in the different directions around both objexts (the bounding boxes)
        foreach ((int, int) offset in directions) {
            int enemyX = enemyPositionX + offset.Item1;
            int enemyY = enemyPositionY + offset.Item2;

             // If the posiition is valid then add it to the possible options list
            if (enemyX >= 0 && enemyX < rows && enemyY >= 0 && enemyY < cols && enemy.isTileMoveable(enemyX, enemyY, tileArray)) {
                enemyPositions.Add((enemyX, enemyY));
            }
            
            int playerX = playerPositionX + offset.Item1;
            int playerY = playerPositionY + offset.Item2;

            if (playerX >= 0 && playerX < rows && playerY >= 0 && playerY < cols && pacman.isTileMoveable(playerX, playerY, tileArray)) {
                playerPositions.Add((playerX, playerY));
            }
        }

        // If any of the positions around are the same then the objects have collided
        return enemyPositions.Intersect(playerPositions).Any();
    }

    // Method to check whether to release the blue ghost, if so then release it
    private void checkToReleaseBlue(GameTime gameTime) {
        blueReleaseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (enemies[3].getMode() == Enemy.EnemyMode.InsideHouse) {

            // On the first level, if more than 30 pellets are eaten then release
            if (pelletsEaten == 30) {
                enemies[3].leaveHouse(currentEnemyMode);
            } else if (pelletsEaten > 30) {
                // If the blue ghost has already been released before but has returned to the house
                // then release after 15 seconds after it returned home.
                if (blueReleaseTimer > 15f) {
                   enemies[3].leaveHouse(currentEnemyMode); 
                }
            }
        }
    }

    // Method to check whether to release the orange ghost, if so then release it
    private void checkToReleaseOrange(GameTime gameTime) {
        orangeReleaseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // On the first level, if more than 85 pellets are eaten then release
        if (enemies[0].getMode() == Enemy.EnemyMode.InsideHouse) {
            if (pelletsEaten == 85) {
                enemies[0].leaveHouse(currentEnemyMode);
            } else if (pelletsEaten > 85) {
                // If the orange ghost has already been released before but has returned to the house
                // then release after 25 seconds after it returned home.
                if (orangeReleaseTimer > 25f) {
                   enemies[0].leaveHouse(currentEnemyMode); 
                }
            }
        }
    }

    private void SetUpTileArray() 
    {
        // Create the tile array based on the design of the map, where different integers represent 
        // different tile types
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (mapDesign[y, x] == 0) // wall
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Wall);
                }
                else if (mapDesign[y, x] == 1) // pellet
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Pellet);
                }
                else if (mapDesign[y, x] == 2) // power pellet
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.PowerPellet);
                }
                else if (mapDesign[y, x] == 3) // enemy house 
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.GhostHouse);
                }
                else if (mapDesign[y, x] == 4) // empty
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.None);
                }
            }
        }
    }

    private void drawPellets() {
        // Draws all pellets across the map (including power pellets in their respective corners)
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
        _spriteBatch.Draw(font, new Vector2(5, 4), new Rectangle(75, 24, 21, 21), Color.White); // S
        _spriteBatch.Draw(font, new Vector2(28, 4), new Rectangle(51, 0, 21, 21), Color.White); // C
        _spriteBatch.Draw(font, new Vector2(51, 4), new Rectangle(339, 0, 21, 21), Color.White); // O
        _spriteBatch.Draw(font, new Vector2(74, 4), new Rectangle(51, 24, 21, 21), Color.White); // R
        _spriteBatch.Draw(font, new Vector2(97, 4), new Rectangle(99, 0, 21, 21), Color.White); // E
        _spriteBatch.Draw(font, new Vector2(120, 4), new Rectangle(267, 48, 21, 21), Color.White); // :
        drawNumbers();
    }

    private void drawNumbers() {
        // converts the integer score to a displayable score in the game
        String scoreString = score.ToString();
        int xPosition = 143;
        for (int i=0; i<scoreString.Length; i++) {
            _spriteBatch.Draw(font, new Vector2(xPosition, 4), Numbers.getRectangle(scoreString[i]), Color.White);
            xPosition += 23;
        }
    }

    private void drawLives() {
        // Draws the lives the player has left
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

    private void drawGameWon() {

        _spriteBatch.Draw(font, new Vector2(253, 370), new Rectangle(219, 24, 21, 21), Color.White); // Y
        _spriteBatch.Draw(font, new Vector2(276, 370), new Rectangle(339, 0, 21, 21), Color.White); // O
        _spriteBatch.Draw(font, new Vector2(299, 370), new Rectangle(123, 24, 21, 21), Color.White); // U

        _spriteBatch.Draw(font, new Vector2(338, 370), new Rectangle(171, 24, 21, 21), Color.White); // W
        _spriteBatch.Draw(font, new Vector2(361, 370), new Rectangle(339, 0, 21, 21), Color.White); // O
        _spriteBatch.Draw(font, new Vector2(384, 370), new Rectangle(315, 0, 21, 21), Color.White); // N
        _spriteBatch.Draw(font, new Vector2(407, 370), new Rectangle(267, 24, 21, 21), Color.White); // !
    }

    private void resetPositions() {
        // Resets variables of the game to inital values
        gameStarted = false;
        countdownNo = 3;
        currentTimer = 0;
        blueReleaseTimer = 0f;
        orangeReleaseTimer = 0f;
        pacman.setInitalState();
        foreach (Enemy enemy in enemies) {
            enemy.setInitalState();
        }
        doResetPositions = false;

        // Pause before starting again
        System.Threading.Thread.Sleep(2000);
    }
}
