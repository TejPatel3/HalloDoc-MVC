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
            table.columns(0).search(value).draw();
            try {
                console.log('accp')
                var headers = document.querySelectorAll('.accordion-header');
                headers.forEach((header) => {
                    const requesttype = header.querySelector('.requesttype-accordion');
                    const nameText = requesttype.textContent || requesttype.innerText;
                    if (nameText.includes(value)) {
                        header.style.display = '';
                    } else {
                        header.style.display = 'none';
                    }
                });
            }
            catch {

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


//encounter/////////////////////////////////////////////////


$('.encounter').on('click', function (e) {
    console.log('encounter');
    var requestid = $(this).val();
    $('.requestid').val(requestid);
    console.log(requestid)
})

$('.encounter-save').on('click', function (e) {
    var requestid = $('.requestid').val();
    var encountervalue = $('input[name="options-outlined"]:checked').attr('value');
    $.ajax({
        url: '/ProviderSide/EncounterSubmit',
        data: { requestid: requestid, encountervalue: encountervalue },
        success: function (data) {
            $('#exampleModalEncounter').click();
            $('#nav-home').html(data);
            if (data == "") {
                location.reload()
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
})
$('.housecallbtnclickp').on('click', function () {
    var requestid = $(this).val();
    $.ajax({
        url: '/ProviderSide/OnHouseOpenEncounter',
        data: { requestid: requestid },
        type: 'POST',
        success: function (data) {
            $('#nav-home').html(data);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
})

//$('.encounterclick_concludep_Encounter').on('click', function () {
//    var requestid = $(this).val();
//    console.log("mnhjdsbchjsdbjhb")
//    $.ajax({
//        url: '/ProviderSide/Encounter',
//        data: { requestid: requestid },
//        success: function (data) {
//            if (data == "") {
//                alert("Already Finalized");
//            } else {
//                $('#nav-home').html(data);
//            }
//        },
//        error: function (xhr, status, error) {
//            console.log(error);
//        }
//    })
//})

$('.encounterclick_concludep').on('click', function () {
    var requestid = $(this).val();
    console.log("download encounter model open js")
    $.ajax({
        url: '/ProviderSide/CheckFinalize',
        data: { requestid: requestid },
        success: function (result) {
            console.log(result)
            if (result) {
                $.ajax({
                    url: '/ProviderSide/Encounter',
                    data: { requestid: requestid },
                    success: function (result) {
                        $('#download_Modal').html(result);
                        var my = new bootstrap.Modal(document.getElementById('DownloadEncounterModal'));
                        my.show();
                    },
                    error: function (xhr, status, error) {
                        console.log(error);
                    }
                })
            }
            else {
                $.ajax({
                    url: '/ProviderSide/Encounter',
                    data: { requestid: requestid },
                    success: function (result) {
                        $('#nav-home').html(result);                   
                    },
                    error: function (xhr, status, error) {
                        console.log(error);
                    }
                })
            }
            console.log(result.IsAjax);
            $('#download_Modal').html(result);
            var my = new bootstrap.Modal(document.getElementById('DownloadEncounterModal'));
            my.show();
        },
        error: function (xhr, status, error) {
            console.log(error);
        }

    })
})
//$('.encounterclick_concludep').on('click', function () {
//    var requestid = $(this).val();
//    console.log("download encounter model open js")
//    $.ajax({
//        url: '/ProviderSide/Encounter',
//        data: { requestid: requestid },
//        success: function (result) {
//            console.log(result.IsAjax);
//            $('#download_Modal').html(result);
//            var my = new bootstrap.Modal(document.getElementById('DownloadEncounterModal'));
//            my.show();
//        },
//        error: function (xhr, status, error) {
//            console.log(error);
//        }

//    })
//})
$('.data-agreement').on('click', function (e) {
    var requestid = $(this).attr('value');
    console.log(requestid);
    $.ajax({
        url: '/Admin/GetAgreementData',
        type: 'GET',
        data: { requestid: requestid },
        success: function (data) {
            $('#agreement-phone').val(data.phonenumber);
            $('#agreement-email').val(data.email);
            var requesttype = data.requesttype;
            console.log(requesttype);
            console.log(data.requesttype);
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
        }
    })
})