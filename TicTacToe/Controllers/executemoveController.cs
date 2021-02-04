using TicTacToe.DataTransferObjects;
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
    public class executemoveController : ControllerBase
    {


        // POST api/<GameController>
        [HttpPost]
        public ExecuteMoveResponse ExecuteMoveResponse([FromBody] ExecuteMove messagePayload)
        {
            ExecuteMoveResponse response = CalculateResponse.CalculateMoveResponse(messagePayload);

            return response;

            
        }
    }
}
