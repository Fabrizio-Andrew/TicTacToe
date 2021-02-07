document.addEventListener('DOMContentLoaded', function() {
    const boardPositions = document.querySelectorAll(".box");
    console.log(boardPositions);
    for (i = 0; i < boardPositions.length; i++) {
        const box = boardPositions[i];
        box.onclick = () => PlayerMove(box);
        console.log(box);
    }
})

function PlayerMove(box) {
    console.log(box.id);
    box.innerHTML = 'X';
    box.style.animationPlayState = "running";
}