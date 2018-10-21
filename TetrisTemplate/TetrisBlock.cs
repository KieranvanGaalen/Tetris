using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TetrisBlock
{
    /* The grid for blocks is drawn as below
          0     1    2     3
       0 false true false false
       1 false true false false
       2 false true false false
       3 false true false false  */
    public bool[,] BlockGrid = new bool[3, 3];
    public Vector2 BlockPosition = new Vector2(13, 1); //When a new block is created it is first placed in the next block position
    public Color color; //The color of the current block
    public TetrisGrid parent; //This is used to make all variables from the grid available to falling blocks

    /// <summary>
    /// The constructor method for a TetrisBlock object.
    /// </summary>
    /// <param name="parent">A variable used to set a reference to the parent object.</param>
    public TetrisBlock(TetrisGrid parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// Checks whether the block can move 1 position down or not. Returns either true or false.
    /// </summary>
    public bool IsBlockBelow()
    {
        for (int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                if (BlockGrid[i, f] && (parent.grid[(int)BlockPosition.X + i, (int)BlockPosition.Y + f + 1] != Color.White))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Places a block in the grid. Also executes the CheckRows and NewBlock methods in the parent object.
    /// </summary>
    protected virtual void PlaceBlock()
    {
        for (int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                if (BlockGrid[i, f])
                    parent.grid[(int)BlockPosition.X + i, (int)BlockPosition.Y + f] = color;
            }
        }
        GameWorld.BlockPlaced.Play();
        parent.CheckRows();
        if (BlockPosition == new Vector2(6, 0))
            parent.IsDead = true;
        parent.NewBlock();
    }

    /// <summary>
    /// Update method for a Tetrisblock.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    public virtual void Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.Ticks % (int)(60.0 / parent.FallingSpeed) == 0 && parent.ForceBlockDownwards == false)
            MoveDown();
    }

    /// <summary>
    /// Drawing method for a TetrisBlock.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = parent.BeginPosition;
        //Sets the coordinates of the top left of the block
        position.Y += 30 * BlockPosition.Y;
        position.X += 30 * BlockPosition.X - 60; //Correction of a larger grid
        //Draws the falling block
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

    /// <summary>
    /// Moves the block 1 position down. If there is a block below, it calls the PlaceBlock() method to place the block in the grid.
    /// </summary>
    public void MoveDown()
    {
        if (IsBlockBelow())
            PlaceBlock();
        BlockPosition.Y += 1;
    }

    public virtual void SlamDown()
    {
        while (true)
        {
            if (IsBlockBelow())
            {
                PlaceBlock();
                return;
            }
            BlockPosition.Y += 1;
        }
    }

    /// <summary>
    /// Checks if the block can move to the left and then moves the block 1 position to the left (if possable). 
    /// </summary>
    public void MoveLeft()
    {
        bool moveLegit = true;
        for (int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                if (BlockGrid[i, f])
                {
                    if (parent.grid[(int)BlockPosition.X + i - 1, (int)BlockPosition.Y + f] != Color.White)
                    {
                        moveLegit = false;
                    }
                }
            }
        }
        if (moveLegit)
            BlockPosition.X--;
    }

    /// <summary>
    /// Checks if the block can move to the right and then moves the block 1 position to the right (if possable). 
    /// </summary>
    public void MoveRight()
    {
        bool moveLegit = true;
        for (int i = BlockGrid.GetLength(0) - 1; i >= 0; i--) //Searches the first colored block to the right
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                if (BlockGrid[i, f]) //When the first colored block to the right is found
                {
                    if (parent.grid[(int)BlockPosition.X + i + 1, (int)BlockPosition.Y + f] != Color.White) //Check if you can move
                        moveLegit = false;
                }
            }
        }
        if (moveLegit)
            BlockPosition.X++;
    }

    /// <summary>
    /// Returns a version of the block that is rotated 90 degrees CounterClockwise. This method does NOT check weither this is alowed or not.
    /// </summary>
    /// <param name="oldBlock">The block that you want to rotate.</param>
    protected static bool[,] RotateCounterClockwise(bool[,] oldBlock)
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

    /// <summary>
    /// Returns a version of the block that is rotated 90 degrees Clockwise. This method does NOT check weither this is alowed or not.
    /// </summary>
    /// <param name="oldBlock">The block that you want to rotate.</param>
    protected static bool[,] RotateClockwise(bool[,] oldBlock)
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

    /// <summary>
    /// Check if the block is allowed to rotate 90 degrees CounterClockwise, and then rotates it (only if that is allowed).
    /// </summary>
    /// <param name="Block">The block that you want to rotate.</param>
    public static void RotateCounterClockwiseLegit(TetrisBlock Block)
    {
        Block.BlockGrid = RotateCounterClockwise(Block.BlockGrid);
        for (int i = 0; i < Block.BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
            {
                if (Block.BlockGrid[i, f] && Block.parent.grid[(int)Block.BlockPosition.X + i, (int)Block.BlockPosition.Y + f] != Color.White)
                {
                    Block.BlockGrid = RotateClockwise(Block.BlockGrid);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Check if the block is allowed to rotate 90 degrees Clockwise, and then rotates it (only if that is allowed).
    /// </summary>
    /// <param name="Block">The block that you want to rotate.</param>
    public static void RotateClockwiseLegit(TetrisBlock Block)
    {
        Block.BlockGrid = RotateClockwise(Block.BlockGrid);
        for (int i = 0; i < Block.BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < Block.BlockGrid.GetLength(1); f++)
            {
                if (Block.BlockGrid[i, f] && Block.parent.grid[(int)Block.BlockPosition.X + i, (int)Block.BlockPosition.Y + f] != Color.White)
                {
                    Block.BlockGrid = RotateCounterClockwise(Block.BlockGrid);
                    return;
                }
            }
        }
    }

}
//Grid assignments for the different standard blocks
class Opiece : TetrisBlock
{
    public Opiece(TetrisGrid parent) : base(parent)
    {
        color = Color.Yellow;
        BlockPosition = new Vector2(12, 0);
        BlockGrid = new bool[4, 4];
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 1] = true;
        BlockGrid[2, 2] = true;
    }
}

class Ipiece : TetrisBlock
{
    public Ipiece(TetrisGrid parent) : base(parent)
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
    public Spiece(TetrisGrid parent) : base(parent)
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
    public Zpiece(TetrisGrid parent) : base(parent)
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
    public Lpiece(TetrisGrid parent) : base(parent)
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
    public Jpiece(TetrisGrid parent) : base(parent)
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
    public Tpiece(TetrisGrid parent) : base(parent)
    {
        color = Color.Purple;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[1, 1] = true;
    }
}
