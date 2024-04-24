///First time Data Load In div
$(document).ready(function () {
    console.log("js run")
    $.ajax({
        url: 'Home/GetTableData',
        type: 'POST',
        success: function (data) {
            $('#data_Table').html(data);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});

//Add Employee model open
$('#AddEmployeebtn').click(function () {
    debugger
    console.log("Add Employee");
    $.ajax({
        url: 'Home/AddEmployeeModel',
        success: function (data) {
            $('#Model').html(data);
            $("#AddEmployee").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});

