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
        PlayingVS,
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

    /// <summary>
    /// The current game state.
    /// </summary>
    public GameState gameState { get; private set; } = GameState.PlayingSinglePlayer;

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
                else if (inputHelper.KeyPressed(Keys.LeftShift))
                {
                    gameState = GameState.PlayingVS;
                    grid1.Reset();
                    if (grid2 == null)
                        grid2 = new TetrisGrid();
                    else
                        grid2.Reset();
                    grid1.BeginPosition = new Vector2(000, 0);
                    grid2.BeginPosition = new Vector2(540, 0);
                    parent.SetScreenSize(1080, 600);
                }
                break;
            case GameState.PlayingVS:
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
            case GameState.PlayingVS:
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
                spriteBatch.DrawString(font, "Press Spacebar to play solo.", new Vector2(TetrisGame.ScreenSize.X / 2 - 100, TetrisGame.ScreenSize.Y / 2 - 30), Color.Blue);
                spriteBatch.DrawString(font, "Press Shift to play VS mode.", new Vector2(TetrisGame.ScreenSize.X / 2 - 100, TetrisGame.ScreenSize.Y / 2), Color.Blue);
                break;
            case GameState.PlayingVS:
                grid2.Draw(gameTime, spriteBatch);
                goto case GameState.PlayingSinglePlayer;
            case GameState.PlayingSinglePlayer:
                grid1.Draw(gameTime, spriteBatch);
                break;
            case GameState.GameOverSinglePlayer:
                spriteBatch.DrawString(font, "Game over!", new Vector2(TetrisGame.ScreenSize.X / 2 - 30, TetrisGame.ScreenSize.Y / 2 - 30), Color.Blue);
                spriteBatch.DrawString(font, "Score : " + grid1.Score.ToString(), new Vector2(TetrisGame.ScreenSize.X / 2 - 19, TetrisGame.ScreenSize.Y / 2 ), Color.Blue);
                break;
            case GameState.Player1Wins:
                spriteBatch.DrawString(font, "Player 1 Wins!!!", new Vector2(TetrisGame.ScreenSize.X / 2 - 57, TetrisGame.ScreenSize.Y / 2 - 15), Color.Blue);
                break;
            case GameState.Player2Wins:
                spriteBatch.DrawString(font, "Player 2 Wins!!!", new Vector2(TetrisGame.ScreenSize.X / 2 - 57, TetrisGame.ScreenSize.Y / 2 - 15), Color.Blue);
                break;
            case GameState.Tie:
                spriteBatch.DrawString(font, "It's a Tie!", new Vector2(TetrisGame.ScreenSize.X / 2 - 32, TetrisGame.ScreenSize.Y / 2 - 15), Color.Blue);
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
