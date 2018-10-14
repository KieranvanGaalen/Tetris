using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/// <summary>
/// A class for representing a Tetris playing grid.
/// </summary>
class TetrisGrid
{
    public static Texture2D emptyCell; // The sprite of a single empty cell in the grid.
    public Vector2 BeginPosition = new Vector2(220, 0);// The position at which this TetrisGrid should be drawn. Also used to draw the collored blocks.
    public int Width { get { return 12; } } // The number of grid elements in the x-direction.
    public int Height { get { return 20; } } // The number of grid elements in the y-direction.
    Random random = new Random();
    public Color[,] grid = new Color[16, 22]; //De grid is groter dan wat getekend wordt want anders valt de blockgrid buiten de array. De array wordt 2 rechts, 2 links en 2 onder de grid uitgebreid
    TetrisBlock Block; //The block that is currently active gets stored here.
    public bool ForceBlockDownwards; //A boolian used for indicating that a block is being forced down.
    public int Score { get; private set; } = 0; //An interger used to keep track of the players score.
    private int level = 1; //An interger used to keep track of the players level.
    public bool IsDead = false; //When the blocks can no longer fall down this will be set to true and the game will be over.
    public double fallingSpeed { get; private set; } = 1; //The current falling speed of the Block.
    private TetrisBlock NextBlock;

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    public TetrisGrid()
    {
        Clear();
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        NextBlock = new Ipiece(this);
        NewBlock();
    }

    /// <summary>
    /// Update method for the TetrisGrid.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    public void Update(GameTime gameTime)
    {
        Block.Update(gameTime);
    }

    //HandleInput methode voor de TetrisGrid.
    /// <summary>
    /// Handles the player input.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    /// <param name="LeftMove">When this key is pressed the currently active block is (if possable) moved 1 block to the left in the grid.</param>
    /// <param name="RightMove">When this key is pressed the currently active block is (if possable) moved 1 block to the right in the grid.</param>
    /// <param name="RotateCW">When this key is pressed the currently active block is (if possable) rotated 90 degrees Clockwise.</param>
    /// <param name="RotateCCW">When this key is pressed the currently active block is (if possable) rotated 90 degrees CounterClockwise.</param>
    /// <param name="GoDown">While this key is pressed the currently active block will move down through the grid at an accelerated speed.</param>
    public void HandleInput(GameTime gameTime, InputHelper inputHelper, Keys LeftMove, Keys RightMove, Keys RotateCW, Keys RotateCCW, Keys Test, Keys GoDown)
    {
        //This part makes sure that the player can move the block to the left and right.
        if (inputHelper.KeyDown(RightMove) && gameTime.TotalGameTime.Ticks % 6 == 0) //Zorgt voor een interval zodat de shape niet te snel beweegt
        {
            Block.MoveRight();
        }
        else if (inputHelper.KeyDown(LeftMove) && gameTime.TotalGameTime.Ticks % 6 == 0)
        {
            Block.MoveLeft();
        }

        //The next part is about rotating the blocks.
        if (inputHelper.KeyPressed(RotateCCW))
        {
            TetrisBlock.RotateCounterClockwiseLegit(Block);
        }
        else if (inputHelper.KeyPressed(RotateCW))
        {
            TetrisBlock.RotateClockwiseLegit(Block);
        }

        //The following part allows the player to move the block down faster.
        if (inputHelper.KeyDown(GoDown) && gameTime.TotalGameTime.Ticks % 6 == 0)
        {
            Block.MoveDown();
            ForceBlockDownwards = true;
        }
        else
            ForceBlockDownwards = false;

        if (inputHelper.KeyPressed(Test)) //Dit is alleen om te testen en moet later verwijderd worden.
            Clear(); //Dit is alleen om te testen en moet later verwijderd worden.
    }
    
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = BeginPosition;
        //Tekent de gekleurde blokjes en de achtergrond grid.
        for (int i = 0; i < Width; i++)
        {
            for (int f = 0; f < Height; f++)
            {
                spriteBatch.Draw(emptyCell, position, grid[i + 2, f]); //Omdat de array groter is dan de daadwerkelijke grid moet de grid opgeschoven worden door waar getekend wordt naar rechts te schuiven
                position.Y += 30;
            }
            position.Y = 0;
            position.X += 30;
        }

        Block.Draw(gameTime, spriteBatch);
        NextBlock.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(GameWorld.font, "Next Block : ", new Vector2(380 + BeginPosition.X, 5 + BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Score : " + Score.ToString(), new Vector2(380 + BeginPosition.X, 155 + BeginPosition.Y), Color.Blue); //Tekent de Score van de speler.
        spriteBatch.DrawString(GameWorld.font, "Level : " + level.ToString(), new Vector2(380 + BeginPosition.X, 175 + BeginPosition.Y), Color.Blue); //Tekent welk level de speler momenteel in zit.
    }

    /// <summary>
    /// Makes the next block the currently active block and creates a new random block to put in the position for the next block.
    /// </summary>
    public void NewBlock()
    {
        Block = NextBlock;
        switch (GameWorld.Random.Next(7))
        {
            case 0:
                NextBlock = new Opiece(this);
                break;
            case 1:
                NextBlock = new Ipiece(this);
                break;
            case 2:
                NextBlock = new Lpiece(this);
                break;
            case 3:
                NextBlock = new Jpiece(this);
                break;
            case 4:
                NextBlock = new Spiece(this);
                break;
            case 5:
                NextBlock = new Zpiece(this);
                break;
            case 6:
                NextBlock = new Tpiece(this);
                break;
        }
        Block.BlockPosition = new Vector2(6, 0); //Dit is de positie waar een blok dat nieuw de grid binnenkomt moet beginnen.
    }

    /// <summary>
    /// Checks weither there are any rows filled with blocks. If a row is filled, it removes that row, awards the player with points and moves all rows above one row down.
    /// </summary>
    public void CheckRows()
    {
        int Multiplier = 1;
        for (int i = 0; i < Height; i++)
        {
            bool RowIsFull = true;
            for (int f = 2; f < Width + 2; f++)
            {
                if (grid[f, i] == Color.White)
                    RowIsFull = false;
            }
            if (RowIsFull)
            {
                for (int k = 2; k < Width + 2; k++) 
                {
                    for (int l = i; l > 0; l--)
                    {
                        grid[k, l] = grid[k, l - 1];
                    }
                }
                Score = Score + 10*Multiplier;
                Multiplier += Multiplier;
                if (Score / 100 != level - 1)
                {
                    fallingSpeed *= 1.2;
                    level++;
                }
            }
        }
    }

    /// <summary>
    /// Clears the grid.                                Als we deze later niet meer gebruiken -> voeg samen met reset()
    /// </summary>
    public void Clear() 
    {
        Vector2 position = BeginPosition;
        for (int i = 2; i < Width + 2; i++)
        {
            for (int f = 0; f < Height; f++)
            {
                grid[i ,f] = Color.White;
                position.Y += 30;
            }
            position.Y = 0;
            position.X += 30;
        }
    }

    /// <summary>
    /// Resets the grid.
    /// </summary>
    public void Reset()
    {
        Clear();
        NewBlock();
        NewBlock();
        Score = 0;
        level = 1;
        fallingSpeed = 1;
        BeginPosition = new Vector2(220, 0);
        IsDead = false;
    }

}