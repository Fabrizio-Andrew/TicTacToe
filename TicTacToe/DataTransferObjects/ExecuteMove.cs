﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class ExecuteMove
    {
        public int? move { get; set; }

        public char azurePlayerSymbol { get; set; }

        public char humanPlayerSymbol { get; set; }

        public char[]  gameBoard { get; set; }

    }
}