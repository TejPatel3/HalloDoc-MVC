
var phoneInputField = document.querySelector("#phone-adminprofile");
var phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
var phoneInputField2 = document.querySelector("#alterphonenumber-adminprofile");
var phoneInput = window.intlTelInput(phoneInputField2, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
$('#resetpassword-adminprofile').click(function () {
    console.log("reset pass")
    $('#password-adminprofile').removeAttr('disabled',false)
    $('#savepassword-adminprofile').removeAttr('hidden', false);
    $('#savepassword-adminprofile').show();
    $(this).hide();
   
});
$('#savepassword-adminprofile').click(function () {
    console.log("reset pass")
    var password = $('#password-adminprofile').val();
    var email = $('#email-adminprofile').val();
    $('#password-adminprofile').attr('disabled', true);
    $('#resetpassword-adminprofile').show();
    $(this).hide();
    $.ajax({
        url: '/AdminCredential/ResetPassword',
        type: 'POST',
        data: { email: email, password: password },
        success: function (response) {
            //$('#nav-profile').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
    
});
$('#editbtn-administrator-adminprofile').click(function () {
    console.log("reset pass")
    var password = $('#password-adminprofile').val();
    var email = $('#email-adminprofile').val();
    $('.adisabled').removeAttr('disabled', false);

    $('#savebtn-administrator-adminprofile').removeAttr('hidden', false);
    $('#savebtn-administrator-adminprofile').show();
    $(this).hide();

    // $.ajax({
    //     url: '/AdminCredential/ResetPassword',
    //     type: 'POST',
    //     data: { email: email, password: password },
    //     success: function (response) {
    //         $('#nav-profile').html(response);
    //     },
    //     error: function (xhr, status, error) {
    //         console.error(error);
    //     }
    // });
});
$('#savebtn-administrator-adminprofile').click(function () {
    console.log("reset pass")
    var password = $('#password-adminprofile').val();
    var email = $('#email-adminprofile').val();
    var confirmemail = $('#confirmemail-adminprofile').val();
    var phone = $('#phone-adminprofile').val();
    var firstname = $('#firstname-adminprofile').val();
    var lastname = $('#lastname-adminprofile').val();
    var selectedregion = [];
    $('input[type="checkbox"]:checked').each(function () {
        selectedregion.push($(this).val());
    });
    console.log(selectedregion)
    var model = {
        firstname: firstname,
        lastname: lastname,
        phoneNUMBER: phone,
        email: email,
        selectedregion: selectedregion,
    }

    if (email != confirmemail) {
        $('#confirmemail-validationspan').html("Email and Confirm Email is not Match");
    }
    else {
        $('#confirmemail-validationspan').html("");
        $('.adisabled').attr('disabled', true);
        $('#editbtn-administrator-adminprofile').show();
        $(this).hide();

        $.ajax({
            url: '/Admin/UpdateAdministrationInfoAdminProfile',
            type: 'POST',
            //dataType: 'json',
            data: model,
            success: function (response) {
                //$('#nav-profile').html(response);
            },


            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});
$('#editbtn-mailing-adminprofile').click(function () {
    console.log("reset pass")
    var password = $('#password-adminprofile').val();
    var email = $('#email-adminprofile').val();
    $('.mdisabled').removeAttr('disabled', false);

    $('#savebtn-mailing-adminprofile').removeAttr('hidden', false);
    $('#savebtn-mailing-adminprofile').show();
    $(this).hide();

    // $.ajax({
    //     url: '/AdminCredential/ResetPassword',
    //     type: 'POST',
    //     data: { email: email, password: password },
    //     success: function (response) {
    //         $('#nav-profile').html(response);
    //     },
    //     error: function (xhr, status, error) {
    //         console.error(error);
    //     }
    // });
});
$('#savebtn-mailing-adminprofile').click(function () {
    console.log("reset pass")
    var email = $('#email-adminprofile').val();
    var address1 = $('#address1-adminprofile').val();
    var address2 = $('#address2-adminprofile').val();
    var zip = $('#zip-adminprofile').val();
    var city = $('#city-adminprofile').val();
    var alterphonenumber = $('#alterphonenumber-adminprofile').val();
    $('.mdisabled').attr('disabled', true);

    $('#editbtn-mailing-adminprofile').show();
    $(this).hide();
    var model = {
        address1: address1,
        address2: address2,
        city: city,
        zip: zip,
        alterphonenumber : alterphonenumber,
    }


    $('#confirmemail-validationspan').html("");



    $.ajax({
        url: '/Admin/UpdateMailingInfoAdminProfile',
        type: 'POST',
        //dataType: 'json',
        data: model,
        success: function (response) {
            //$('#nav-profile').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

});

    