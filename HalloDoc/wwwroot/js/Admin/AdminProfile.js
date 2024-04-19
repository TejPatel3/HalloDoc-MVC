var regexstrongpassword = /[A-Za-z\d@$!%*?&]{8,}/
var phoneInputField = document.querySelector("#alterphonenumber-adminprofile");

var phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
var phoneInputField2 = document.querySelector("#phone-adminprofile");
var phoneInput = window.intlTelInput(phoneInputField2, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-profile-tab').addClass('admin-layout-nav-active');
    $('#resetpassword-adminprofile').click(function () {
        console.log("reset pass")
        $('#password-adminprofile').removeAttr('disabled', false)
        $('#savepassword-adminprofile').removeAttr('hidden', false);
        $('#savepassword-adminprofile').show();
        $(this).hide();
    });
    $('#savepassword-adminprofile').click(function () {
        console.log("reset pass")
        var password = $('#password-adminprofile').val();
        var email = $('#email-adminprofile').val();
        $('#AdminProfileResetPasswordSpan').html("");
        if (password == "") {
            $('#AdminProfileResetPasswordSpan').html("Please enter password");
        }
        else if (!regexstrongpassword.test(password)) {
            alert("Please Enter Strong Password that contain\nAt least one uppercase letter\nAt least one lowercase letter\nAt least one digit\nAt least one special character\nAt least 8 characters in length")
        }
        else {
            $('#password-adminprofile').attr('disabled', true);
            $('#resetpassword-adminprofile').show();
            $(this).hide();
            $.ajax({
                url: '/AdminCredential/ResetPassword',
                type: 'POST',
                data: { email: email, password: password },
                success: function (response) {
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        }
    });
    $('#editbtn-administrator-adminprofile').click(function () {
        console.log("reset pass")
        var password = $('#password-adminprofile').val();
        var email = $('#email-adminprofile').val();
        $('.adisabled').removeAttr('disabled', false);
        $('#savebtn-administrator-adminprofile').removeAttr('hidden', false);
        $('#savebtn-administrator-adminprofile').show();
        $(this).hide();
    });
    $('#email-adminprofile').on('input', function () {
        var email = $('#email-adminprofile').val();
        $('#confirmemail-validationspan').html("");
        $('#email-validationspan').html("");
        if (email == "") {
            $('#email-validationspan').html("Please Enter Email");
            allvalidation = false;
        }
        else if (!regexemail.test(email)) {
            $('#email-validationspan').html("Please Enter Valid Email");
        }
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
        var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
        var regexphone = /^[1-9][0-9]{9}$/;
        var regexname = /^[a-zA-Z]+$/i;
        var allvalidation = true;
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
        $('#firstname-validationspan').html("");
        $('#lastname-validationspan').html("");
        $('#email-validationspan').html("");
        $('#confirmemail-validationspan').html("");
        $('#phone-validationspan').html("");
        if (firstname == "") {
            $('#firstname-validationspan').html("please enter firstname");
            allvalidation = false;
        }
        else if (!regexname.test(firstname)) {
            $('#firstname-validationspan').html("please enter valid name");
            allvalidation = false;
        }
        else {
            $('#firstname-validationspan').html("");
        }
        if (lastname == "") {
            $('#lastname-validationspan').html("please enter lastname");
            allvalidation = false;
        }
        else if (!regexname.test(lastname)) {
            $('#lastname-validationspan').html("please enter valid name");
            allvalidation = false;
        }
        else {
            $('#lastname-validationspan').html("");
        }
        if (email == "") {
            $('#email-validationspan').html("Please Enter Email");
            allvalidation = false;
            $('#confirmemail-validationspan').html("");
        }
        else if (!regexemail.test(email)) {
            $('#email-validationspan').html("Please Enter Valid Email");
            $('#confirmemail-validationspan').html("");
            allvalidation = false;
        }
        else {
            $('#email-validationspan').html("");
        }
        if (email != confirmemail) {
            $('#confirmemail-validationspan').html("Email and Confirm Email is not Match");
            allvalidation = false;
        }
        if (phone == "") {
            $('#phone-validationspan').html("please enter phone");
            allvalidation = false;
        }
        else if (!regexphone.test(phone)) {
            $('#phone-validationspan').html("please enter 10 digit mobile number");
            allvalidation = false;
        }
        if (selectedregion == "") {
            $('#selectedregion-validationspan').html("please select atleast one region");
            allvalidation = false;
        }
        if (allvalidation) {
            $('#confirmemail-validationspan').html("");
            $('.adisabled').attr('disabled', true);
            $('#editbtn-administrator-adminprofile').show();
            $(this).hide();
            $.ajax({
                url: '/Admin/UpdateAdministrationInfoAdminProfile',
                type: 'POST',
                data: model,
                success: function (response) {
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
            alterphonenumber: alterphonenumber,
        }
        $('#confirmemail-validationspan').html("");
        $.ajax({
            url: '/Admin/UpdateMailingInfoAdminProfile',
            type: 'POST',
            data: model,
            success: function (response) {
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
}