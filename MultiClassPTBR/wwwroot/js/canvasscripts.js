var ctx;
var canvas;

window.initMonitor = (dark = false) => {
    canvas = document.getElementById("canvas");
    var wid = canvas.parentElement.clientWidth;
    canvas.setAttribute("width", wid);
    ctx = canvas.getContext("2d");
    
    
};

window.PlotSinglePoint = (move, line) => {

    if (line.y > 0.5) {
        ctx.strokeStyle = "#8a4e4b";
    }
    else if (line.y > 0.7) {
        ctx.strokeStyle = "#891f19";
    }
    else {
        ctx.strokeStyle = "#5b688a";
    }

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