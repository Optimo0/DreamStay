var zegar = new Date();
var hour = zegar.getHours();
var minute = zegar.getMinutes();
var second = zegar.getSeconds();

function timer() {
    zegar = new Date();
    hour = zegar.getHours();
    minute = zegar.getMinutes();
    second = zegar.getSeconds();

    if (minute < 10) {
        minute = "0" + zegar.getMinutes();
    }

    if (second < 10) {
        second = "0" + zegar.getSeconds();
    }

    if (hour < 10) {
        hour = "0" + zegar.getHours();
    }

    document.getElementById("zegar").innerHTML = hour + ":" + minute + ":" + second

}

setInterval(timer, 1000);