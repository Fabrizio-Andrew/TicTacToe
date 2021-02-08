document.addEventListener('DOMContentLoaded', function () {
    const boardPositions = document.querySelectorAll(".box");
    for (i = 0; i < boardPositions.length; i++) {
        const box = boardPositions[i];
        box.onclick = () => PlayerMove(box);
    }
    const selectBoxes = document.querySelectorAll(".select-box");
    for (i = 0; i < selectBoxes.length; i++) {
        const selectBox = selectBoxes[i];
        console.log(selectBox.id);
        selectBox.onclick = () => SymbolSelect(selectBox.getAttribute('data-value'));
    }
})

// API Endpoint
let URI = "https://localhost:5001/api/executemove";

// Global Variables
let humanSymbol = "?";
let azureSymbol = "?";


// Prevents user from submitting another move before azure has responded.
let azureThinking = false;

function PlayerMove(playerPosition) {
    
    if (azureThinking != true) {
        playerPosition.setAttribute('data-value', humanSymbol);
        playerPosition.innerHTML = humanSymbol;
        playerPosition.style.animationPlayState = "running";
        document.getElementById('playermove-audio').play();
        playerPosition.onclick = null;
        azureThinking = true;

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

function AzureMove(response) {

    if (response.move != null) {
        const azurePosition = document.getElementById(response.move)

        azurePosition.setAttribute('data-value', azureSymbol)
        azurePosition.onclick = null;

        azurePosition.style.color = 'red';
        azurePosition.style.animationName = 'azure-move';
        azurePosition.style.animationPlayState = 'running';
        document.getElementById('azuremove-audio').play();
        azurePosition.innerHTML = azureSymbol;

        if (response.winner == azureSymbol) {
            document.getElementById('game-over').innerHTML = '<p>You lose.  End of Line.</p>';
        } else if (response.winner == 'tie') {
            document.getElementById('game-over').innerHTML = '<p>Tie.</p>';
        } else {
            azureThinking = false;
        }
    }
    else if (response.winner == humanSymbol) {
        document.getElementById('game-over').innerHTML = '<p>You win.  End of Line.</p>';
    }

    else {
        document.getElementById('game-over').innerHTML = '<p>Tie.</p>';
    }
}

function SymbolSelect(symbol) {

    humanSymbol = symbol;
    console.log(symbol);

    if (symbol == 'X') {
        document.getElementById('select-box-O').style.display = 'none';
        azureSymbol = 'O';
    } else if (symbol == 'O') {
        document.getElementById('select-box-X').style.display = 'none';
        azureSymbol = 'X';
    }

    document.getElementById('symbol-container').style.animationPlayState = 'running';
    document.getElementById('gamestart-audio').play();

    setTimeout(() => {
        document.getElementById('start').style.display = 'none';
        board = document.getElementById('board');
        board.style.display = 'grid';
        board.style.animationPlayState = 'running';
    }, 4000);
}
