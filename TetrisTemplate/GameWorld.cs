using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    public enum GameState 
    {
        StartScreen,
        PlayingSinglePlayer,
        PlayingVSMode,
        HighScoreMode,
        GameOverSinglePlayer,
        Player1Wins,
        Player2Wins,
        Tie
    }
    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get; } = new Random();
    /// <summary>
    /// The main font of the game.
    /// </summary>
    public static SpriteFont font;

    /// <summery>
    /// Loads the sprites for the gamestates where the player is not playing.
    /// </summery>
    public static Texture2D StartScreen;
    public static Texture2D GameOverSinglePlayer;
    public static Texture2D Player1Wins;
    public static Texture2D Player2Wins;
    public static Texture2D Tie;
    public static Texture2D SideBar;
    public static Texture2D Bomb;
    public static SoundEffect BlockPlaced;
    public static SoundEffect RowComplete;
    public static SoundEffect LevelUp;

    /// <summary>
    /// The current game state.
    /// </summary>
    public GameState gameState { get; private set; } = GameState.StartScreen;

    /// <summary>
    /// A reference to the parent TetrisGame object.
    /// </summary>
    TetrisGame parent;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid1;
    TetrisGrid grid2;

    /// <summary>
    /// Creates the sidebar objects.
    /// </summary>
    VSSideBar VSSideBarPlayer1;
    VSSideBar VSSideBarPlayer2;

    public GameWorld(TetrisGame parent)
    {
        this.parent = parent;
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        StartScreen = TetrisGame.ContentManager.Load<Texture2D>("StartScreen");
        GameOverSinglePlayer = TetrisGame.ContentManager.Load<Texture2D>("GameOver1Player");
        Player1Wins = TetrisGame.ContentManager.Load<Texture2D>("GameOverPlayer1");
        Player2Wins = TetrisGame.ContentManager.Load<Texture2D>("GameOverPlayer2");
        Tie = TetrisGame.ContentManager.Load<Texture2D>("GameOverTie");
        SideBar = TetrisGame.ContentManager.Load<Texture2D>("SideBar");
        Bomb = TetrisGame.ContentManager.Load<Texture2D>("Bomb");
        BlockPlaced = TetrisGame.ContentManager.Load<SoundEffect>("BlockPlaced");
        RowComplete = TetrisGame.ContentManager.Load<SoundEffect>("RowComplete");
        LevelUp = TetrisGame.ContentManager.Load<SoundEffect>("LevelUp");
        grid1 = new TetrisGrid();
        grid2 = new TetrisGrid();
        VSSideBarPlayer1 = new VSSideBar(this, grid1, grid2);
        VSSideBarPlayer2 = new VSSideBar(this, grid2, grid1);
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        if (gameState != GameState.StartScreen && inputHelper.KeyPressed(Keys.Escape))
            Reset();
        switch (gameState)
        {
            case GameState.StartScreen:
                if (inputHelper.KeyPressed(Keys.Space))
                {
                    gameState = GameState.PlayingSinglePlayer;
                    grid1.Reset();
                    parent.SetScreenSize(480, 600);
                }
                else if (inputHelper.KeyPressed(Keys.Enter))
                {
                    gameState = GameState.HighScoreMode;
                    grid1.Reset();
                    grid2.Reset();
                    grid2.BeginPosition = new Vector2(480, 0);
                    parent.SetScreenSize(960, 600);
                }
                else if (inputHelper.KeyPressed(Keys.LeftShift))
                {
                    gameState = GameState.PlayingVSMode;
                    grid1.Reset();
                    grid2.Reset();
                    grid2.BeginPosition = new Vector2(480, 0);
                    parent.SetScreenSize(960, 600);
                }
                break;
            case GameState.PlayingVSMode:
                VSSideBarPlayer1.HandleInput(gameTime, inputHelper, Keys.Z, Keys.X, Keys.C);
                VSSideBarPlayer2.HandleInput(gameTime, inputHelper, Keys.M, Keys.OemComma, Keys.OemPeriod);
                goto case GameState.HighScoreMode;
            case GameState.HighScoreMode:
                if(!grid2.IsDead)
                    grid2.HandleInput(gameTime, inputHelper, Keys.J, Keys.L, Keys.O, Keys.U, Keys.I, Keys.K);
                goto case GameState.PlayingSinglePlayer;
            case GameState.PlayingSinglePlayer:
                if(!grid1.IsDead)
                    grid1.HandleInput(gameTime, inputHelper, Keys.A, Keys.D, Keys.E, Keys.Q, Keys.W, Keys.S);
                break;
        }
    }

    public void Update(GameTime gameTime)
    {
        switch(gameState)
        {
            case GameState.PlayingVSMode:
                if (!grid2.IsDead)
                    grid2.Update(gameTime);
                else
                    gameState = GameState.Player1Wins;
                if (grid1.IsDead)
                    gameState = GameState.Player2Wins;
                goto case GameState.PlayingSinglePlayer;
            case GameState.HighScoreMode:
                if (!grid2.IsDead)
                    grid2.Update(gameTime);
                else if (grid2.IsDead && grid1.IsDead)
                {
                    if (grid2.Score < grid1.Score)
                        gameState = GameState.Player1Wins;
                    else if (grid1.Score < grid2.Score)
                        gameState = GameState.Player2Wins;
                    else
                        gameState = GameState.Tie;
                }
                goto case GameState.PlayingSinglePlayer;
            case GameState.PlayingSinglePlayer:
                if (!grid1.IsDead)
                    grid1.Update(gameTime);
                else if (gameState == GameState.PlayingSinglePlayer)
                {
                    gameState = GameState.GameOverSinglePlayer;
                    parent.SetScreenSize(800, 600);
                }
                    break;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        switch (gameState)
        {
            case GameState.StartScreen:
                spriteBatch.Draw(StartScreen, Vector2.Zero, Color.White);
                break;
            case GameState.PlayingVSMode:
                VSSideBarPlayer1.Draw(gameTime, spriteBatch, "Z", "X", "C");
                VSSideBarPlayer2.Draw(gameTime, spriteBatch, "M", "<,", ">.");
                grid2.Draw(gameTime, spriteBatch);
                grid1.Draw(gameTime, spriteBatch);
                break;
            case GameState.HighScoreMode:
                VSSideBarPlayer2.Draw(gameTime, spriteBatch);
                grid2.Draw(gameTime, spriteBatch);
                goto case GameState.PlayingSinglePlayer;
            case GameState.PlayingSinglePlayer:
                VSSideBarPlayer1.Draw(gameTime, spriteBatch);
                grid1.Draw(gameTime, spriteBatch);
                break;
            case GameState.GameOverSinglePlayer:
                spriteBatch.Draw(GameOverSinglePlayer, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, "Score : " + grid1.Score.ToString(), new Vector2(TetrisGame.ScreenSize.X / 2 - 115, TetrisGame.ScreenSize.Y / 2 + 200 ),  Color.Blue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                break;
            case GameState.Player1Wins:
                spriteBatch.Draw(Player1Wins, new Vector2(-60,0), Color.White);
                break;
            case GameState.Player2Wins:
                spriteBatch.Draw(Player2Wins, new Vector2(-60, 0), Color.White);
                break;
            case GameState.Tie:
                spriteBatch.Draw(Tie, new Vector2(-60, 0), Color.White);
                break;
        }
        spriteBatch.End();
    }

    public void Reset()
    {
        gameState = GameState.StartScreen;
        parent.SetScreenSize(800, 600);
    }

}
