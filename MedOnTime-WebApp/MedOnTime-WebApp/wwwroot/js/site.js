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


function timepicker(id) {
    var tpicker = document.getElementById(id),
        tbl = document.createElement("table"),
        tblBody = document.createElement("tbody"),
        row = document.createElement("tr"),
        hours = document.createElement("th"),
        minutes = document.createElement("th");

    hours.appendChild(document.createTextNode("Hours"));
    hours.setAttribute("colspan", "5");
    minutes.appendChild(document.createTextNode("Minutes"));
    minutes.setAttribute("colspan", "4");
    row.appendChild(hours);
    row.appendChild(minutes);

    var format,
        hrs,
        mins,
        _tim,
        _class,
        _text,
        _i;

    tblBody.appendChild(row);

    for (var i = 0; i < 6; i++) {
        row = document.createElement("tr");
        if (i === 0 || i === 3) {
            format = document.createElement("td");
             // this is for AM/PM time
            if (i === 0) {
                _tim = 'AM';
                _class = 'hrs am';
            } else {
                _tim = 'PM';
                _class = 'hrs pm';
            }
            format.appendChild(document.createTextNode(_tim));
            format.setAttribute("rowspan", "3");
            format.setAttribute("class", "timaFormat");
            row.appendChild(format);
        }
        for (var _h = 1; _h <= 4; _h++) {
            if (i < 3) {
                _i = i;
            } else {
                _i = i - 3;
            }
            _text = _i * 4 + _h;
            if (_text < 10) _text = '0' + _text;
            hrs = document.createElement("td");
            hrs.appendChild(document.createTextNode(_text));
            hrs.setAttribute("class", _class);
            hrs.setAttribute("data-time", _tim);
            row.appendChild(hrs);
        }

        if (i === 0 || i == 2 || i == 4) {
            for (var _m = 0; _m < 4; _m++) {
                _text = 5 * _m + i * 10;
                if (_text < 10) _text = '0' + _text;

                mins = document.createElement("td");
                mins.appendChild(document.createTextNode(_text));
                mins.setAttribute("class", "minutes");
                mins.setAttribute("rowspan", "2");
                row.appendChild(mins);
            }
        }
        tblBody.appendChild(row);
    }
    tbl.appendChild(tblBody);
    tpicker.appendChild(tbl);
    tbl.setAttribute("class", "timepicker");
}
if ($('#timepicker').get(0)) {
    window.onload = timepicker('timepicker');
}
var hours,
    min = null,
    isAm = false;

$(document).on('click', '.hrs', function () {
    hours = $(this).html();
    setDate();
    return false;
});

$(document).on('click', '.minutes', function () {
    min = $(this).html();
    setDate();
    return false;
});
$(document).on('click', '.am', function () {
    isAm = true;
    setDate();
    return false;
});
$(document).on('click', '.pm', function () {
    isAm = false;
    setDate();
});

/**
 *  @function setDate formats the numbers for hours and minutes picked in AM/PM time 
 */
function setDate() {
    var timeFormat;
    if (isAm) {
        timeFormat = ' AM';
    } else {
        timeFormat = ' PM';
    }

    if (hours) {
        if (min) {
            $('#timepicker input').val(hours + ':' + min + timeFormat);
        } else {
            $('#timepicker input').val(hours + ':00' + timeFormat);
        }
    } else {
        if (min) {
            $('#timepicker input').val('00:' + min + timeFormat);
        } else {
            $('#timepicker input').val();
        }
    }
}
$(document).on('click', 'div.timepicker input', function (e) {
    e.stopPropagation();
    $(this).parent().find('table.timepicker').show();
});
$(document).on('click', function () {
    $('table.timepicker').hide();
});