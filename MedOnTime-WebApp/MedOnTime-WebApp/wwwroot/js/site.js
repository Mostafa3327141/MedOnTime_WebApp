const NAVBTN = document.getElementById("layout-nav-button"), SIDEPANEL = document.getElementById("layout-side-panel");

// this event is mainly for the side panel's keyframe animation
NAVBTN.addEventListener("click", function () {
    // adding keyframe transition for selection
    if (SIDEPANEL.style.opacity == 0) {
        SIDEPANEL.animate([
            // keyframes
            { opacity: '0%' },
            { opacity: '100%' }
        ], {
            // timing options
            duration: 500
        });
        SIDEPANEL.style.opacity = "100%";
    } else {
        SIDEPANEL.animate([
            // keyframes
            { opacity: '100%' },
            { opacity: '0%' }
        ], {
            // timing options
            duration: 500
        });
        SIDEPANEL.style.opacity = "0%";
    }
}, false);