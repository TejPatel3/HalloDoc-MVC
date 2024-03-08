
    $('.savenote').click(function () {

        var reqid = $('#rid').attr('value');
    var notes = $('#notes').val();
    var viewModel = {
        requestid: reqid,
    BlockNotes: notes
        };
    console.log(viewModel)
    console.log(viewModel)
    console.log(viewModel)
    $.ajax({
        url: '/Admin/ViewNotes', // replace with your URL
    type: 'POST',
    dataType: 'json',
    // contentType: 'application/json; charset=utf-8',
    data: viewModel,
    success: function (data) {
        // Handle success
        console.log('Success:', data);
            },
    error: function () {
        // Handle error
        console.log('Error occurred');
            }
        });

    $('#adminnoteupdate').html(notes);
    });



                // $(document).ready(function () {

                //     $('.savenote').on('click', function (e) {
                //         console.log("hello")

                //         var reqid = $('#rid').attr('value');
                //         var notes = $('#notes').val();
                //         console.log(reqid);
                //         console.log(notes);
                //         // console.log(rid);
                //         var model = {
                //             requestid : reqid,
                //             BlockNotes : notes
                //         }
                //         $.ajax({
                //             url: '/Admin/ViewNotes', // replace with your URL
                //             type: 'POST',
                //             dataType: 'json',
                //             contentType: 'application/json; charset=utf-8',
                //             data: JSON.stringify(model),
                //             success: function (data) {
                //                 // Handle success
                //                 console.log('Success:', data);
                //             },
                //             error: function () {
                //                 // Handle error
                //                 console.log('Error occurred');
                //             }
                //         });
                //         // $(this).closest('form')
                //     });
                // });
