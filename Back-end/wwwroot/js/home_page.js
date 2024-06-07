// Array containing image paths
let tab = ["~/images/photos/test3.jpg", "~/images/photos/test2.jpg", "~/images/icons/test4.jpg", "~/images/photos/test15.jpg"];

// When the window loads
window.onload = function () {
    // Randomly select two different indexes from the array
    let index1 = Math.floor(Math.random() * 4);
    let index2 = Math.floor(Math.random() * 4);
    // Swap the images at the selected indexes
    if (index1 != index2) {
        let tmp = tab[index1];
        tab[index1] = tab[index2];
        tab[index2] = tmp;
        // Set the src attributes of slider images
        document.getElementById("slider-p1").src = tab[0];
        document.getElementById("slider-p2").src = tab[1];
        document.getElementById("slider-p3").src = tab[2];
        document.getElementById("slider-p4").src = tab[3];

    }
}

// Initialize slide show with first button checked
document.getElementById('btn1').checked = true;
var n = 2;

// Automatic slideshow change every 3 seconds
setInterval(function () {
    document.getElementById('btn' + n).checked = true;
    n++;
    if (n > 4) {
        n = 1;
    }
}, 3000)

// Functions to handle manual slideshow button clicks
function one() { n = 1; }
function two() { n = 2; }
function three() { n = 3; }
function four() { n = 4; }