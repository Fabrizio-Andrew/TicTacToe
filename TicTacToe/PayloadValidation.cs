using TicTacToe.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class PayloadValidation
    {
        public static bool ValidatePayload(ExecuteMove messagePayload)
        {
            // Validate that symbols are 'X' or 'O' and are different
            if (messagePayload.humanPlayerSymbol != 'X' && messagePayload.humanPlayerSymbol != 'O')
            {
                return false;
            }

            if (messagePayload.azurePlayerSymbol != 'X' && messagePayload.azurePlayerSymbol != 'O')
            {
                return false;
            }

            if (messagePayload.azurePlayerSymbol == messagePayload.humanPlayerSymbol)
            {
                return false;
            }

            // Validate that the gameBoard is the proper size and all values are 'X', 'O', or '?'
            if (messagePayload.gameBoard.Length > 9 || messagePayload.gameBoard.Length < 9)
            {
                return false;
            }

            for (int i = 0; i < messagePayload.gameBoard.Length; i++)
            {
                if (messagePayload.gameBoard[i] != 'X' && messagePayload.gameBoard[i] != 'O' && messagePayload.gameBoard[i] != '?')
                {
                    return false;
                }
            }


            // Validate that the difference between the number of X's and O's is not greater than 1
            int xCount = 0;
            int oCount = 0;

            for (int i = 0; i < messagePayload.gameBoard.Length; i++)
            {
                if (messagePayload.gameBoard[i] == 'X')
                {
                    xCount++;
                }
                else if (messagePayload.gameBoard[i] == 'O')
                {
                    oCount++;
                }
            }
            if ((xCount - oCount) > 1 || (xCount - oCount) < -1)
            {
                return false;
            }
            

            // Validate that the player's move is represented on the gameBoard.  If the player did not move, gameBoard must be all ?'s.
            if (messagePayload.move == null)
            {
                for (int i = 0; i < messagePayload.gameBoard.Length; i++)
                {
                    if (messagePayload.gameBoard[i] != '?')
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (messagePayload.gameBoard[(int)messagePayload.move] != messagePayload.humanPlayerSymbol)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
