//Edit button Edit Model Open
$('.editbtn').click(function () {
    var id = $(this).val();
    $.ajax({
        url: 'Home/EditEmployee',
        data: { id: id },
        success: function (data) {
            $('#Model').html(data);
            $("#editEmployee").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});

// delete employee from database
$('.deletebtn').click(function () {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            console.log("Add Employee");
            var id = $(this).val();
            $.ajax({
                url: 'Home/DeleteEmployee',
                data: { id: id },
                success: function (data) {
                    $('#data_Table').html(data);
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                },
                error: function (xhr, status, error) {
                    console.log(error);
                }
            });
        }
    });
});


//Pagination in table

$('.example').DataTable({
    "lengthMenu": [[5, 10, -1], [5, 10, "All"]],
    "pageLength": 5,
    language: {
        oPaginate: {
            sNext: '<i class="bi bi-caret-right-fill text-info"></i>',
            sPrevious: '<i class="bi bi-caret-left-fill text-info"></i>'
        }
    }
});

var table = $('.example').DataTable();
$('.dataTables_filter').hide();

$('input[type="search"]').on('keyup', function () {
    table.search(this.value).draw();
});
