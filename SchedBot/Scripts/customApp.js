function removeDisabledAttFromTimePickers() {
    document.getElementById("timepicker").readOnly = false;
    document.getElementById("timepicker2").readOnly = false;
}

function fixTimepickerScrollbar() {
    //hide the scrollbars that were being forced outside the bottom of the modal from showing
    $("#timepicker_root > .picker__holder").hide();
    $("#timepicker2_root > .picker__holder").hide();

    $("#timepicker").click(function () {
        $("#timepicker_root > .picker__holder").show();
        $(".picker__holder").click(function () {
            $("#timepicker_root > .picker__holder").hide();
        })
    })

    $("#timepicker2").click(function () {
        $("#timepicker2_root > .picker__holder").show();
        $(".picker__holder").click(function () {
            $("#timepicker2_root > .picker__holder").hide();
        })
    })
}