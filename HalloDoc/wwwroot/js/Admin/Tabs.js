﻿var currentpage = 1;
var status = 1;
var requesttype = 0;
var searchkey;
var regionid = 0;
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
        url: '/Admin/DashboardTabsData',
        data: { "status": status, "currentpage": currentpage },
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
                status = 1;

                break;
            case '#status_pending':
                $('#statusnamehead').html('Pending');

                status = 2;
                break;
            case '#status_active':
                $('#statusnamehead').html('Active');
                status = 4
                break;
            case '#status_conclude':
                $('#statusnamehead').html('Conclude');
                status = 6
                break;
            case '#status_toclose':
                $('#statusnamehead').html('To Close');
                status = 3
                break;
            case '#status_unpaid':
                $('#statusnamehead').html('Unpaide');
                status = 9
                break;

            default:
        }
        console.log(url);
        $.ajax({
            url: '/Admin/DashboardTabsData',
            data: { "status": status, "currentpage": currentpage },

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
        document.body.appendChild(link);
        Adminlink.click();
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

function RequestTypeFilter(type) {
    requesttype = type;

    $.ajax({
        url: '/Admin/DashboardTabsData',
        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
        success: function (response) {
            $('#statusnamehead').html('New');
            $('#datatable').html(response);
            $(this).css("border", "1px solid red")
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
};
function search() {
    searchkey = $('#my-search-input').val();
    $.ajax({
        url: '/Admin/DashboardTabsData',
        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
        success: function (response) {
            $('#statusnamehead').html('New');
            $('#datatable').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
};

function SearchByRegion() {
    var regionid = $('#RegionSearch').val();
    $.ajax({
        url: '/Admin/DashboardTabsData',
        data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
        success: function (response) {
            $('#statusnamehead').html('New');
            $('#datatable').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
};


//  <button id="downloadButton" class="btn btn-primary">Download Excel</button>

//<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
$('a.download').on('click', function () {
    //console.log("kykykyk")
    //$.ajax({
    //    url: '/Admin/DownloadExcelfile', // Replace with your URL
    //    type: 'GET', // Or 'POST', depending on your needs
    //    data: { "status": status, "currentpage": currentpage, "requesttype": requesttype, "searchkey": searchkey, "regionid": regionid },
    //    success: function (response) {
    //        // This function will be executed if the request succeeds
    //        // You can use the 'response' variable to access the data returned from the server

    //        // Create a link element
    //        var link = document.createElement('a');
    //        link.href = window.URL.createObjectURL(new Blob([response]));
    //        link.download = 'Data.xlsx';

    //        // Trigger the download by simulating a click on the link
    //        link.click();
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        // This function will be executed if the request fails
    //        // 'jqXHR' is the raw request object
    //        // 'textStatus' is a string describing the type of error
    //        // 'errorThrown' is an optional exception object, if one occurred
    //    }
    //});

    var link = document.createElement('a');
    link.href = '/Admin/DownloadExcelfile?status=' + status + "&currentpage=" + currentpage + "&requestType=" + requesttype + "&searchkey"+ searchkey+ "&regionid"+ regionid;
    link.style.display = 'none';
    document.body.appendChild(link)
        ;
    link.click();
    document.body.removeChild(link)
        ;
});

