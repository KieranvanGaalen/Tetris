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
    public static Texture2D emptyCell ; // The sprite of a single empty cell in the grid.
    public Vector2 BeginPosition = new Vector2(250, 0);// The position at which this TetrisGrid should be drawn. Also used to draw the collored blocks.
    public int Width { get { return 10; } } // The number of grid elements in the x-direction.
    public int Height { get { return 20; } } // The number of grid elements in the y-direction.
    readonly Random random = new Random(); //Random used for creating the new blocks
    public Color[,] grid = new Color[14, 22]; //The grid is larger than is drawn, it is 2 larger to the left, right and bottom, this is later corrected
    TetrisBlock Block; //The block that is currently active gets stored here.
    public bool ForceBlockDownwards; //A boolian used for indicating that a block is being forced down.
    public int Score { get; private set; } = 0; //An integer used to keep track of the players score.
    private int level = 1; //An interger used to keep track of the players level.
    public bool IsDead = false; //When the blocks can no longer fall down this will be set to true and the game will be over.
    public double FallingSpeed { get; private set; } = 1; //The current falling speed of the Block.
    private TetrisBlock NextBlock;
    private GhostBlock GhostBlock;

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    public TetrisGrid()
    {
        Clear();
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        NextBlock = new Ipiece(this);
        NewBlock();
        GhostBlock = new GhostBlock(this);
    }

    /// <summary>
    /// Update method for the TetrisGrid.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    public void Update(GameTime gameTime)
    {
        Block.Update(gameTime);
        GhostBlock.Update(gameTime, Block);
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
    public void HandleInput(GameTime gameTime, InputHelper inputHelper, Keys LeftMove, Keys RightMove, Keys RotateCW, Keys RotateCCW, Keys SlamDown, Keys GoDown)
    {
        //This part makes sure that the player can move the block to the left and right.
        if (inputHelper.KeyDown(RightMove) && gameTime.TotalGameTime.Ticks % 6 == 0) //Makes sure the movement is not to fast
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

        if (inputHelper.KeyPressed(SlamDown)) 
            Block.SlamDown();
    }
    
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = BeginPosition;
        //Draws the colored blocks and the background grid
        for (int i = 0; i < Width; i++)
        {
            for (int f = 0; f < Height; f++)
            {
                spriteBatch.Draw(emptyCell, position, grid[i + 2, f]); //The array is 2 larger to the left and to the right, but the grid starts at 0, this is corrected by moving the grid 2 to the right, or x+2
                position.Y += 30;
            }
            position.Y = 0;
            position.X += 30;
        }
        GhostBlock.Draw(gameTime, spriteBatch);
        Block.Draw(gameTime, spriteBatch);
        NextBlock.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(GameWorld.font, "Next Block : ", new Vector2(350 + BeginPosition.X, 5 + BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Score : " + Score.ToString(), new Vector2(350 + BeginPosition.X, 155 + BeginPosition.Y), Color.Blue); //Draws the players score
        spriteBatch.DrawString(GameWorld.font, "Level : " + level.ToString(), new Vector2(350 + BeginPosition.X, 175 + BeginPosition.Y), Color.Blue); //Draws the players level
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
        Block.BlockPosition = new Vector2(6, 0); //This is the position that a new block is first drawn on the grid
    }

    /// <summary>
    /// Checks whether there are any rows filled with blocks. If a row is filled, it removes that row, awards the player with points and moves all rows above one row down.
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
                    FallingSpeed *= 1.2;
                    level++;
                }
            }
        }
    }

    /// <summary>
    /// Clears the grid.
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
        FallingSpeed = 1;
        BeginPosition = new Vector2(220, 0);
        IsDead = false;
    }

}