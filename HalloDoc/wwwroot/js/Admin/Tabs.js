window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-home-tab').addClass('admin-layout-nav-active');
}

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
                    value: "",
                    text: "Select Physician"
                }))
                console.log(data)
                $.each(data, function (index, item) {
                    secondDropdown.append($('<option>', {
                        value: item.physicianId, // Replace with the actual value from your data
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
                url = "/Admin/New";
                status = 1;

                break;
            case '#status_pending':
                $('#statusnamehead').html('Pending');
                statusnamehead = "Pending";
                url = "/Admin/Pending";

                status = 2;
                break;
            case '#status_active':
                $('#statusnamehead').html('Active');
                url = "/Admin/Active";
                statusnamehead = "Active";

                status = 4
                break;
            case '#status_conclude':
                $('#statusnamehead').html('Conclude');
                statusnamehead = "Conclude";
                url = "/Admin/Conclude";

                status = 6
                break;
            case '#status_toclose':
                $('#statusnamehead').html('To Close');
                statusnamehead = "To Close";

                url = "/Admin/ToClose";
                status = 3
                break;
            case '#status_unpaid':
                $('#statusnamehead').html('Unpaide');
                statusnamehead = "Unpaid";
                url = "/Admin/Unpaid";

                status = 9
                break;

            default:
                url = "/Admin/New";
        }
        console.log(url)
        $.ajax({
            url: url,

            success: function (response) {
                $('#datatable').html(response);

            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    console.log("error accured with 401");
                    location.reload();
                }
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
        document.body.appendChild(link);
        Adminlink.click();
        document.body.removeChild(link);
    });
}

function filterAccordionSearch() {
    console.log("serach run")
    const input = document.getElementById('my-search-input');
    const filter = input.value.toUpperCase();
    const headers = document.querySelectorAll('.accordion-header');


    headers.forEach((header) => {
        const patientName = header.querySelector('.patient-name');
        const nameText = patientName.textContent || patientName.innerText;

        if (nameText.toUpperCase().includes(filter)) {
            header.style.display = ''; // Show the header
        } else {
            header.style.display = 'none'; // Hide the header
        }
    });
}

// Attach the filter function to the input field
document.getElementById('my-search-input').addEventListener('keyup', filterAccordionSearch);



//var all = document.getElementById('downloadall');
//console.log(all);
//all.addEventListener('click', function () {
//    console.log("run");
//});


//$('#downloadall').click(function () {

//});
//$(".radio-under").click(function () {
//    let requesttype = $(this).attr('id');
//    if (requesttype == "requestbyAll") {
//        requesttype = 0;
//    }
//    $.ajax({
//        url: '/Admin/DashboardTabsData',
//        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
//        success: function (response) {
//            $('#statusnamehead').html(statusnamehead);
//            $('#datatable').html(response);
//            $(this).css("border", "1px solid red")
//        },
//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//});

//function search() {
//    searchkey = $('#my-search-input').val();
//    $.ajax({
//        url: '/Admin/DashboardTabsData',
//        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
//        success: function (response) {
//            $('#statusnamehead').html(statusnamehead);
//            $('#datatable').html(response);
//        },
//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//};

//function SearchByRegion() {
//    var regionid = $('#RegionSearch').val();
//    console.log("kolok")
//    $.ajax({
//        url: '/Admin/DashboardTabsData',
//        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
//        success: function (response) {
//            $('#statusnamehead').html(statusnamehead);
//            $('#datatable').html(response);
//        },
//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//};


//  <button id="downloadButton" class="btn btn-primary">Download Excel</button>

//<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
//$('a.download').on('click', function () {
//    //console.log("kykykyk")
//    //$.ajax({
//    //    url: '/Admin/DownloadExcelfile', // Replace with your URL
//    //    type: 'GET', // Or 'POST', depending on your needs
//    //    data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
//    //    success: function (response) {
//    //        // This function will be executed if the request succeeds
//    //        // You can use the 'response' variable to access the data returned from the server

//    //        // Create a link element
//    //        var link = document.createElement('a');
//    //        link.href = window.URL.createObjectURL(new Blob([response]));
//    //        link.download = 'Data.xlsx';

//    //        // Trigger the download by simulating a click on the link
//    //        link.click();
//    //    },
//    //    error: function (jqXHR, textStatus, errorThrown) {
//    //        // This function will be executed if the request fails
//    //        // 'jqXHR' is the raw request object
//    //        // 'textStatus' is a string describing the type of error
//    //        // 'errorThrown' is an optional exception object, if one occurred
//    //    }
//    //});

//    var link = document.createElement('a');
//    link.href = '/Admin/DownloadExcelfile?status=' + status + "&currentpage=" + currentpage + "&requestType=" + requesttype + "&searchkey" + searchkey + "&regionid" + regionid;
//    link.style.display = 'none';
//    document.body.appendChild(link)
//        ;
//    link.click();


//    document.body.removeChild(link)
//        ;
//});

