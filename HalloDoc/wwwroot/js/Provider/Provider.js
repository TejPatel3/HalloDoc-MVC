var regextext = /^[a-zA-Z][a-zA-Z-, ]+$/i;

window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-provider-tab').addClass('admin-layout-nav-active');
}
$('.Contactbtn-Provider').on('click', function () {
    let physicianid = $(this).val();
    console.log(physicianid)
    $('#contactprovidersubmitbtn').val(physicianid)
});
$('#contactprovidersubmitbtn').on('click', function () {
    let physicianid = $(this).val();
    let message = $('#message-contactprovidermodel').val();
    let radiobutton = $('input[name="contactMethod"]:checked').val();
    console.log(radiobutton)
    $('#message-providercontactmodal-span').html("");
    if (radiobutton == undefined) {
        alert("please select one of them");
    }
    else if (message == "") {
        $('#message-providercontactmodal-span').html("please enter message");
    }
    else if (!regextext.test(message)) {

        $('#message-providercontactmodal-span').html("message is not valid");
    }
    else {

        $.ajax({
            url: '/Provider/ContactProviderModelSubmit',
            data: { "physicianid": physicianid, "message": message },
            success: function (response) {
                $(ContactProviderModal).modal('hide');
            },
            error: function (xhr, status, error) {
                console.error(error)
            }
        });
    }
});

$('.editbtn-Provider').on('click', function () {
    let physicianid = $(this).val();


    $.ajax({
        url: '/Provider/EditProviderAccount',
        data: { physicianid: physicianid },
        success: function (response) {
            $('#provider-maindiv').html(response);
            $('#physicianid-editprovideraccount').val(physicianid);
        },
        error: function (xhr, status, error) {
            console.error(error)
        }
    });

});
