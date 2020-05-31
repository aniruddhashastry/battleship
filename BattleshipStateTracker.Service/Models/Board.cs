using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipStateTracker.Service.Models
{
    public class Board
    {
        public Block[,] Blocks;

        public bool AttackHasStarted { get; set; }

        public Board(int boardSize)
        {
            Blocks = new Block[boardSize, boardSize];

            for (int row = 0; row < Blocks.GetLength(0); row++)
            {
                for (int col = 0; col < Blocks.GetLength(1); col++)
                {
                    Blocks[row, col] = new Block();
                }
            }

        }
    }
}
