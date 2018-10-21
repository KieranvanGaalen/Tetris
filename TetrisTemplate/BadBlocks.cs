using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


class FullOpiece : TetrisBlock //Assigns the blockgrids for the special VS mode blocks
{
    public FullOpiece(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[0, 1] = true;
        BlockGrid[2, 1] = true;
        BlockGrid[0, 2] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 2] = true;
    }
}

class Hpiece : TetrisBlock
{
    public Hpiece(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[0, 2] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 2] = true;
    }
}


class ThreeDots : TetrisBlock
{
    public ThreeDots(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid[0, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[1, 2] = true;
    }
}

class TwoLines : TetrisBlock
{
    public TwoLines(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[0, 2] = true;
        BlockGrid[1, 2] = true;
        BlockGrid[2, 2] = true;
    }
}

class Ldiagonal : TetrisBlock
{
    public Ldiagonal(TetrisGrid parent) : base(parent)
    {
        color = Color.DimGray;
        BlockGrid = new bool[4, 4];
        BlockGrid[0, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[2, 2] = true;
        BlockGrid[3, 3] = true;
    }
}

class InvisableTpiece : TetrisBlock
{
    public InvisableTpiece(TetrisGrid parent) : base(parent)
    {
        color = new Color(254, 254, 254);
        BlockGrid[0, 0] = true;
        BlockGrid[1, 0] = true;
        BlockGrid[2, 0] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[1, 2] = true;
    }
}