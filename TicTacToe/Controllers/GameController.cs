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
        private static readonly int[,] victoryConditions = new int[8,3]
        {
            {0, 1, 2}, {0, 3, 6}, {0, 4, 8}, {1, 4, 7}, {2, 4, 6}, {2, 5, 8}, {3, 4, 5}, {6, 7, 8}
        };

        // POST api/<GameController>
        [HttpPost]
        public string ExecuteMoveResponse([FromBody] ExecuteMove messagePayload)
        {

            // Creates two lists: for Gameboard positions with Xs and Os
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

            return $"Human's Positions ({messagePayload.HumanPlayerSymbol}) = {string.Join(",",humanPositions)} ::::::: Azure's Positions ({messagePayload.AzurePlayerSymbol}) = {string.Join(",",azurePositions)}";
        }

        // TO-DO: Go through app setup walkthrough again.  Understand why this POST method isn't working.
    }
}
