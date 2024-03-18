$('#CancelModalSubminbtn').click(function () {
    console.log("assign modal validation js")
    var note = $('#CancelModalNote').val();
    var firsrdrop = $('#CancelModalFirstDropDownSelect').val();


    if (FirstDropDownValidation(firsrdrop)) {
        $('#CancelModalFirstDropDownSpan').html("Please Select Reason for cancellation");
        $('#CancelModalNoteSpan').html("");
    }
    else {
        $('#CancelModalFirstDropDownSpan').html("");
        const regexPattern = /^[a-zA-Z0-9 ]+$/;

        if (note == "") {
            $('#CancelModalNoteSpan').html("Please Enter cancellation Note");
        }
        else if (!regexPattern.test(note)) {
            $('#CancelModalNoteSpan').html("cancellation Note is not valid");
        }
        else {
            $('#CancelModalSubminbtn').closest('form').submit()

        }
    }
});




$('#AssignModalSubminbtn').click(function () {
    console.log("assign modal validation js")
    var note = $('#AssignModalNote').val();
    var Region = $('#AssignModalFirstDropDownSelect').val();
    var Physician = $('#AssignModalSecondDropDownSelect').val();
    console.log(Physician)
    console.log(note)

    if (FirstDropDownValidation(Region)) {
        $('#AssignModalFirstDropDownSpan').html("Please Select Region");
        $('#AssignModalNoteSpan').html("");
        $('#AssignModalSecondDropDownSpan').html("");
    }
    else {
        $('#AssignModalFirstDropDownSpan').html("");
        if (SecondDropDownValidation(Physician)) {
            $('#AssignModalSecondDropDownSpan').html("Please Select Physician");
        }
        else {
            $('#AssignModalSecondDropDownSpan').html("");
            const regexPattern = /^[a-zA-Z0-9 ]+$/;

            if (note == "") {
                $('#AssignModalNoteSpan').html("Please Enter Assign Note");
            }
            else if (!regexPattern.test(note)) {
                $('#AssignModalNoteSpan').html("Assign Note is not valid");
            }
            else {
                $('#AssignModalSubminbtn').closest('form').submit()
            }
        }
    }
});


$('#BlockModalSubminbtn').click(function () {
    console.log("assign modal validation js")
    var note = $('#BlockModalNote').val();
    const regexPattern = /^[a-zA-Z0-9 ]+$/;
    if (note == "") {
        $('#BlockModalNoteSpan').html("Please Enter Block Note");
    }
    else if (!regexPattern.test(note)) {
        $('#BlockModalNoteSpan').html("Block Note is not valid");
    }
    else {
        $('#BlockModalSubminbtn').closest('form').submit()
    }
});




$('#TransferModalSubminbtn').click(function () {
    console.log("assign modal validation js")
    var note = $('#TransferModalNote').val();
    var Region = $('#TransferModalFirstDropDownSelect').val();
    var Physician = $('#TransferModalSecondDropDownSelect').val();
    console.log(Physician)
    console.log(note)

    if (FirstDropDownValidation(Region)) {
        $('#TransferModalFirstDropDownSpan').html("Please Select Region");
        $('#TransferModalNoteSpan').html("");
        $('#TransferModalSecondDropDownSpan').html("");
    }
    else {
        $('#TransferModalFirstDropDownSpan').html("");
        if (SecondDropDownValidation(Physician)) {
            $('#TransferModalSecondDropDownSpan').html("Please Select Physician");
        }
        else {
            $('#TransferModalSecondDropDownSpan').html("");
            const regexPattern = /^[a-zA-Z0-9 ]+$/;

            if (note == "") {
                $('#TransferModalNoteSpan').html("Please Enter Assign Note");
            }
            else if (!regexPattern.test(note)) {
                $('#TransferModalNoteSpan').html("Assign Note is not valid");
            }
            else {
                $('#TransferModalSubminbtn').closest('form').submit()
            }
        }
    }
});


function FirstDropDownValidation(firstdropvalue) {
    console.log(firstdropvalue)
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
    console.log(note)
    const regexPattern = /^[a-zA-Z0-9 ]+$/;
    if (note == "") {
        return true;
    }
    else if (!regexPattern.test(note)) {
        return true;
    }
    return false;
};




//$('.ModalSubminbtn').click(function () {
//    console.log("assign modal validation js")
//    var note = $('.notes').val();
//    var firstdrop = $('.ModalFirstDropDownSelect').val();
//    var seconddrop = $('.ModalSecondDropDownSelect').val();
//    let regexPattern = /^[a-z A-Z 0-9]+$/;
//    console.log(firstdrop)
//    console.log(seconddrop)
//    console.log(note)
//    if (firstdrop == "") {
//        $('.ModalFirstDropDownSpan').html("This is Required Please Select");
//    }
//    else {
//        $('.ModalFirstDropDownSpan').html("");
//    }
//    if (seconddrop == "") {
//        $('.ModalSecondDropDownSpan').html("This is Required Please Select");
//    }
//    else {
//        $('.ModalSecondDropDownSpan').html("");

//    }
//    if (note == "") {
//        $('.ModalNotesSpan').html("Please Enter Note");
//    }
//    else if (!regexPattern.test(note)) {
//        $('.ModalNotesSpan').html("Note is not Valid");
//    }
//    else {
//        $('.ModalNotesSpan').html("");
//        $('.ModalSubminbtn').closest('form').submit()
//    }
//});

