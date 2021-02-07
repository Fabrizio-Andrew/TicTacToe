document.addEventListener('DOMContentLoaded', function() {
    const boardPositions = document.querySelectorAll(".box");
    for (i = 0; i < boardPositions.length; i++) {
        const box = boardPositions[i];
        box.onclick = () => PlayerMove(box);
    }
})

let azureThinking = false;

function PlayerMove(box) {
    
    if (azureThinking != true) {
        box.setAttribute("value", "X");
        box.innerHTML = 'X';
        box.style.animationPlayState = "running";
        azureThinking = true;
        // Fetch goes here
        azureThinking = false;
    }
}