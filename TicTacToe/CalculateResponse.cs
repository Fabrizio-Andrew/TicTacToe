using TicTacToe.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class CalculateResponse
    // Contains all business logic for the game of Tic Tac Toe
    {

        public static ExecuteMoveResponse CalculateMoveResponse(ExecuteMove messagePayload)
        // Calculates
        {


            int[,] victoryConditions = new int[8, 3]
            // Defines all 8 possible winning combinations in Tic Tac Toe
            {
                {0, 1, 2}, {0, 3, 6}, {0, 4, 8}, {1, 4, 7}, {2, 4, 6}, {2, 5, 8}, {3, 4, 5}, {6, 7, 8}
            };

            string[,] gameState = new string[8, 3]
            // Tracks player positions against possible victories
            {
                {"0", "1", "2"}, {"0", "3", "6"}, {"0", "4", "8"}, {"1", "4", "7"}, {"2", "4", "6"}, {"2", "5", "8"}, {"3", "4", "5"}, {"6", "7", "8"}
            };

            //========== 1. SET UP THE GAME BOARD ==========
            // This section sets Human/Azure positions on an array called "gameState".  It also checks if the Human has won or the game is tied.
            // (We can assume Azure has not won before making its move.)

            // Prepare known elements of Azure's response
            ExecuteMoveResponse response = new ExecuteMoveResponse()
            {
                azurePlayerSymbol = messagePayload.azurePlayerSymbol,
                humanPlayerSymbol = messagePayload.humanPlayerSymbol,
                gameBoard = messagePayload.gameBoard
            };


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

            // Compare gameState to humanPositions.
            // Replace any values in gameState with humanSymbol if they match (indicating that the human owns that space)
            string humanSymbol = messagePayload.humanPlayerSymbol.ToString();

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
                        response.winner = humanSymbol;

                        // Set winPositions
                        response.winPositions = new int[3];
                        for (int j = 0; j < 3; j++)
                        {
                            response.winPositions[j] = victoryConditions[row, j];
                        }

                        return response;
                    }
                }
            }

            // Check for a tie game resulting from Player's move
            if (Array.IndexOf(messagePayload.gameBoard, '?') == -1)
            {
                response.winner = "tie";
                return response;
            }


            // Compare gameState to azurePositions.
            // Replace any values in gameState with azureSymbol if they match (indicating that Azure owns that space)
            string azureSymbol = messagePayload.azurePlayerSymbol.ToString();

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

            // Look for any possible winning moves or blocking moves
            for (int row = 0; row < gameState.GetLength(0); row++)
            {
                string[] gameStateRow = { gameState[row, 0], gameState[row, 1], gameState[row, 2] };

                // Winning moves
                int? winningMove = WinBlock(gameStateRow, azureSymbol, humanSymbol);

                if (winningMove != null)
                {
                    response.move = winningMove;
                    response.gameBoard[(int)winningMove] = messagePayload.azurePlayerSymbol;
                    response.winner = azureSymbol;
                    response.winPositions = new int[3];
                    for (int j = 0; j < 3; j++)
                    {
                        response.winPositions[j] = victoryConditions[row, j];
                    }
                    return response;
                }

                // Blocking moves
                int? blockingMove = WinBlock(gameStateRow, humanSymbol, azureSymbol);

                if (blockingMove != null)
                {
                    response.move = blockingMove;
                    response.gameBoard[(int)blockingMove] = messagePayload.azurePlayerSymbol;
                }
            }
            
            // If no winning or blocking moves have been found, select the first available move from a predetermined order of priority
            if (response.move == null)
            {
                int[] movePriority = { 4, 8, 6, 2, 0, 7, 5, 3, 1 };
                List<int> occupiedPositions = humanPositions.Concat(azurePositions).ToList();

                for (int i = 0; i < movePriority.Length; i++)
                {
                    if (occupiedPositions.IndexOf(movePriority[i]) == -1)
                    {
                        response.move = movePriority[i];
                        response.gameBoard[movePriority[i]] = messagePayload.azurePlayerSymbol;
                        break;
                    }
                }
            }
        
            

            // Check for a tie game resulting from Azure's move
            if (Array.IndexOf(messagePayload.gameBoard, '?') == -1)
            {
                response.winner = "tie";
                return response;
            }

            response.winner = "inconclusive";
            return response;
        }

        public static int? WinBlock(string[] winCondition, string firstSymbol, string secondSymbol)
        // The WinBlock method finds any victory condition where 2/3 squares are taken by the same player and the third is open.
        // If no such situation exists, returns null.

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
                return null;
            }
        }
    }
}
