var regexstrongpassword = /[A-Za-z\d@$!%*?&]{8,}/
var regextext = /^[a-zA-Z][a-zA-Z ]+$/i;
var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;

var regexphone = /^[1-9][0-9]{9}$/;
$('#sendcodeforgotpassword').on('click', function () {
    var email = $('#email-forgotpassword').val();
    var model = { sendcode: true, checkcode: false, updatepassword: false, email: email };
    if (email == "") {
        $('#email-forgotpassword-span').html("please enter email")
    }
    else if (!regexemail.test(email)) {
        $('#email-forgotpassword-span').html("email is not valid")
    }
    else {
        $.ajax({
            url: '/AdminCredential/AdminForgotPassword',
            type: 'POST',
            data: model,
            success: function (response) {
                $('#generatedconfirmationcode').val(response.confirmationCode); // replace 'your-input-field' with the id of your input field

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});
$('#checkcodeforgotpassword').on('click', function () {
    let confirmationcode = $('#confirmationcode-forgotpassword').val();
    let originalconfirmationcode = $('#generatedconfirmationcode').val();
    var model = { sendcode: false, checkcode: true, updatepassword: false, email: null, confirmationcode: confirmationcode, originalconfirmationcode: originalconfirmationcode };
    $('#confirmationcode-forgotpassword-span').html("")
    if (confirmationcode == "") {
        $('#confirmationcode-forgotpassword-span').html("please enter confirmation code that sent in mail")
    }
    else {

        $.ajax({
            url: '/AdminCredential/AdminForgotPassword',
            type: 'POST',
            data: model,
            success: function (data) {
                console.log(data.confirmationnumbernotmatch)
                if (!data.confirmationnumbernotmatch) {
                    $('#email-forgotpassword-div').addClass('d-none');
                    $('#confirmationcode-forgotpassword-div').addClass('d-none');
                    $('#sendcodeforgotpassword').addClass('d-none');
                    $('#checkcodeforgotpassword').addClass('d-none');
                    $('#password-forgotpassword-div').removeClass('d-none');
                    $('#confirmpassword-forgotpassword-div').removeClass('d-none');
                    $('#updatepasswordforgotpassword').removeClass('d-none');
                }
                else {
                    $('#confirmationcode-forgotpassword-span').html("Confirmation code is incorrect")
                }

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});
$('#updatepasswordforgotpassword').on('click', function () {
    var email = $('#email-forgotpassword').val();
    var allvalidation = true;
    let password = $('#password-forgotpassword').val();
    let confirmpassword = $('#confirmpassword-forgotpassword').val();
    var model = { sendcode: false, checkcode: false, updatepassword: true, email: email, confirmationcode: null, password: password };
    $('#password-forgotpassword-span').html("")
    $('#confirmpassword-forgotpassword-span').html("")
    if (password == "") {
        $('#password-forgotpassword-span').html("please enter password")
        allvalidation = false;
    }
    else if (!regexstrongpassword.test(password)) {
        alert("Please Enter Strong Password that contain\nAt least one uppercase letter\nAt least one lowercase letter\nAt least one digit\nAt least one special character\nAt least 8 characters in length")
        allvalidation = false;
    }
    else if (password == "") {
        $('#confirmpassword-forgotpassword-span').html("please enter confirm password")
        allvalidation = false;
    }
    else if (password != confirmpassword) {
        $('#confirmpassword-forgotpassword-span').html("password and confirm password not match")
        allvalidation = false;
    }
    if (allvalidation) {
        $.ajax({
            url: '/AdminCredential/AdminForgotPassword',
            type: 'POST',
            data: model,
            success: function (response) {
                window.location.href = "https://localhost:44370/AdminCredential/AdminLogin";
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});