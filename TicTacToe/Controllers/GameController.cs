using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        // Defines all 8 possible winning combinations in Tic Tac Toe
        private static string[,] victoryConditions = new string[8,3]
        {
            {"0", "1", "2"}, {"0", "3", "6"}, {"0", "4", "8"}, {"1", "4", "7"}, {"2", "4", "6"}, {"2", "5", "8"}, {"3", "4", "5"}, {"6", "7", "8"}
        };

        // POST api/<GameController>
        [HttpPost]
        public string ExecuteMoveResponse([FromBody] ExecuteMove messagePayload)
        {

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

            foreach (int i in humanPositions)
            {
                for (int row = 0; row < victoryConditions.GetLength(0); row++)
                {
                    for (int column = 0; column < victoryConditions.GetLength(1); column++)
                    {
                        if (i.ToString() == victoryConditions[row, column])
                        {
                            victoryConditions[row, column] = humanSymbol;
                        }
                    }

                    // Check to see if the human has met any of the victory conditions
                    if (victoryConditions[row, 0] == humanSymbol && victoryConditions[row,1] == humanSymbol && victoryConditions[row,2] == humanSymbol)
                    {
                        return "Human won.";
                    }
                }
            }

            // Compare victoryConditions to azurePositions.
            // Replace any values in victoryConditions with azureSymbol if they match (indicating that Azure owns that space)

            foreach (int i in azurePositions)
            {
                for (int row = 0; row < victoryConditions.GetLength(0); row++)
                {
                    for (int column = 0; column < victoryConditions.GetLength(1); column++)
                    {
                        if (i.ToString() == victoryConditions[row, column])
                        {
                            victoryConditions[row, column] = azureSymbol;
                        }
                    }
                }
            }


            return $"Human's Positions ({messagePayload.HumanPlayerSymbol}) = {string.Join(",",humanPositions)} ::::::: Azure's Positions ({messagePayload.AzurePlayerSymbol}) = {string.Join(",",azurePositions)}";
        }
    }
}
