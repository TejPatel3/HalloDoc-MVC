var firstname;
var lastname;
var email;
var phone;
function loadData() {
    console.log("load data")
    $.ajax({
        url: '/Records/GetPatientGistoryTable',
        data: { 'firstname': firstname, 'lastname': lastname, 'email': email, 'phone': phone },
        success: function (data) {
            $('#patientHistoryTable').html(data)
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}
loadData();
$('#searchPatientHistorybtn').click(function () {
    firstname = $('#firstNamePatientHistory').val();
    lastname = $('#lastNamePatientHistory').val();
    email = $('#emailPatientHistory').val();
    phone = $('#phonePatientHistory').val();
    if (firstname.trim() == "" && lastname.trim() == "" && email.trim() == "" && phone.trim() == "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Please enter valid data",
        });
        return
    }
    loadData();
});