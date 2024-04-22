window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-home-tab').addClass('admin-layout-nav-active');
}
var regexnote = /^[a-zA-Z0-9][a-zA-Z0-9 ]+$/i;
var regextext = /^[a-zA-Z][a-zA-Z ]+$/i;
var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
var regexphone = /^[1-9][0-9]{9}$/;
$('#CancelModalSubminbtn').click(function () {
    var note = $('#CancelModalNote').val();
    var firsrdrop = $('#CancelModalFirstDropDownSelect').val();
    var allvalidation = true;
    $('#CancelModalFirstDropDownSpan').html("");
    $('#CancelModalNoteSpan').html("");
    if (FirstDropDownValidation(firsrdrop)) {
        $('#CancelModalFirstDropDownSpan').html("Please Select Reason for cancellation");
        allvalidation = false;
    }
    if (note == "") {
        $('#CancelModalNoteSpan').html("Please Enter cancellation Note");
        allvalidation = false;
    }
    else if (!regexnote.test(note)) {
        $('#CancelModalNoteSpan').html("cancellation Note is not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#CancelModalSubminbtn').closest('form').submit()
    }
});

$('#AssignModalSubminbtn').click(function () {
    var note = $('#AssignModalNote').val();
    var Region = $('#AssignModalFirstDropDownSelect').val();
    var Physician = $('#AssignModalSecondDropDownSelect').val();
    var allvalidation = true;
    $('#AssignModalFirstDropDownSpan').html("");
    $('#AssignModalNoteSpan').html("");
    $('#AssignModalSecondDropDownSpan').html("");
    if (FirstDropDownValidation(Region)) {
        $('#AssignModalFirstDropDownSpan').html("Please Select Region");
        allvalidation = false;
    }
    if (SecondDropDownValidation(Physician)) {
        if (FirstDropDownValidation(Region)) {
            $('#AssignModalSecondDropDownSpan').html("Please Select Region first");
        }
        else {
            $('#AssignModalSecondDropDownSpan').html("Please Select Physician");
        }
        allvalidation = false;
    }
    if (note == "") {
        $('#AssignModalNoteSpan').html("Please Enter Assign Note");
        allvalidation = false;
    }
    else if (!regexnote.test(note)) {
        $('#AssignModalNoteSpan').html("Assign Note is not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#AssignModalSubminbtn').closest('form').submit()
    }
});


$('#BlockModalSubminbtn').click(function () {
    var note = $('#BlockModalNote').val();
    let allvalidation = true;
    $('#BlockModalNoteSpan').html(" ");
    $('#BlockModalNoteSpan').html("");
    if (note == "") {
        $('#BlockModalNoteSpan').html("Please Enter Block Note");
        allvalidation = false;
    }
    else if (!regexnote.test(note)) {
        $('#BlockModalNoteSpan').html("Block Note is not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#BlockModalSubminbtn').closest('form').submit()
    }
});

$('#TransferModalSubminbtn').click(function () {
    console.log("model tranfer")
    var note = $('#TransferModalNote').val();
    var Region = $('#TransferModalFirstDropDownSelect').val();
    var Physician = $('#TransferModalSecondDropDownSelect').val();
    var allvalidation = true;
    $('#TransferModalFirstDropDownSpan').html("");
    $('#TransferModalNoteSpan').html("");
    $('#TransferModalSecondDropDownSpan').html("");
    if (FirstDropDownValidation(Region)) {
        $('#TransferModalFirstDropDownSpan').html("Please Select Region");
        allvalidation = false;
    }
    if (SecondDropDownValidation(Physician)) {
        if (FirstDropDownValidation(Region)) {
            $('#TransferModalSecondDropDownSpan').html("Please Select Region First");
        }
        else {
            $('#TransferModalSecondDropDownSpan').html("Please Select Physician");

        }
        allvalidation = false;
    }
    if (note == "") {
        $('#TransferModalNoteSpan').html("Please Enter Assign Note");
        allvalidation = false;
    }
    else if (!regexnote.test(note)) {
        $('#TransferModalNoteSpan').html("Assign Note is not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#TransferModalSubminbtn').closest('form').submit();
    }
});

$('#SendAgreementModalSubminbtn').click(function () {
    var phone = $('#AgreementModal-Phone').val();
    var email = $('#AgreementModal-Email').val();
    let allvalidation = true;
    $('#SendAgreementModalphoneSpan').html("");
    $('#SendAgreementModalemailSpan').html("");
    if (phone == "") {
        $('#SendAgreementModalphoneSpan').html("Please Enter Phone number");
        allvalidation = false;
    }
    else if (!regexphone.test(phone)) {
        $('#SendAgreementModalphoneSpan').html("Please enter 10 digit mobile number");
        allvalidation = false;
    }
    if (email == "") {
        $('#SendAgreementModalemailSpan').html("Please Enter Email");
        allvalidation = false;
    }
    else if (!regexemail.test(email)) {
        $('#SendAgreementModalemailSpan').html("Email is not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#SendAgreementModalSubminbtn').closest('form').submit()
    }
});

$('#SendCreateRequestModalSubminbtn').click(function () {
    var name = $('#SendCreateRequestModalName').val();
    var phonenumber = $('#SendCreateRequestModalPhoneNumber').val();
    let email = $('#SendCreateRequestModalEmail').val();
    var allvalidation = true;
    $('#SendCreateRequestModalNameSpan').html("");
    $('#SendCreateRequestModalPhoneNumberSpan').html("");
    $('#SendCreateRequestModalEmailSpan').html("");
    if (name == "") {
        $('#SendCreateRequestModalNameSpan').html("Please Enter Name");
        allvalidation = false;
    }
    else if (!regextext.test(name)) {
        $('#SendCreateRequestModalNameSpan').html("Name is not valid");
        allvalidation = false;
    }
    if (phonenumber == "") {
        $('#SendCreateRequestModalPhoneNumberSpan').html("please enter phonenumber");
        allvalidation = false;
    }
    else if (!regexphone.test(phonenumber)) {
        $('#SendCreateRequestModalPhoneNumberSpan').html("please enter 10 digit phone number");
        allvalidation = false;
    }
    if (email == "") {
        $('#SendCreateRequestModalEmailSpan').html("please enter email");
        allvalidation = false;
    }
    else if (!regexemail.test(email)) {
        $('#SendCreateRequestModalEmailSpan').html("please enter valid email");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#SendCreateRequestModalSubminbtn').closest('form').submit()
    }
});


function FirstDropDownValidation(firstdropvalue) {
    if (firstdropvalue == "") {
        return true;
    }
    return false;
};
function SecondDropDownValidation(seconddropvalue) {
    console.log(seconddropvalue)
    if (seconddropvalue == "") {
        return true;
    }
    return false;
};

function ModalNotesValidation(note) {
    const regexPattern = /^[a-zA-Z0-9 ]+$/;
    if (note == "") {
        return true;
    }
    else if (!regexPattern.test(note)) {
        return true;
    }
    return false;
};


