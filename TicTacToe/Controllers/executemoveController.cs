using TicTacToe.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicTacToe.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class executemoveController : ControllerBase
    {


        /// <summary>
        /// Facilitates the game of Tic Tac Toe by accepting a JSON input representing the human player's move and returning a JSON object with Azure's move
        /// </summary>
        /// <param name="inputPayload">The input payload representing human player's move.</param>
        /// <returns>
        /// A JSON object representing Azure's move and indicating if there is a winner.
        /// </returns>
        /// <response code="200">Indicates the request was successful</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExecuteMoveResponse), StatusCodes.Status200OK)] // Tells swagger what the response format will be for a success message
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)] // Tells swagger that the response format will be an int for a BadRequest (400)
        public ActionResult<ExecuteMoveResponse> ExecuteMoveResponse([FromBody] ExecuteMove inputPayload)
        {
            if (PayloadValidation.ValidatePayload(inputPayload) == false)
            {
                return BadRequest(4);
            }

            ExecuteMoveResponse response = CalculateResponse.CalculateMoveResponse(inputPayload);

            return response;

            
        }
    }
}
