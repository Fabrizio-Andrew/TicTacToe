document.addEventListener('DOMContentLoaded', function () {
    const boardPositions = document.querySelectorAll(".box");
    for (i = 0; i < boardPositions.length; i++) {
        const box = boardPositions[i];
        box.onclick = () => PlayerMove(box);
    }
})

// Global Variables
let humanSymbol = "X";
let azureSymbol = "O";
let URL = "https://localhost:5001/api/executemove";

// Prevents user from submitting another move before azure has responded.
let azureThinking = false;

function PlayerMove(playerPosition) {
    
    if (azureThinking != true) {
        playerPosition.setAttribute('data-value', humanSymbol);
        playerPosition.innerHTML = humanSymbol;
        playerPosition.style.animationPlayState = "running";
        playerPosition.onclick = null;
        azureThinking = true;

        // Post user's move to API and handle response
        fetch(URL, {
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
        azurePosition.innerHTML = azureSymbol;

        if (response.winner == azureSymbol) {
            console.log(`winner: ${azureSymbol}`);

        } else if (response.winner == 'tie') {
            console.log('Tie.')

        } else {
            azureThinking = false;
        }
    }
    else if (response.winner == humanSymbol) {
        console.log(`winner: ${humanSymbol}`);
    }

    else {
        console.log(`Tie.`);
    }

}