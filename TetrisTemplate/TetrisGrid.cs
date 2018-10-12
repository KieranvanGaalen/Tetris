﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    public static Texture2D emptyCell; // The sprite of a single empty cell in the grid.
    public static Vector2 BeginPosition = new Vector2(220, 0);// The position at which this TetrisGrid should be drawn. Also used to draw the collored blocks.
    public int Width { get { return 12; } } // The number of grid elements in the x-direction.
    public int Height { get { return 20; } } // The number of grid elements in the y-direction.
    Random random = new Random();
    public Color[,] grid = new Color[16, 22]; //De grid is groter dan wat getekend wordt want anders valt de blockgrid buiten de array, de array wordt 2 rechts, 2 links en 2 onder de grid uitgebreid
    TetrisBlock Block;

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        Clear();
        grid[8, 6] = Color.Green; //Dit is alleen om te testen en moet later verwijderd worden.
        grid[8, 7] = Color.White; //Dit is alleen om te testen en moet later verwijderd worden.
        grid[8, 8] = Color.Gray; //Dit is alleen om te testen en moet later verwijderd worden.
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        Block = new Ipiece(this);
    }

    //Update methode voor de TetrisGrid.
    public void Update(GameTime gameTime)
    {
        Block.Update(gameTime);
    }

    //HandleInput methode voor de TetrisGrid.
    public void HandleInput(GameTime gameTime, InputHelper inputHelper, Keys LeftMove, Keys RightMove, Keys RotateCW, Keys RotateCCW, Keys Test)
    {
        int moveLegit = 0;
        if (inputHelper.KeyDown(RightMove) && gameTime.TotalGameTime.Ticks % 6 == 0) //Zorgt voor de input en een interval zodat de shape niet te snel beweegt
        {
            for (int i = Block.BlockGrid.GetLength(0) - 1; i >= 0; i--) //Gaat vanaf rechts het eerste ingekleurde blokje zoeken
            {
                for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
                {
                    if (Block.BlockGrid[i, f]) //Als het eerste ingekleurde blokje rechts gevonden is
                    {
                        if (grid[(int)Block.BlockPosition.X + i + 1, (int)Block.BlockPosition.Y + f] == Color.White)
                        {
                            moveLegit++;
                        }
                    }
                }
            }
            if (moveLegit == 4)
            {
                Block.BlockPosition.X++;
                moveLegit = 0;
            }
        }
        else if (inputHelper.KeyDown(LeftMove) && gameTime.TotalGameTime.Ticks % 6 == 0)
        {
            for (int i = 0; i < Block.BlockGrid.GetLength(0); i++)
            {
                for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
                {
                    if (Block.BlockGrid[i, f])
                    {
                        if (grid[(int)Block.BlockPosition.X + i - 1, (int)Block.BlockPosition.Y + f] == Color.White)
                        {
                            moveLegit++;
                        }
                    }
                }
            }
            if (moveLegit == 4)
            {
                Block.BlockPosition.X--;
                moveLegit = 0;
            }
        }

        if (inputHelper.KeyPressed(RotateCCW))
        {
            Block.BlockGrid = TetrisBlock.RotateCounterClockwise(Block.BlockGrid);
            for (int i = 0; i < Block.BlockGrid.GetLength(0); i++)
            {
                for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
                {
                    if (Block.BlockGrid[i, f])
                    {
                        if (grid[(int)Block.BlockPosition.X + i, (int)Block.BlockPosition.Y + f] != Color.White)
                        {
                            Block.BlockGrid = TetrisBlock.RotateClockwise(Block.BlockGrid);
                            return;
                        }
                    }
                }
            }
        }
        else if (inputHelper.KeyPressed(RotateCW))
        {
            Block.BlockGrid = TetrisBlock.RotateClockwise(Block.BlockGrid);
            for (int i = 0; i < Block.BlockGrid.GetLength(0); i++)
            {
                for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
                {
                    if (Block.BlockGrid[i, f])
                    {
                        if (grid[(int)Block.BlockPosition.X + i, (int)Block.BlockPosition.Y + f] != Color.White)
                        {
                            Block.BlockGrid = TetrisBlock.RotateCounterClockwise(Block.BlockGrid);
                            return;
                        }
                    }
                }
            }
        }

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
    }

    public void NewBlock()
    {
        switch (GameWorld.Random.Next(7))
        {
            case 0:
                Block = new Opiece(this);
                break;
            case 1:
                Block = new Ipiece(this);
                break;
            case 2:
                Block = new Lpiece(this);
                break;
            case 3:
                Block = new Jpiece(this);
                break;
            case 4:
                Block = new Spiece(this);
                break;
            case 5:
                Block = new Zpiece(this);
                break;
            case 6:
                Block = new Tpiece(this);
                break;
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
    
}