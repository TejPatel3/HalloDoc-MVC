var regextext = /^[a-zA-Z][a-zA-Z0-9 ]+$/i;
var regexlicense = /^[a-zA-Z0-9][a-zA-Z0-9 ]+$/i;
var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
var regexphone = /^[1-9][0-9]{9}$/;


$('#username-editprovideraccount').on('input', function () {
    let username = $('#username-editprovideraccount').val();
    $('#username-editprovideraccount-span').html("");
    if (username == "") {
        $('#username-editprovideraccount-span').html("please enter username");
    }
    else if (!regextext.test(username)) {
        $('#username-editprovideraccount-span').html("username not valid");

    }
});
$('#Aieditbtn-editprovideraccount').on('click', function () {
    $('#Aisavebtn-editprovideraccount').removeClass('d-none')
    $('.aidisabled').removeAttr('disabled', false);
    $(this).addClass('d-none')
});
$('#Aisavebtn-editprovideraccount').on('click', function () {
    let username = $('#username-editprovideraccount').val();
    let physicianid = $('#physicianid-editprovideraccount').val();
    console.log(physicianid)
    var model = {
        physicianid: physicianid,
        username: username,
    }

    if ($('#username-editprovideraccount-span').html() == ""){
        $('#Aieditbtn-editprovideraccount').removeClass('d-none')
        $('.aidisabled').attr('disabled', true);
        $(this).addClass('d-none');

        $.ajax({
            url: '/Provider/EditProviderAccount',
            type: 'POST',
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
$('#pieditbtn-editprovideraccount').on('click', function () {

    $('.pidisabled').removeAttr('disabled', false);
    $('#pisavebtn-editprovideraccount').removeClass('d-none');
    $(this).addClass('d-none');
});


$('#firstname-editprovideraccount').on('input', function () {
    let firstname = $('#firstname-editprovideraccount').val();

    $('#firstname-editprovideraccount-span').html("");
    if (firstname == "") {
        $('#firstname-editprovideraccount-span').html("please enter firstname");
        allvalidation = false;
    }
    else if (!regextext.test(firstname)) {
        $('#firstname-editprovideraccount-span').html("firstname not valid");
        allvalidation = false;

    }
});

//$('#lastname-editprovideraccount')
//$('#phone-editprovideraccount')

//$('#medicallicense-editprovideraccount')
//$('#npinumber-editprovideraccount')




$('#pisavebtn-editprovideraccount').on('click', function () {
    let physicianid = $('#physicianid-editprovideraccount').val();

    let firstname = $('#firstname-editprovideraccount').val();
    let lastname = $('#lastname-editprovideraccount').val();
    let email = $('#email-editprovideraccount').val();
    let phonenumber = $('#phone-editprovideraccount').val();
    let medicallicense = $('#medicallicense-editprovideraccount').val();
    let npinum = $('#npinumber-editprovideraccount').val();
    let regionlist = $('#username-editprovideraccount').val();
    let allvalidation = true;
    //var selectedregion = [];

    //$('input[type="checkbox"]:checked').each(function () {
    //    selectedregion.push($(this).val());
    //});
    var model = {
        physicianid: physicianid,
        firstname: firstname,
        lastname: lastname,
        email: email,
        phonenumber: phonenumber,
        medicallicencenumber: medicallicense,
        npinumber: npinum,
    }
    $('#lastname-editprovideraccount-span').html("");
    $('#email-editprovideraccount-span').html("");
    $('#phone-editprovideraccount-span').html("");
    $('#medicallicense-editprovideraccount-span').html("");
    $('#npinumber-editprovideraccount-span').html("");
   
    if (lastname == "") {
        $('#email-editprovideraccount-span').html("please enter lastname");
        allvalidation = false;
    }
    else if (!regextext.test(lastname)) {
        $('#email-editprovideraccount-span').html("lastname not valid");
        allvalidation = false;
    }
    if (email == "") {
        $('#firstname-editprovideraccount-span').html("please enter email");
        allvalidation = false;
    }
    else if (!regexemail.test(email)) {
        $('#firstname-editprovideraccount-span').html("email not valid");
        allvalidation = false;
    }
    if (phonenumber == "") {
        $('#phone-editprovideraccount-span').html("please enter phonenumber");
        allvalidation = false;
    }
    else if (!regexphone.test(phonenumber)) {
        $('#phone-editprovideraccount-span').html("phonenumber not valid");
        allvalidation = false;
    }
    if (medicallicense == "") {
        $('#medicallicense-editprovideraccount-span').html("please enter medical license");
        allvalidation = false;
    }
    else if (!regexlicense.test(medicallicense)) {
        $('#medicallicense-editprovideraccount-span').html("medical license not valid");
        allvalidation = false;
    }
    if (npinum == "") {
        $('#npinumber-editprovideraccount-span').html("please enter NPI number");
        allvalidation = false;
    }
    else if (!regexlicense.test(npinum)) {
        $('#npinumber-editprovideraccount-span').html("NPI number not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#pieditbtn-editprovideraccount').removeClass('d-none')
        $('.pidisabled').attr('disabled', true);
        $(this).addClass('d-none');
        $.ajax({
            url: '/Provider/EditProviderAccount',
            type: 'POST',
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