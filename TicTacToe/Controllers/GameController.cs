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

        // POST api/<GameController>
        [HttpPost]
        public string ExecuteMoveResponse([FromBody] MessagePayload messagePayload)
        {
            return $"what is up, {messagePayload.MessageContent}?";
        }

        // TO-DO: Go through app setup walkthrough again.  Understand why this POST method isn't working.
    }
}
