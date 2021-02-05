using Microsoft.AspNetCore.Http;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClientSDKLibrary;
using RestClientSDKLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalTestProject
{
    [TestClass]
    public class FunctionalTests
    {
        ServiceClientCredentials _serviceClientCredentials;

        RestClientSDKLibraryClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            _client = new RestClientSDKLibraryClient(
                new Uri("https://localhost:5001/"), _serviceClientCredentials);
        }

        [TestMethod]
        public async Task HappyPathTestAsync()
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "?", "?", "?", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;

            // Assert
            if (resultPayload != null)
            {
                Assert.AreEqual(StatusCodes.Status200OK, (int)resultObject.Response.StatusCode);

                Assert.AreEqual(resultPayload.Move, 4);
                Assert.AreEqual(resultPayload.GameBoard.Count, 9);
                Assert.AreEqual(resultPayload.GameBoard[4], "X");
                Assert.AreEqual(resultPayload.Winner, "inconclusive");
                Assert.AreEqual(resultPayload.WinPositions, null);

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task PlayerSymbolsTestAsync()
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 2,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "X", "O", "?", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;

            // Assert
            if (resultPayload != null)
            {
                Assert.AreNotEqual(resultPayload.AzurePlayerSymbol, resultPayload.HumanPlayerSymbol);
                Assert.AreEqual(resultPayload.AzurePlayerSymbol, "O");
                Assert.AreEqual(resultPayload.HumanPlayerSymbol, "X");
            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }
    }
}

// Validate that symbols are 'X' or 'O' and are different
//if (messagePayload.humanPlayerSymbol != 'X' && messagePayload.humanPlayerSymbol != 'O')
//{
//    return false;
//}

//if (messagePayload.azurePlayerSymbol != 'X' && messagePayload.azurePlayerSymbol != 'O')
//{
//   return false;
//}

//if (messagePayload.azurePlayerSymbol == messagePayload.humanPlayerSymbol)
//{
//return false;
//}

// Validate that the gameBoard is the proper size and all values are 'X', 'O', or '?'
//if (messagePayload.gameBoard.Length > 9 || messagePayload.gameBoard.Length < 9)
//{
//return false;
//}

//for (int i = 0; i < messagePayload.gameBoard.Length; i++)
//{
//if (messagePayload.gameBoard[i] != 'X' && messagePayload.gameBoard[i] != 'O' && messagePayload.gameBoard[i] != '?')
//{
//return false;
//    }
//}


// Validate that the difference between the number of X's and O's is not greater than 1
//int xCount = 0;
//int oCount = 0;

//for (int i = 0; i < messagePayload.gameBoard.Length; i++)
//{
//if (messagePayload.gameBoard[i] == 'X')
//{
//xCount++;
//    }
//else if (messagePayload.gameBoard[i] == 'O')
//{
//oCount++;
//   }
//}
//if ((xCount - oCount) > 1 || (xCount - oCount) < -1)
//{
//return false;
//}


// Validate that the player's move is represented on the gameBoard.  If the player did not move, gameBoard must be all ?'s.
//if (messagePayload.move == null)
//{
//for (int i = 0; i < messagePayload.gameBoard.Length; i++)
//{
//if (messagePayload.gameBoard[i] != '?')
//{
//return false;
//       }
//    }
//}
//else
//{
//if (messagePayload.gameBoard[(int)messagePayload.move] != messagePayload.humanPlayerSymbol)
//{
//return false;
//    }
//}
