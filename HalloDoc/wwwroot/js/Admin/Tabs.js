
$(document).ready(function () {
    $('.SelectedRegionGivePhysicianList').change(function () {
        var regionId = $(this).find(":selected").attr('id'); // This will get the id of the selected region

        $.ajax({
            url: '/Admin/GetPhysicianByRegionId', // Replace with your server script URL
            type: 'GET',
            data: { regionId: regionId },
            success: function (data) {
                var secondDropdown = $('.physiciandrop'); // Replace with your second dropdown selector
                secondDropdown.empty(); // Clear existing options
                secondDropdown.append($('<option>', {
                    hidden: "hidden",
                    value: "invalid",
                    text: "Select Physician"
                }))
                $.each(data, function (index, item) {
                    secondDropdown.append($('<option>', {
                        value: item.firstName + item.lastName, // Replace with the actual value from your data
                        text: item.firstName + item.lastName // Replace with the actual text from your data
                    }));
                });
            }
        });
    });



    $.ajax({
        url: '/Admin/New',
        success: function (response) {
            $('#statusnamehead').html('New');
            $('#datatable').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });


    $('.dashboardtab').on('click', function (e) {
        e.preventDefault();
        $('.dashboardtab').removeClass('active');
        $(this).addClass('active');
        var target = $(this).data('bs-target');
        localStorage.setItem("target", target);
        var url;
        switch (target) {
            case '#status_new':
                $('#statusnamehead').html('New');
                url = '/Admin/New';

                break;
            case '#status_pending':
                $('#statusnamehead').html('Pending');

                url = '/Admin/Pending';
                break;
            case '#status_active':
                $('#statusnamehead').html('Active');

                url = '/Admin/Active';
                break;
            case '#status_conclude':
                $('#statusnamehead').html('Conclude');

                url = '/Admin/Conclude';
                break;
            case '#status_toclose':
                $('#statusnamehead').html('To Close');

                url = '/Admin/Toclose';
                break;
            case '#status_unpaid':
                $('#statusnamehead').html('Unpaide');

                url = '/Admin/Unpaid';
                break;

            default:
                url = '../Home/AdminLogin';
        }
        console.log(url);
        $.ajax({
            url: url,

            success: function (response) {
                $('#datatable').html(response);

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

function download() {
    var selectedFiles = document.querySelectorAll('input[name="checkbox"]:checked');
    var fileUrls = [];
    console.log(selectedFiles)

    // Iterate through selected checkboxes and extract file URLs
    selectedFiles.forEach(function (checkbox) {
        var row = checkbox.closest('tr');
        var fileUrl = row.querySelector('a').getAttribute('href');
        fileUrls.push(fileUrl);
    });
    // Download each file
    fileUrls.forEach(function (url) {
        // Create an anchor element to trigger the download
        var link = document.createElement('a');
        link.href = url;
        link.download = '';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });
}


//var all = document.getElementById('downloadall');
//console.log(all);
//all.addEventListener('click', function () {
//    console.log("run");
//});


//$('#downloadall').click(function () {

//});