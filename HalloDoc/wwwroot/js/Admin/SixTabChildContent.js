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
    data: {reqid: requestid },
    success: function (response) {
        $('#nav-home').html(response);
                },
    error: function (xhr, status, error) {
        console.error(error);
                }
            });
        });

    $('.canclebtn').on('click', function (e) {

            var name = $(this).attr('value');
    console.log(name)
    $('.pname').html(name);
    var reqid = $(this).attr('id');
    var inputval = $('.requestid');
    inputval.val(reqid);
        });

    $('.example').DataTable({
        "initComplete": function (settings, json) {

        $('#my-search-input').val(settings.oPreviousSearch.sSearch);

    $('#my-search-input').on('keyup', function () {
                    var searchValue = $(this).val();
    settings.oPreviousSearch.sSearch = searchValue;
    settings.oApi._fnReDraw(settings);
                });
            },
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

    $('input[name="requestby"]').on('change', function () {
            var value = $(this).attr('id');
    console.log(value);
    if (value == 'requestbyAll') {
        table.columns(0).search('').draw();
            }
    else {
        table.columns(0).search(value).draw(); // Replace 0 with the index of the column you want to filter
            }
        });

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