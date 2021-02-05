using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.DataTransferObjects
{
    /// <summary>
    /// Defines the message payload representing the player's move.
    /// </summary>
    public class ExecuteMove
    {
        public int? move { get; set; }

        public char azurePlayerSymbol { get; set; }

        public char humanPlayerSymbol { get; set; }

        public char[]  gameBoard { get; set; }

    }
}
