using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/// <summary>
/// A class for representing a SideBar.
/// </summary>
class SideBar
{
    protected TetrisGrid MyGrid;
    protected GameWorld parent;
    public SideBar(GameWorld parent, TetrisGrid grid)
    {
        MyGrid = grid;
        this.parent = parent;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
       spriteBatch.Draw(GameWorld.SideBar, MyGrid.BeginPosition + new Vector2(MyGrid.Width * 30, 0), Color.White);
    }
}

class VSSideBar : SideBar
{
    TetrisGrid EnemyGrid;
    TetrisBlock DotExample;
    TetrisBlock BadBlockExample;
    TetrisBlock BombExample;
    public VSSideBar(GameWorld parent, TetrisGrid MyGrid, TetrisGrid EnemyGrid) : base(parent, MyGrid)
    {
        this.EnemyGrid = EnemyGrid;
        DotExample = new Dot(MyGrid);
        DotExample.BlockPosition = new Vector2(14, 15);
        BadBlockExample = new FullOpiece(MyGrid);
        BadBlockExample.BlockPosition = new Vector2(13, 9);
        BombExample = new Bomb(MyGrid);
        BombExample.BlockPosition = new Vector2(14, 19);
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper, Keys BadBlock, Keys EzBlock, Keys Bomb)
    {
        if (inputHelper.KeyPressed(BadBlock) && MyGrid.Score >= 90)
        {
            EnemyGrid.NextBlock = MakeBadBlock();
            MyGrid.Score -= 90;
        }
        else if (inputHelper.KeyPressed(EzBlock) && MyGrid.Score >= 60)
        {
            MyGrid.NextBlock = new Dot(MyGrid);
            MyGrid.Score -= 60;
        }
        else if (inputHelper.KeyPressed(Bomb) && MyGrid.Score >= 150)
        {
            MyGrid.NextBlock = new Bomb(MyGrid);
            MyGrid.Score -= 150;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, string BadBlockKey, string EzBlockKey, string BombKey)
    {
        base.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(GameWorld.font, BadBlockKey + ": send bad block", new Vector2(320 + MyGrid.BeginPosition.X, 220 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "to the enemy", new Vector2(320 + MyGrid.BeginPosition.X, 235 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Cost: 90", new Vector2(320 + MyGrid.BeginPosition.X, 250 + MyGrid.BeginPosition.Y), Color.Blue);
        BadBlockExample.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(GameWorld.font, EzBlockKey + ": Next block", new Vector2(320 + MyGrid.BeginPosition.X, 370 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "becomes this", new Vector2(320 + MyGrid.BeginPosition.X, 385 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "1x1 block", new Vector2(320 + MyGrid.BeginPosition.X, 400 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Cost: 60", new Vector2(320 + MyGrid.BeginPosition.X, 415 + MyGrid.BeginPosition.Y), Color.Blue);
        DotExample.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(GameWorld.font, BombKey + ": Next block", new Vector2(320 + MyGrid.BeginPosition.X, 485 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "becomes a bomb. ", new Vector2(320 + MyGrid.BeginPosition.X, 500 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Explodes in", new Vector2(320 + MyGrid.BeginPosition.X, 515 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "a 3x3 area", new Vector2(320 + MyGrid.BeginPosition.X, 530 + MyGrid.BeginPosition.Y), Color.Blue);
        spriteBatch.DrawString(GameWorld.font, "Cost: 150", new Vector2(320 + MyGrid.BeginPosition.X, 545 + MyGrid.BeginPosition.Y), Color.Blue);
        BombExample.Draw(gameTime, spriteBatch);
    }

    private TetrisBlock MakeBadBlock()
    {
        TetrisBlock BadBlock = new Opiece(EnemyGrid);
        switch (GameWorld.Random.Next(5))
        {
            case 0:
                BadBlock = new FullOpiece(EnemyGrid);
                break;
            case 1:
                BadBlock = new Hpiece(EnemyGrid);
                break;
            case 2:
                BadBlock = new ThreeDots(EnemyGrid);
                break;
            case 3:
                BadBlock = new TwoLines(EnemyGrid);
                break;
            case 4:
                BadBlock = new Ldiagonal(EnemyGrid);
                break;
            case 5:
                BadBlock = new InvisableTpiece(EnemyGrid);
                break;
        }
        return BadBlock;
    }
}