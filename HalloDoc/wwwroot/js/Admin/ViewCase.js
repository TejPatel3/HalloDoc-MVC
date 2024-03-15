

console.log("helo");

    $('.cancelbtn').on('click', function (e) {
        var name = $(this).attr('value');
        console.log(name)
        $('.pname').html(name);
        var reqid = $(this).attr('id');
        var inputval = $('.requestid');
        inputval.val(reqid);
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
        console.log(email)
        console.log(phoneNumber)
        var ViewModel = {
            requestId: requestid,
            Email: email,
            PhoneNumber: phoneNumber,
            ConfirmationNumber: confirmationNumber,
        };
      
        console.log(ViewModel)
        $('input[type="email"]').attr('disabled', true);
        $('input[type="tel"]').attr('disabled', true);
        $(this).hide();
        $('#editButton').show();
        $.ajax({
            url: '/Admin/ViewCaseEditData',
            type: 'POST',
            data: ViewModel,
            dataType: 'json',
            success: function (response) {
                $('#nav-home').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });


//const phoneInputField = document.querySelector("#viewcasephone ");
//const phoneInput = window.intlTelInput(phoneInputField, {
//    utilsScript:
//        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",

var phoneInputField = document.querySelector("#viewcasephone ");
var phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
var phoneInputField2 = document.querySelector("#viewcasephone ");
var phoneInput = window.intlTelInput(phoneInputField2, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
