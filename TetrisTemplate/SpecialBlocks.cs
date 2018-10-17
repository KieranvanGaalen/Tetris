using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

class GhostBlock : TetrisBlock
{
    public GhostBlock(TetrisGrid parent) : base(parent)
    {
        color = Color.Gray;
        BlockGrid = new bool[4, 4];
        BlockGrid[1, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[1, 3] = true;
    }

    public void Clone(TetrisBlock Block)
    {
        BlockGrid = Block.BlockGrid;
        BlockPosition = Block.BlockPosition;
    }

    public override void SlamDown()
    {
        while (true)
        {
            if (IsBlockBelow())
                return;
            else
                BlockPosition.Y += 1;
        }
    }

    public void Update(GameTime gameTime, TetrisBlock Block)
    {
        Clone(Block);
        SlamDown();
    }
}

class Dot : TetrisBlock
{
    public Dot(TetrisGrid parent) : base(parent)
    {
        color = Color.Gold;
        BlockGrid = new bool[1, 1];
        BlockGrid[0, 0] = true;
    }
}

class Bomb : TetrisBlock
{
    public Bomb(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid = new bool[1, 1];
        BlockGrid[0, 0] = true;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 position = parent.BeginPosition;
        //Sets the coordinates of the top left of the block
        position.Y += 30 * BlockPosition.Y;
        position.X += 30 * BlockPosition.X - 60; //Correction of a larger grid
        //Draws the falling bomb
        spriteBatch.Draw(TetrisGrid.emptyCell, position, color);
        spriteBatch.Draw(GameWorld.Bomb, position, Color.White);
    }

    protected override void PlaceBlock()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int f = 0; f < 3; f++)
            {
                parent.grid[(int)BlockPosition.X + i - 1, (int)BlockPosition.Y + f - 1] = Color.White;
            }
        }
        parent.NewBlock();
    }
}