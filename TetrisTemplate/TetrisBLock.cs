using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TetrisBlock
{
    /* De grid van de blokken zal zoals hieronder aangegeven worden
          0     1    2     3
       0 false true false false
       1 false true false false
       2 false true false false
       3 false true false false
        Het middelpunt voor o.a. rotatie word 1,1 (de tweede true van boven) */
    public bool[,] BlockGrid = new bool[3, 3];
    public Vector2 BlockPosition = new Vector2(4, 0);
    public Color color;

    public bool IsBlockBelow()
    {
        //Kijken of het bewegende blok nog 1 naar beneden kan. Zoniet dan zit er een blok onder.
        return false;
    }

    private void PlaceBlock()
    {
        //Hier nog iets van code dat er voorzorgt dat het blok vast staat in de grid.
        //Checken of er een lijn gevuld is en punten geven. (Misschien aparte methode voor maken.)
    }

    //Update methode voor het bewegende blok.
    public void Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.Ticks % (60 / GameWorld.Level) == 0)
        {
            BlockPosition.Y += 1;
        }
    }

    //Tekent het bewegende blok
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = TetrisGrid.BeginPosition;
        //Zet de cordinaten van de linkerbovenhoek van de grid van het vallende blok erin.
        position.Y += 30 * BlockPosition.Y;
        position.X += 30 * BlockPosition.X;
        //Tekent het vallende blok.
        for (int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                if (BlockGrid[i, f] == true)
                {
                    spriteBatch.Draw(TetrisGrid.emptyCell, position, color);
                }
                position.Y += 30;
            }
            position.Y = 30 * BlockPosition.Y;
            position.X += 30;
        }

    }

    //Plaatst een nieuw blok bovenaan de grid
    public void NewBlock()
    {

        BlockPosition.X = 4;
        BlockPosition.Y = 0;
    }

    //Roteert een blok tegen de klok in.
    public static bool[,] RotateCounterClockwise(bool[,] oldBlock)
    {
        bool[,] newBlock = new bool[oldBlock.GetLength(1), oldBlock.GetLength(0)];
        int oldColumn = 0, oldRow;
        for (int newRow = 0; newRow < newBlock.GetLength(0); newRow++)
        {
            oldRow = 0;
            for (int newColumn = newBlock.GetLength(1) - 1; newColumn >= 0; newColumn--)
            {
                newBlock[newRow, newColumn] = oldBlock[oldRow, oldColumn];
                oldRow++;
            }
            oldColumn++;
        }
        return newBlock;
    }

    //Roteert een blok met de klok mee.
    public static bool[,] RotateClockwise(bool[,] oldBlock)
    {
        bool[,] newBlock = new bool[oldBlock.GetLength(1), oldBlock.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldBlock.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldBlock.GetLength(0); oldRow++)
            {
                newBlock[newRow, newColumn] = oldBlock[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newBlock;
    }

}

class Opiece : TetrisBlock
{
    public Opiece()
    {
        color = Color.Yellow;
        BlockGrid = new bool[4, 4];
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 1] = true;
        BlockGrid[2, 2] = true;
    }
}

class Ipiece : TetrisBlock
{
    public Ipiece()
    {
        color = Color.CornflowerBlue;
        BlockGrid = new bool[4, 4];
        BlockGrid[1, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[1, 3] = true;
    }
}

class Spiece : TetrisBlock
{
    public Spiece()
    {
        color = Color.LightGreen;
        BlockGrid[0, 1] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
    }
}

class Zpiece : TetrisBlock
{
    public Zpiece()
    {
        color = Color.Red;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 1] = true;
    }
}

class Lpiece : TetrisBlock
{
    public Lpiece()
    {
        color = Color.Blue;
        BlockGrid[1, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 2] = true;
    }
}

class Jpiece : TetrisBlock
{
    public Jpiece()
    {
        color = Color.Orange;
        BlockGrid[1, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[0, 2] = true;
    }
}

class Tpiece : TetrisBlock
{
    public Tpiece()
    {
        color = Color.Purple;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[1, 1] = true;
    }
}
