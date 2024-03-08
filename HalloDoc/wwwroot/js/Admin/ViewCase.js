
    $(document).ready(function () {
        $('.canclebtn').on('click', function (e) {
            var name = $(this).attr('value');
            console.log(name)
            $('.pname').html(name);
            var reqid = $(this).attr('id');
            var inputval = $('.requestid');
            inputval.val(reqid);
        });

    $('#editButton').click(function () {
        $('input[type="email"]').removeAttr('disabled', false);
    $('input[type="tel"]').removeAttr('disabled', false);
    $(this).hide();
    $('#savebtn').removeAttr('hidden', false);
        });

    $('.viewNotes').on('click', function (e) {
            var requestid = $(this).attr('value');
    console.log(requestid)
    $.ajax({
        url: '/Admin/ViewNotes',
    type: 'GET',
    data: {reqid: requestid },
    success: function (response) {
        $('#nav-home').html(response);
                },
    error: function (xhr, status, error) {
        console.error(error);
                }
            });
           
        });

    $('.ViewCaseSaveBtn').on('click', function (e) {
        console.log("checked for view case edit btn")
            var requestid = @Model.requestId;
    var email = $('#ViewCasePatientEmail').val();
    var phoneNumber = $('#home').val();
    var confirmationNumber = $('#ViewCaseConfirmationNumber').val();
    console.log(email)
    console.log(phoneNumber)
    var ViewModel = {
        requestId: requestid,
    Email: email,
    PhoneNumber: phoneNumber,
    ConfirmationNumber : confirmationNumber,
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
    });

    const phoneInputField = document.querySelector("#phone");
    const phoneInput = window.intlTelInput(phoneInputField, {
        utilsScript:
    "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
