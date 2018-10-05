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
        public static bool[,] BlockGrid = new bool[3, 3];
        public static bool[,] BlockGrid4x4 = new bool[4, 4];
        public static bool[,] PreviousBlockGrid = new bool[3, 3];
        static int x = 0;
        static int y = 0;


        public void IsBlockBelow()
        {
            
        }

        public static void NewBlock()
        {
            x = 4;
            y = 0;
        for(int i = 0; i < BlockGrid.GetLength(0); i++)
        {
            for (int f = 0; f < BlockGrid.GetLength(1); f++)
            {
                BlockGrid[i, f] = false;
            }
        }
    }
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
        static public void opiece()
        {
            BlockGrid4x4[1, 1] = true;
            BlockGrid4x4[1, 2] = true;
            BlockGrid4x4[2, 1] = true;
            BlockGrid4x4[2, 2] = true;
        }
    }

    class Ipiece : TetrisBlock
    {
        static public void ipiece()
        {
            BlockGrid4x4[1, 0] = true;
            BlockGrid4x4[1, 1] = true;
            BlockGrid4x4[1, 2] = true;
            BlockGrid4x4[1, 3] = true;
        }
    }
    class Spiece : TetrisBlock
    {
        static public void spiece()
        {
            BlockGrid[0, 1] = true;
            BlockGrid[1, 1] = true;
            BlockGrid[1, 0] = true;
            BlockGrid[2, 0] = true;
        }
    }

    class Zpiece : TetrisBlock
    {
        static public void zpiece()
        {
            BlockGrid[0, 0] = true;
            BlockGrid[0, 1] = true;
            BlockGrid[1, 1] = true;
            BlockGrid[2, 1] = true;
        }
    }

    class Lpiece : TetrisBlock
    {
        static public void lpiece()
        {
            BlockGrid[1, 0] = true;
            BlockGrid[1, 1] = true;
            BlockGrid[1, 2] = true;
            BlockGrid[2, 2] = true;
        }
    }

    class Jpiece : TetrisBlock
    {
        static public void jpiece()
        {
            BlockGrid[1, 0] = true;
            BlockGrid[1, 1] = true;
            BlockGrid[1, 2] = true;
            BlockGrid[0, 2] = true;
        }
    }

    class Tpiece : TetrisBlock
    {
        static public void tpiece()
        {
            BlockGrid[0, 1] = true;
            BlockGrid[1, 1] = true;
            BlockGrid[2, 1] = true;
            BlockGrid[1, 2] = true;
        }
    }
