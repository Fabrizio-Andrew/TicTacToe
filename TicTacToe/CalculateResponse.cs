using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class CalculateResponse
    {
        //public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        //{
        //    return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        //}

        public static string CalculateMoveResponse(ExecuteMove messagePayload)
        {

            // Defines all 8 possible winning combinations in Tic Tac Toe
            int[,] victoryConditions = new int[8, 3]
            {
                {0, 1, 2}, {0, 3, 6}, {0, 4, 8}, {1, 4, 7}, {2, 4, 6}, {2, 5, 8}, {3, 4, 5}, {6, 7, 8}
            };

            //========== 1. SET UP THE GAME BOARD ==========
            // This section sets Human/Azure positions on an array called "gameState".  It also checks if the Human has won or the game is tied.
            // (We can assume Azure has not won before making its move.)

            // Prepare known elements of Azure's response
            ExecuteMoveResponse response = new ExecuteMoveResponse { };
            response.azurePlayerSymbol = messagePayload.azurePlayerSymbol;
            response.humanPlayerSymbol = messagePayload.humanPlayerSymbol;
            response.gameBoard = messagePayload.gameBoard;

            // Creates two lists: for gameBoard human and azure player positions
            List<int> humanPositions = new List<int>();
            List<int> azurePositions = new List<int>();

            for (int i = 0; i < messagePayload.gameBoard.Length; i++)
            {
                if (messagePayload.gameBoard[i] == messagePayload.humanPlayerSymbol)
                {
                    humanPositions.Add(i);
                }
                else if (messagePayload.gameBoard[i] == messagePayload.azurePlayerSymbol)
                {
                    azurePositions.Add(i);
                }
            }

            string humanSymbol = messagePayload.humanPlayerSymbol.ToString();
            string azureSymbol = messagePayload.azurePlayerSymbol.ToString();

            // Create a copy of victoryConditions (gameState) which will be used to track game progress towards possible victories
            string[,] gameState = (string[,])victoryConditions.Clone();


            // Compare gameState to humanPositions.
            // Replace any values in gameState with humanSymbol if they match (indicating that the human owns that space)
            foreach (int i in humanPositions)
            {
                for (int row = 0; row < gameState.GetLength(0); row++)
                {
                    for (int column = 0; column < gameState.GetLength(1); column++)
                    {
                        if (i.ToString() == gameState[row, column])
                        {
                            gameState[row, column] = humanSymbol;
                        }
                    }

                    // Check to see if the human has met any of the victory conditions
                    if (gameState[row, 0] == humanSymbol && gameState[row, 1] == humanSymbol && gameState[row, 2] == humanSymbol)
                    {
                        response.winner = messagePayload.humanPlayerSymbol;

                        // Set winPositions
                        for (int j = 0; j < 3; j++)
                        {
                            response.winPositions[j] = victoryConditions[row, j];
                        }

                        return response.ToString();
                    }
                }
            }

            // Check for a tie game
            if (Array.IndexOf(messagePayload.gameBoard, '?') == -1)
            {
                return "Tie game.";
            }


            // Compare gameState to azurePositions.
            // Replace any values in gameState with azureSymbol if they match (indicating that Azure owns that space)
            foreach (int i in azurePositions)
            {
                for (int row = 0; row < gameState.GetLength(0); row++)
                {
                    for (int column = 0; column < gameState.GetLength(1); column++)
                    {
                        if (i.ToString() == gameState[row, column])
                        {
                            gameState[row, column] = azureSymbol;
                        }
                    }
                }
            }

            //========== 2. CALCULATE AZURE'S MOVE ==========
            // This section calculate's Azure's next move and checks if that move results in a win or tie.

            for (int row = 0; row < gameState.GetLength(0); row++)
            {
                WinBlock(gameState[row], azureSymbol, humanSymbol);


            }

            // Always play the center square if it is available
            if (messagePayload.gameBoard[4] == '?')
            {

                response.move = 4;
            }






            return $"Human's Positions ({messagePayload.humanPlayerSymbol}) = {string.Join(",", humanPositions)} ::::::: Azure's Positions ({messagePayload.azurePlayerSymbol}) = {string.Join(",", azurePositions)}";
        }

        public static int WinBlock(string[] winCondition, string firstSymbol, string secondSymbol)
        {
            if (winCondition[0] == firstSymbol && winCondition[1] == firstSymbol && winCondition[2] != secondSymbol)
            {
                return int.Parse(winCondition[2]);
            }
            else if (winCondition[0] == firstSymbol && winCondition[1] != secondSymbol && winCondition[2] == firstSymbol)
            {
                return int.Parse(winCondition[1]);
            }
            else if (winCondition[0] != secondSymbol && winCondition[1] == firstSymbol && winCondition[2] == firstSymbol)
            {
                return int.Parse(winCondition[0]);
            }
            else
            {
                return 99;
            }

        }
    }
}
