using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    Texture2D emptyCell; // The sprite of a single empty cell in the grid.
    Vector2 position;// The position at which this TetrisGrid should be drawn
    public int Width { get { return 12; } } // The number of grid elements in the x-direction.
    public int Height { get { return 20; } } // The number of grid elements in the y-direction.
    Random random = new Random();
    Color mycolor;
    enum ShapeColors {Blue, Red, Green, Yellow, Purple, Orange, Pink};
    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        int RRandom = random.Next(10, 240), GRandom = random.Next(10, 240), BRandom = random.Next(10, 240);
        mycolor = new Color(RRandom, GRandom, BRandom);
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = new Vector2 (220, 0);
        Clear();
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
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
        position = new Vector2(220, 0);



    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

