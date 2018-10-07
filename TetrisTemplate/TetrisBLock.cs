﻿using Microsoft.Xna.Framework;
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
    public bool[,] BlockGrid4x4 = new bool[4, 4];
    public bool[,] PreviousBlockGrid = new bool[3, 3];
    public Vector2 BlockPosition = new Vector2(4,0);
    
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
            if (IsBlockBelow())
            {
                PlaceBlock();
                NewBlock();
            }
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
                    spriteBatch.Draw(TetrisGrid.emptyCell, position, Color.Blue);
                }
                position.Y += 30;
            }
            position.Y = 30 * BlockPosition.Y;
            position.X += 30;
        }
        
    }

    //Plaatst een nieuw blok bovenaan de grid
    public void NewBlock() //Deze methode wordt waarschijnlijk overbodig, omdat er iedere keer dat er een nieuw blok verschijnt
    {                      //een andere class aangeroepen moet worden speciaal voor dat blok waardoor alles automatisch reset.
        BlockPosition.X = 4;
        BlockPosition.Y = 0;
        for (int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                BlockGrid[i, f] = false;
            }
        }
        // ̶H̶i̶e̶r̶ ̶n̶o̶g̶ ̶e̶e̶n̶ ̶m̶e̶t̶h̶o̶d̶e̶ ̶t̶o̶e̶v̶o̶e̶g̶e̶n̶ ̶o̶m̶ ̶e̶e̶n̶ ̶r̶a̶n̶d̶o̶m̶ ̶b̶l̶o̶k̶ ̶e̶r̶i̶n̶ ̶t̶e̶ ̶z̶e̶t̶t̶e̶n̶.
        // De verschillende blokken zijn aparte classes die alle methoden van deze class doorkrijgen, 
        // Dus moet een objocet van 1 van die classes aangemaakt worden. (op willekeurige wijze.)
    }

    //Roteert een blok tegen de klok in.
    public void BlockRotationCCW()
    {
        PreviousBlockGrid = BlockGrid;
        BlockGrid[0, 0] = PreviousBlockGrid[2, 0];
        BlockGrid[0, 1] = PreviousBlockGrid[1, 0];
        BlockGrid[0, 2] = PreviousBlockGrid[0, 0];
        BlockGrid[1, 2] = PreviousBlockGrid[0, 1];
        BlockGrid[2, 2] = PreviousBlockGrid[0, 2];
        BlockGrid[2, 1] = PreviousBlockGrid[1, 2];
        BlockGrid[2, 0] = PreviousBlockGrid[2, 2];
        BlockGrid[1, 0] = PreviousBlockGrid[2, 1];
    }

    //Roteert een blok met de klok mee.
    public void BlockRotationCW()
    {
        PreviousBlockGrid = BlockGrid;
        BlockGrid[0, 0] = PreviousBlockGrid[0, 2];
        BlockGrid[0, 1] = PreviousBlockGrid[1, 2];
        BlockGrid[0, 2] = PreviousBlockGrid[2, 2];
        BlockGrid[1, 2] = PreviousBlockGrid[2, 1];
        BlockGrid[2, 2] = PreviousBlockGrid[2, 0];
        BlockGrid[2, 1] = PreviousBlockGrid[1, 2];
        BlockGrid[2, 0] = PreviousBlockGrid[0, 0];
        BlockGrid[1, 0] = PreviousBlockGrid[0, 1];
    }

}

class Opiece : TetrisBlock
{
    public Opiece()
    {
        BlockGrid4x4[1, 1] = true;
        BlockGrid4x4[1, 2] = true;
        BlockGrid4x4[2, 1] = true;
        BlockGrid4x4[2, 2] = true;
    }
}

class Ipiece : TetrisBlock
{
    public Ipiece()
    {
        BlockGrid4x4[1, 0] = true;
        BlockGrid4x4[1, 1] = true;
        BlockGrid4x4[1, 2] = true;
        BlockGrid4x4[1, 3] = true;
    }
}

class Spiece : TetrisBlock
{
    public Spiece()
    {
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
        BlockGrid[0, 0] = true;
        BlockGrid[0, 1] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[2, 1] = true;
    }
}

class Lpiece : TetrisBlock
{
    public Lpiece()
    {
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
        BlockGrid[0, 1] = true;
        BlockGrid[1, 1] = true;
        BlockGrid[2, 1] = true;
        BlockGrid[1, 2] = true;
    }
}