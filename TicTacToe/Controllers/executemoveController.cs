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


        // POST api/<executemoveController>
        [ProducesResponseType(typeof(ExecuteMoveResponse), StatusCodes.Status200OK)] // Tells swagger what the response format will be for a success message
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)] // Tells swagger that the response format will be an int for a BadRequest (400)
        [HttpPost]
        public ActionResult<ExecuteMoveResponse> ExecuteMoveResponse([FromBody] ExecuteMove messagePayload)
        {
            if (PayloadValidation.ValidatePayload(messagePayload) == false)
            {
                return BadRequest(4);
            }

            ExecuteMoveResponse response = CalculateResponse.CalculateMoveResponse(messagePayload);

            return response;

            
        }
    }
}
