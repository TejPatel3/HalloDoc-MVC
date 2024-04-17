$(document).ready(function () {
    $('.viewCase').on('click', function (e) {
        var requestid = $(this).attr('value');
        $.ajax({
            url: '/Admin/ViewCase',
            type: 'GET',
            data: { id: requestid },
            success: function (response) {
                $('#nav-home').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $('.viewNotes').on('click', function (e) {
        var requestid = $(this).attr('value');
        $.ajax({
            url: '/Admin/ViewNotes',
            type: 'GET',
            data: { reqid: requestid },
            success: function (response) {
                $('#nav-home').html(response);
                $('#patientRecordMainDiv').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $('.cancelbtn').on('click', function (e) {
        var name = $(this).attr('value');
        console.log(name)
        $('.pname').html(name);
        var reqid = $(this).attr('id');
        var inputval = $('.requestid');
        var reqtype = $('.requesttype');
        inputval.val(reqid);
        var requesttypeid = $(this).attr('value');;
        reqtype.val(requesttypeid);
        console.log(requesttypeid)
    });

    $('.SendAgreementModal').on('click', function (e) {
        var requestid = $(this).attr('id');
        console.log(requestid);
        $.ajax({
            url: '/Admin/GetDataForAgreemenModal',
            type: 'GET',
            data: { requestid: requestid },
            success: function (response) {
                $('#AgreementModal-Phone').val(response.phonenumber);
                $('#AgreementModal-Email').val(response.email);

                var requesttype = response.requesttype;
                console.log(requesttype);

                var color = "green-dot";
                var text = "";
                switch (requesttype) {
                    case 1:
                        {
                            color = "green-dot";
                            text = "Patient";
                            break;
                        }
                    case 2:
                        {
                            color = "orange-dot";
                            text = "Family/Friend";
                            break;
                        }
                    case 3:
                        {
                            color = "pink-dot";
                            text = "Business Partner";
                            break;
                        }
                    case 4:
                        {
                            color = "cyan-dot";
                            text = "Concierge";
                            break;
                        }
                    default:
                        {
                            color = "purple-dot";
                            text = "vip";
                            break;
                        }
                }
                $('#requesttype-agreement').html(text);
                $('#requesttype-dot').addClass(color);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

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

    $('.dataTables_filter').hide();
    var table = $('.example').DataTable();
    $('input[type="search"]').on('keyup', function () {
        table.search(this.value).draw();
    });
    $('input[name="requestby"]').on('change', function () {
        var value = $(this).attr('id');
        console.log(value);
        if (value == 'requestbyAll') {
            table.columns(0).search('').draw();
        }
        else {
            table.columns(0).search(value).draw(); // Replace 0 with the index of the column you want to filter

            try {
                console.log('accp')
                var headers = document.querySelectorAll('.accordion-header');

                headers.forEach((header) => {
                    const requesttype = header.querySelector('.requesttype-accordion');
                    const nameText = requesttype.textContent || requesttype.innerText;


                    if (nameText.includes(value)) {
                        header.style.display = ''; // Show the header
                    } else {
                        header.style.display = 'none'; // Hide the header
                    }
                });
            }
            catch {
                console.log("catch fro filter")
            }
        }


    });

    $('#RegionSearch').on('change', function () {
        var regionid = $(this).val();
        console.log("hsabhdbshb")
        if (regionid == '1234') {
            table.columns(1).search('').draw();
        }
        else {
            table.columns(1).search(regionid).draw(); // Replace 0 with the index of the column you want to filter
            try {

                var headers = document.querySelectorAll('.accordion-header');

                headers.forEach((header) => {
                    const regionName = header.querySelector('.region-accordion');
                    const nameText = regionName.textContent || regionName.innerText;

                    if (nameText.includes(regionid)) {
                        header.style.display = ''; // Show the header
                    } else {
                        header.style.display = 'none'; // Hide the header
                    }
                });
            }
            catch {

            }
        }
    })

});

$('.viewUpload').on('click', function (e) {
    var requestid = $(this).attr('value');
    console.log(requestid)
    $.ajax({
        url: '/Admin/ViewUpload',
        type: 'GET',
        data: { requestid: requestid },
        success: function (response) {
            $('#nav-home').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('.order').on('click', function (e) {
    var requestid = $(this).attr('value');
    console.log(requestid)
    $.ajax({
        url: '/Admin/Order',
        type: 'GET',
        data: { requestid: requestid },
        success: function (response) {
            $('#nav-home').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('.sendAgreementBtn').on('click', function (e) {

    $.ajax({
        url: '/Admin/SendAgreementModals',
        type: 'POST',
        data: { reqid: requestid },
        success: function (response) {

        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});


$('.closecasebtn').on('click', function (e) {
    var requestid = $(this).attr('id');
    console.log(requestid)
    $.ajax({
        url: '/Admin/CloseCase',
        type: 'GET',
        data: { requestid: requestid },
        success: function (response) {
            $('#nav-home').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});


$('.acceptRequest').on('click', function () {
    console.log("accept")
    var requestid = $(this).val();
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Accept it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ProviderSide/AcceptRequest',
                data: { requestid: requestid },
                success: function (response) {
                    $('#adminLayoutMainDiv').html(response)
                    Swal.fire({
                        title: "Accepted!",
                        text: "Request Accepted Successfully.",
                        icon: "success"
                    });
                }
            })
        }
    });
});

