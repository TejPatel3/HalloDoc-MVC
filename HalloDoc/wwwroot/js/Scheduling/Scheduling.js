var regionid;
var status;
var filterDate = new Date($('#currentDateValue').text());
var currentPartial = "";
window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-provider-tab').addClass('admin-layout-nav-active');
}
function loadSchedulingPartial(PartialName) {
    currentPartial = PartialName;
    $.ajax({
        url: '/Scheduling/LoadSchedulingPartial',
        data: { PartialName: PartialName, date: filterDate.toISOString(), 'regionid': regionid, status: status },
        success: function (data) {
            $(".calander").html(data);
        },
        error: function (e) {
            console.log(e);
        }
    });
}

$(document).ready(function () {
    //$('.region').on('change', function () {
    //    regionid = $(this).val();
    //    $.ajax({
    //        url: '/Scheduling/LoadSchedulingPartial',
    //        data: { PartialName: currentPartial, date: filterDate.toISOString(), 'regionid': regionid },
    //        success: function (data) {
    //            $(".calander").html(data);
    //        },
    //        error: function (e) {
    //            console.log(e);
    //        }
    //    });
    //});
    loadSchedulingPartial('_DayWise');
    $('#prevDateBtn').on('click', function () {
        if (currentPartial == "_MonthWise") {
            var date = filterDate.setMonth(filterDate.getMonth() - 1);
            loadSchedulingPartial(currentPartial);
        }
        else if (currentPartial == "_DayWise") {
            var date = filterDate.setDate(filterDate.getDate() - 1);
            loadSchedulingPartial(currentPartial);
        }
        else if (currentPartial == "_WeekWise") {
            var date = filterDate.setDate(filterDate.getDate() - 7);
            loadSchedulingPartial(currentPartial);
        }
    });

    $('#nextDateBtn').on('click', function () {
        if (currentPartial == "_MonthWise") {
            var date = filterDate.setMonth(filterDate.getMonth() + 1);
            loadSchedulingPartial(currentPartial);
        }
        else if (currentPartial == "_DayWise") {
            var date = filterDate.setDate(filterDate.getDate() + 1);
            loadSchedulingPartial(currentPartial);
        }
        else if (currentPartial == "_WeekWise") {
            var date = filterDate.setDate(filterDate.getDate() + 7);
            loadSchedulingPartial(currentPartial);
        }
    });

    $('.physiciandata').on('change', function (e) {
        var regionid = $(this).val();
        debugger
        console.log(regionid)
        $.ajax({
            url: '/Scheduling/filterregion',
            data: { "regionid": regionid },
            success: function (response) {
                //console.log(response);
                var physelect = $('#physelectschedule');
                //console.log(physelect);
                physelect.empty();
                physelect.append($('<option>', {
                    value: "",
                    text: "Select Physician"
                }))
                $.each(response, function (index, item) {
                    console.log(item)
                    physelect.append($('<option>', {
                        value: item.physicianId,
                        text: item.firstName + item.lastName
                    }));
                });

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });


    $('.repeatchk').on('change', function () {
        if ($(this).prop('checked')) {
            $('.disable').each(function () {
                $(this).prop('disabled', false);
            });
        }
        else {
            $('.disable').each(function () {
                $(this).prop('disabled', true);
            });
        }

    });
});

$('#editbtnviewshiftmodel').click(function () {
    console.log("jdsbhjdbshj")
    $('#viewshiftstartdate').prop('disabled', false);
    $('#viewshiftenddate').prop('disabled', false);
    $('#editbtnviewshiftmodel').addClass('d-none');
    $(this).hide();
    $('#savebtnviewshiftmodel').removeClass('d-none');
});



$('#regionDropDownScheduling').on('change', function () {
    regionid = $(this).val()
    loadSchedulingPartial(currentPartial);

});
$('#endTimeAddShiftModel , #startTimeAddShiftModel').on('change', function () {
    let start = $('#startTimeAddShiftModel').val();
    let end = $('#endTimeAddShiftModel').val();
    console.log("sgvgvd" + start + end)
    if (start != "00:00" && end != "00:00:00.000") {
        const startdate = new Date(`1970-01-01T${start}`);
        const enddate = new Date(`1970-01-01T${end}`);
        let diff = enddate - startdate
        if (diff < 0) {
            Swal.fire({
                title: "Alert",
                text: "Selected End Time can not be earlier from start time ",
                icon: "warning",
            });
            $('#endTimeAddShiftModel').val("");

        }
        else {

            let diffMilliseconds = Math.abs(enddate - startdate);
            let minutes = Math.floor(diffMilliseconds / 60000);

            console.log(minutes)
            if (minutes < 120) {

                Swal.fire({
                    title: "Alert",
                    text: "you can add minimum 2 hour shift",
                    icon: "warning",
                });
                $('#endTimeAddShiftModel').val("");
            }

        }
    }

});
$('#viewshiftstartdate , #viewshiftenddate').on('change', function () {
    let start = $('#viewshiftstartdate').val();
    let end = $('#viewshiftenddate').val();
    console.log("sgvgvd" + start + end)
    if (start != "00:00" && end != "00:00:00.000") {

        let startdate = new Date(`1970-01-01T${start}`);
        let enddate = new Date(`1970-01-01T${end}`);
        let diff = enddate - startdate
        if (diff < 0) {
            Swal.fire({
                title: "Alert",
                text: "Selected End Time can not be earlier from start time ",
                icon: "warning",
            });
            $('#viewshiftenddate').val("");

        }
        else {

            let diffMilliseconds = Math.abs(enddate - startdate);
            let minutes = Math.floor(diffMilliseconds / 60000);

            console.log(minutes)
            if (minutes < 120) {

                Swal.fire({
                    title: "Alert",
                    text: "you can add minimum 2 hour shift",
                    icon: "warning",
                });
                $('#viewshiftenddate').val("");
            }

        }
    }
});

$('#deletebtnviewshiftmodel').on('click', function () {
    let shiftdetailsid = $('#shiftdetailidviewmodel').val()
    console.log("sjhdihj" + shiftdetailsid)
    $.ajax({
        url: '/Scheduling/ViewShiftModelDeletebtn',
        type: 'POST',
        data: {
            shiftdetailsid: shiftdetailsid
        },

        success: function (response) {
            loadSchedulingPartial(currentPartial);
            $('#viewShiftModal').modal('hide');
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });

});

$('#returnbtnviewshiftmodel').on('click', function () {
    let shiftdetailsid = $('#shiftdetailidviewmodel').val()
    console.log("sjhdihj" + shiftdetailsid)
    $.ajax({
        url: '/Scheduling/ViewShiftModelReturnbtn',
        type: 'POST',
        data: {
            shiftdetailsid: shiftdetailsid
        },

        success: function (response) {
            loadSchedulingPartial(currentPartial);
            $('#viewShiftModal').modal('hide');
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });

});

$("#viewShiftForm").submit(function (event) {
    event.preventDefault();
    if ($("#viewShiftForm").valid()) {
        var formData = new FormData(this);
        console.log("djhsbwdhb")

        $.ajax({
            url: '/Scheduling/ViewShiftModelSavebtn',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                loadSchedulingPartial(currentPartial);
                $('#viewShiftModal').modal('hide');
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error while updating physician info:', errorThrown);
            }
        });
    }
});
$('#viewShiftModal').on('hidden.bs.modal', function (e) {
    $(this).remove();
})

$('#pendingShiftbtn , #activeShiftbtn, #allShiftbtn').click(function () {
    status = $(this).val();
    loadSchedulingPartial(currentPartial);
    $('.shiftfilter').removeClass('border-bottom-active');
    $(this).addClass('border-bottom-active');
});
$('#calendar-icon-datepicker').click(function () {
    console.log("cal")
    $('#calendar-hidden').click();
})