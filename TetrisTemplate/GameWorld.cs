using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    public GameWorld(TetrisGame parent)
    {
        this.parent = parent;
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        StartScreen = TetrisGame.ContentManager.Load<Texture2D>("StartScreen");
        GameOverSinglePlayer = TetrisGame.ContentManager.Load<Texture2D>("GameOver1Player");
        Player1Wins = TetrisGame.ContentManager.Load<Texture2D>("GameOverPlayer1");
        Player2Wins = TetrisGame.ContentManager.Load<Texture2D>("GameOverPlayer2");
        Tie = TetrisGame.ContentManager.Load<Texture2D>("GameOverTie");
        grid1 = new TetrisGrid();
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
                }
                else if (inputHelper.KeyPressed(Keys.Enter))
                {
                    gameState = GameState.HighScoreMode;
                    grid1.Reset();
                    if (grid2 == null)
                        grid2 = new TetrisGrid();
                    else
                        grid2.Reset();
                    grid1.BeginPosition = new Vector2(000, 0);
                    grid2.BeginPosition = new Vector2(510, 0);
                    parent.SetScreenSize(1020, 600);
                }
                else if (inputHelper.KeyPressed(Keys.LeftShift))
                {
                    gameState = GameState.PlayingVSMode;
                    grid1.Reset();
                    if (grid2 == null)
                        grid2 = new TetrisGrid();
                    else
                        grid2.Reset();
                    grid1.BeginPosition = new Vector2(000, 0);
                    grid2.BeginPosition = new Vector2(510, 0);
                    parent.SetScreenSize(1020, 600);
                }
                break;
            case GameState.PlayingVSMode:
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
                    gameState = GameState.GameOverSinglePlayer;
                break;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        switch (gameState)
        {
            case GameState.StartScreen:
                spriteBatch.Draw(StartScreen, new Vector2(-30, 0), Color.White);
                break;
            case GameState.PlayingVSMode:
            case GameState.HighScoreMode:
                grid2.Draw(gameTime, spriteBatch);
                goto case GameState.PlayingSinglePlayer;
            case GameState.PlayingSinglePlayer:
                grid1.Draw(gameTime, spriteBatch);
                break;
            case GameState.GameOverSinglePlayer:
                spriteBatch.Draw(GameOverSinglePlayer, new Vector2(-30, 0), Color.White);
                spriteBatch.DrawString(font, "Score : " + grid1.Score.ToString(), new Vector2(TetrisGame.ScreenSize.X / 2 - 115, TetrisGame.ScreenSize.Y / 2 + 200 ),  Color.Blue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                break;
            case GameState.Player1Wins:
                spriteBatch.Draw(Player1Wins, new Vector2(-30,0), Color.White);
                break;
            case GameState.Player2Wins:
                spriteBatch.Draw(Player2Wins, new Vector2(-30, 0), Color.White);
                break;
            case GameState.Tie:
                spriteBatch.Draw(Tie, new Vector2(-30, 0), Color.White);
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
