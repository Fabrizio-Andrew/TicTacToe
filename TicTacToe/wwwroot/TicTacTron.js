document.addEventListener('DOMContentLoaded', function () {

    // Set onclick properties for gameboard positions
    const boardPositions = document.querySelectorAll(".box");
    for (i = 0; i < boardPositions.length; i++) {
        const box = boardPositions[i];
        box.onclick = () => PlayerMove(box);
    }

    // Set onclick properties for start menu symbol selection
    const selectBoxes = document.querySelectorAll(".select-box");
    for (i = 0; i < selectBoxes.length; i++) {
        const selectBox = selectBoxes[i];
        console.log(selectBox.id);
        selectBox.onclick = () => SymbolSelect(selectBox.getAttribute('data-value'));
    }

    document.getElementById('music').volume = 0.5;
    document.getElementById('music').play();
})

// API Endpoint
let URI = "/api/executemove";

// Global Variables
let humanSymbol = "?";
let azureSymbol = "?";


// Flag prevents user from submitting another move before azure has responded.
let azureThinking = false;

// Handles the animation and API call for the user's inputs
function PlayerMove(playerPosition) {
    
    if (azureThinking != true) {

        azureThinking = true;

        // Set humanSymbol in space
        playerPosition.setAttribute('data-value', humanSymbol);
        playerPosition.innerHTML = humanSymbol;

        // Run animation & audio
        playerPosition.style.animationPlayState = "running";
        document.getElementById('playermove-audio').play();

        // Prevent player from clicking the same box again
        playerPosition.onclick = null;


        // Post user's move to API and handle response
        fetch(URI, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                move: playerPosition.id,
                azurePlayerSymbol: azureSymbol,
                humanPlayerSymbol: humanSymbol,
                gameBoard: [
                    document.getElementById('0').getAttribute('data-value'),
                    document.getElementById('1').getAttribute('data-value'),
                    document.getElementById('2').getAttribute('data-value'),
                    document.getElementById('3').getAttribute('data-value'),
                    document.getElementById('4').getAttribute('data-value'),
                    document.getElementById('5').getAttribute('data-value'),
                    document.getElementById('6').getAttribute('data-value'),
                    document.getElementById('7').getAttribute('data-value'),
                    document.getElementById('8').getAttribute('data-value'),
                ]
            })
        })
        .then(response => response.json())
        .then(result => {
            console.log(result);
            setTimeout(() => AzureMove(result), 1500);
        });
    }
}

// Handles the render and animation of the azurePlayers move received via API response
function AzureMove(response) {

    if (response.move != null) {
        const azurePosition = document.getElementById(response.move)

        // Set azureSymbol in space
        azurePosition.innerHTML = azureSymbol;
        azurePosition.setAttribute('data-value', azureSymbol)

        // Run animation & audio
        azurePosition.style.color = 'red';
        azurePosition.style.animationName = 'azure-move';
        azurePosition.style.animationPlayState = 'running';
        document.getElementById('azuremove-audio').play();

        // Prevent player from clicking this box
        azurePosition.onclick = null;

        // Display messaging for Azure win or tie
        if (response.winner == azureSymbol) {
            document.getElementById('game-over').innerHTML = '<p>You lose.  End of Line.</p>';
        } else if (response.winner == 'tie') {
            document.getElementById('game-over').innerHTML = '<p>Tie.</p>';
        } else {
            azureThinking = false;
        }
    // Display messaging for human win or tie
    } else if (response.winner == humanSymbol) {
        document.getElementById('game-over').innerHTML = '<p>You win.  End of Line.</p>';
    } else {
        document.getElementById('game-over').innerHTML = '<p>Tie.</p>';
    }
}

function SymbolSelect(symbol) {

    // Set the humanSymbol value
    humanSymbol = symbol;

    // Set azureSymbol value to opposite of humanSymbol.  Hide corresponding select box.
    if (symbol == 'X') {
        document.getElementById('select-box-O').style.display = 'none';
        azureSymbol = 'O';
    } else if (symbol == 'O') {
        document.getElementById('select-box-X').style.display = 'none';
        azureSymbol = 'X';
    }

    // Run animation/audio - hides start menu and displays game board
    document.getElementById('symbol-container').style.animationPlayState = 'running';
    document.getElementById('gamestart-audio').play();

    setTimeout(() => {
        document.getElementById('start').style.display = 'none';
        board = document.getElementById('board');
        board.style.display = 'grid';
        board.style.animationPlayState = 'running';
    }, 4000);
}
