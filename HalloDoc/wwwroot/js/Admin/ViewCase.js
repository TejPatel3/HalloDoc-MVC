
var regextext = /^[a-zA-Z0-9][a-zA-Z0-9 ]+$/i;
var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
var regexphone = /^[1-9][0-9]{9}$/;
console.log("helo");

$('.cancelbtn').on('click', function (e) {
    var name = $(this).attr('value');
    console.log(name)
    $('.pname').html(name);
    var reqid = $(this).attr('id');
    var inputval = $('.requestid');
    inputval.val(reqid);
    $('#exampleModal').modal();
});

$('.viewNotes').on('click', function (e) {
    var requestid = $(this).attr('value');
    console.log("clickit");
    $.ajax({
        url: '/Admin/ViewNotes',
        type: 'GET',
        data: { reqid: requestid },
        success: function (response) {
            $('#nav-home').html(response);
            $('#patientRecordMainDiv').html(response);

        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});
$('#editButton').click(function () {
    $('input[type="email"]').removeAttr('disabled', false);
    $('input[type="tel"]').removeAttr('disabled', false);
    $(this).hide();
    $('#savebtn').removeAttr('hidden', false);
    $('#savebtn').show();
});
$('.ViewCaseSaveBtn').on('click', function (e) {
    console.log("checked for view case edit btn")
    var requestid = $('.requestid').val();
    var email = $('#ViewCasePatientEmail').val();
    var phoneNumber = $('#viewcasephone ').val();
    var confirmationNumber = $('#ViewCaseConfirmationNumber').val();
    let allvalidation = true;
    console.log(email)
    console.log(phoneNumber)
    var ViewModel = {
        requestId: requestid,
        Email: email,
        PhoneNumber: phoneNumber,
        ConfirmationNumber: confirmationNumber,
    };


    $('#email-validationspan-viewcase').html("");
    $('#phone-validationspan-viewcase').html("");

    if (email == "") {
        $('#email-validationspan-viewcase').html("Please Enter Email");
        allvalidation = false;
    }
    else if (!regexemail.test(email)) {
        $('#email-validationspan-viewcase').html("Please Enter Valid Email");
        allvalidation = false;
    }

    if (phoneNumber == "") {
        $('#phone-validationspan-viewcase').html("please enter phone");
        allvalidation = false;
    }
    else if (!regexphone.test(phoneNumber)) {
        $('#phone-validationspan-viewcase').html("please enter 10 digit mobile number");
        allvalidation = false;
    }
    if (allvalidation) {
        console.log(ViewModel)
        $('input[type="email"]').attr('disabled', true);
        $('input[type="tel"]').attr('disabled', true);
        $(this).hide();
        $('#editButton').show();
        $.ajax({
            url: '/Admin/ViewCaseEditData',
            type: 'POST',
            data: ViewModel,
            //dataType: 'json',
            success: function (response) {
                $('#nav-home').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});

//const phoneInputField = document.querySelector("#viewcasephone ");
//const phoneInput = window.intlTelInput(phoneInputField, {
//    utilsScript:
//        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",

var phoneInputField = document.querySelector("#viewcasephone");
var phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
var phoneInputField2 = document.querySelector("#viewcasephone");
var phoneInput = window.intlTelInput(phoneInputField2, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

//$('#ViewCaseBackbtn').click(function () {
//    location.reload();
//});
$('.backbuttonglobal').click(function () {
    location.reload();
});

$('.acceptRequest').on('click', function () {
    console.log("accept")
    var requestid = $(this).val();
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Accept it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ProviderSide/AcceptRequest',
                data: { requestid: requestid },
                success: function (response) {
                    $('#adminLayoutMainDiv').html(response)
                    Swal.fire({
                        title: "Accepted!",
                        text: "Request Accepted Successfully.",
                        icon: "success"
                    });
                }
            })
        }
    });
});

$('.declineRequest').click(function () {
    var requestid = $(this).val();
    Swal.fire({
        title: "Are you sure?",
        text: "You want to Decline Request?\nYou won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Decline it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ProviderSide/DeclineRequest',
                data: { requestid: requestid },
                success: function (response) {
                    $('#adminLayoutMainDiv').html(response)
                    Swal.fire({
                        title: "Accepted!",
                        text: "Request Declined Successfully.",
                        icon: "success"
                    });
                }
            })
        }
    });
});