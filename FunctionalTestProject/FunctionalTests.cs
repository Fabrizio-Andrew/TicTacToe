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
        // Validate that the application is providing a valid response to a properly-formatted request.
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

            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(StatusCodes.Status200OK, (int)resultObject.Response.StatusCode);

                //Assert.AreEqual(resultPayload.Move, 4);
                //Assert.AreEqual(resultPayload.GameBoard[4], "X");
                //Assert.AreEqual(resultPayload.Winner, "inconclusive");
                //Assert.AreEqual(resultPayload.WinPositions, null);

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task XPlayerWinTestAsync()
        // Validate that the application properly detects a human player "O" victory
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 1,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "O", "X", "O", "?", "X", "?", "?", "X", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;


            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(resultPayload.Winner, "X");

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task OPlayerWinTestAsync()
        // Validate that the application properly detects a human player "O" victory
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "X", "?", "O", "?", "?", "O", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;


            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(resultPayload.Winner, "O");

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task TieTestAsync()
        // Validate that the application properly detects a tie
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "X", "O", "O", "X", "O", "X", "O" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;


            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(resultPayload.Winner, "tie");

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task InconclusiveTestAsync()
        // Validate that the application properly detects an inconclusive move - indicating that the game should continue
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 1,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new List<string> { "X", "O", "X", "O", "?", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);
            ExecuteMoveResponse resultPayload = resultObject.Body as ExecuteMoveResponse;


            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(resultPayload.Winner, "inconclusive");

            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task ValidResponseTestAsync()
        // Validates that the ExecuteMove Response is properly formatted 
        // Validate that the Azure and Human player symbols are either "X" or "O" and are not the same.
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
            if (resultObject != null)
            {
                // Validate that the Azure and Human player symbols are either "X" or "O" and are not the same.
                Assert.IsTrue(resultPayload.AzurePlayerSymbol == "O" || resultPayload.AzurePlayerSymbol == "X");
                Assert.IsTrue(resultPayload.HumanPlayerSymbol == "O" || resultPayload.HumanPlayerSymbol == "X");
                Assert.AreNotEqual(resultPayload.AzurePlayerSymbol, resultPayload.HumanPlayerSymbol);

                // Validate that the gameBoard is the proper size and all values are 'X', 'O', or '?'
                Assert.AreEqual(resultPayload.GameBoard.Count, 9);

                for (int i = 0; i < resultPayload.GameBoard.Count; i++)
                    Assert.IsTrue(resultPayload.GameBoard[i] == "X" || resultPayload.GameBoard[i] == "O" || resultPayload.GameBoard[i] == "?");
            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }


        [TestMethod]
        public async Task BadGameBoardSizeTestAsync()
        // Validate that the application returns a 400 error when the GameBoard length is not 9.
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 2,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "X", "O", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);

            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task BadGameBoardCharTestAsync()
        // Validate that the application returns a 400 error when a character other than X, O, or ? are entered on the GameBoard.
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 2,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "X", "O", "?", "?", "?", "?", "A"}
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);

            // Assert
            if (resultObject != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
            }
            else
            {
                Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
            }
        }

        [TestMethod]
        public async Task PositionCountTestAsync()
        // Validate that the application returns a 400 error if the difference between the HumanPlayer's positions and Azure Player's Positions is greater than 1.
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 2,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "X", "X", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);


            // Assert
            if (resultObject != null)
            {
                // Assert
                if (resultObject != null)
                {
                    Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
                }
                else
                {
                    Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
                }
            }
        }

        [TestMethod]
        public async Task HumanMoveTestAsync()
        // Validate that the application returns a 400 error if the human player's move is not represented on the gameBoard.
        {
            // Arrange 
            ExecuteMove payload = new ExecuteMove()
            {
                Move = 8,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new List<string> { "X", "O", "X", "?", "?", "?", "?", "?" }
            };

            // Act
            HttpOperationResponse<object> resultObject = await _client.ExecuteMoveResponseWithHttpMessagesAsync(payload);


            // Assert
            if (resultObject != null)
            {
                // Assert
                if (resultObject != null)
                {
                    Assert.AreEqual(StatusCodes.Status400BadRequest, (int)resultObject.Response.StatusCode);
                }
                else
                {
                    Assert.Fail("Expected an ExecuteMoveResponse but didn't recieve one.");
                }
            }
        }
    }
}




