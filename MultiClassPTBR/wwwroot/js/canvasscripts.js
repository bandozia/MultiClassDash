var ctx;
var canvas;

window.initMonitor = (dark = false) => {
    canvas = document.getElementById("canvas");
    var wid = canvas.parentElement.clientWidth;
    canvas.setAttribute("width", wid);
    ctx = canvas.getContext("2d");
    if (dark) {
        ctx.strokeStyle = "#FFFFFF";    
    }
    else {
        ctx.strokeStyle = "#333333";
    }
    
};

window.PlotSinglePoint = (move, line) => {
    ctx.moveTo(move.x, canvas.height - (move.y * 100));
    ctx.lineTo(line.x, canvas.height - (line.y * 100));    
    ctx.stroke();   
};

window.GetMonitorWidth = () => {
    return canvas.width;
}

window.ClearCanvas = () => {
    ctx.beginPath();
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}