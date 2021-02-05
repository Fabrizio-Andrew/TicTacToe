﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace RestClientSDKLibrary.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    /// <summary>
    /// Defines the message payload representing the player's move.
    /// </summary>
    public partial class ExecuteMove
    {
        /// <summary>
        /// Initializes a new instance of the ExecuteMove class.
        /// </summary>
        public ExecuteMove() { }

        /// <summary>
        /// Initializes a new instance of the ExecuteMove class.
        /// </summary>
        public ExecuteMove(int? move = default(int?), string azurePlayerSymbol = default(string), string humanPlayerSymbol = default(string), IList<string> gameBoard = default(IList<string>))
        {
            Move = move;
            AzurePlayerSymbol = azurePlayerSymbol;
            HumanPlayerSymbol = humanPlayerSymbol;
            GameBoard = gameBoard;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "move")]
        public int? Move { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "azurePlayerSymbol")]
        public string AzurePlayerSymbol { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "humanPlayerSymbol")]
        public string HumanPlayerSymbol { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gameBoard")]
        public IList<string> GameBoard { get; set; }

    }
}
