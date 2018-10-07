using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    public static Texture2D emptyCell; // The sprite of a single empty cell in the grid.
    public static Vector2 BeginPosition = new Vector2 (220,0);// The position at which this TetrisGrid should be drawn. Also used to draw the collored blocks.
    public int Width { get { return 12; } } // The number of grid elements in the x-direction.
    public int Height { get { return 20; } } // The number of grid elements in the y-direction.
    Random random = new Random();
    public Color[,] grid = new Color[12, 20];
    TetrisBlock Block = new Jpiece();

    Color mycolor;
    
    public void colorofzo() //Deze methode is alleen om te testen en moet later verwijderd worden.
    {
        grid[6, 6] = Color.Green;
    }
    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        colorofzo(); //Dit is alleen om te testen en moet later verwijderd worden.
        int RRandom = random.Next(10, 240), GRandom = random.Next(10, 240), BRandom = random.Next(10, 240);
        mycolor = new Color(RRandom, GRandom, BRandom);
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
    }

    //Update methode voor de TetrisGrid.
    public void Update(GameTime gameTime)
    {
        Block.Update(gameTime);
    }

    //HandleInput methode voor de TetrisGrid.
    public void HandleInput(GameTime gameTime, InputHelper inputHelper, Keys LeftMove, Keys RightMove)
    {
        if (inputHelper.KeyDown(RightMove) && gameTime.TotalGameTime.Ticks % 6 == 0)
            Block.BlockPosition.X += 1;
        else if (inputHelper.KeyDown(LeftMove) && gameTime.TotalGameTime.Ticks % 6 == 0)
            Block.BlockPosition.X -= 1;
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = BeginPosition;
        //Tekent de achtergrondgrid
        for (int i = 0; i < Width; i++)
        {
            for (int f = 0; f < Height; f++)
            {
                spriteBatch.Draw(emptyCell, position, Color.White);
                position.Y += 30;
            }
            position.Y = 0;
            position.X += 30;
        }
        
        //Tekent de gelkeurde blokjes over de achtergrond grid heen.
        position = BeginPosition;
        for (int i = 0; i < Width; i++)
        {
            for (int f = 0; f < Height; f++)
            {
                if (grid[i,f] != null)
                {
                    spriteBatch.Draw(emptyCell, position, grid[i,f]);
                }
                position.Y += 30;
            }
            position.Y = 0;
            position.X += 30;
        }

        Block.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

