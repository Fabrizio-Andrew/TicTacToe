using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class CalculateResponse {
        //public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        //{
        //    return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        //}

        public static string CalculateMoveResponse(ExecuteMove messagePayload) {
            
            // Defines all 8 possible winning combinations in Tic Tac Toe
            string[,] victoryConditions = new string[8, 3]
            {
                {"0", "1", "2"}, {"0", "3", "6"}, {"0", "4", "8"}, {"1", "4", "7"}, {"2", "4", "6"}, {"2", "5", "8"}, {"3", "4", "5"}, {"6", "7", "8"}
            };

            // Creates two lists: for Gameboard human and azure player positions
            List<int> humanPositions = new List<int>();
            List<int> azurePositions = new List<int>();

            for (int i = 0; i < messagePayload.Gameboard.Length; i++)
            {
                if (messagePayload.Gameboard[i] == messagePayload.HumanPlayerSymbol)
                {
                    humanPositions.Add(i);
                }
                else if (messagePayload.Gameboard[i] == messagePayload.AzurePlayerSymbol)
                {
                    azurePositions.Add(i);
                }
            }

            string humanSymbol = messagePayload.HumanPlayerSymbol.ToString();
            string azureSymbol = messagePayload.AzurePlayerSymbol.ToString();

            // Compare victoryConditions to humanPositions.
            // Replace any values in victoryConditions with humanSymbol if they match (indicating that the human owns that space)

            string[,] gameState = (string[,])victoryConditions.Clone();

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
                        string winPositions = $"({victoryConditions[row, 0]}, {victoryConditions[row, 1]}, {victoryConditions[row, 2]})";
                        return $"Human won. Winning Positions = {winPositions}";
                    }
                }
            }

            // Compare victoryConditions to azurePositions.
            // Replace any values in victoryConditions with azureSymbol if they match (indicating that Azure owns that space)

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

            return $"Human's Positions ({messagePayload.HumanPlayerSymbol}) = {string.Join(",", humanPositions)} ::::::: Azure's Positions ({messagePayload.AzurePlayerSymbol}) = {string.Join(",", azurePositions)}";
        }
    }
}
