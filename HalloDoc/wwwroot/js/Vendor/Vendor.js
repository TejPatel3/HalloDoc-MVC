window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-partner-tab').addClass('admin-layout-nav-active');
}
var search = ""; 
var professionid = 0; 
var vendorid = 0; 
$(document).ready(function () {
    loadview("", 0);
});
    function loadview(search, professionid) {
        $.ajax({
            url: '/Vendor/VendorFilter',
            data: { search: search, professionid: professionid },
            success: function (response) {
                $('#vendortablediv').html(response)
            }
        });
    }


$('#professionnamefilter').on('change', function () {
    professionid = $(this).val();
    $.ajax({
        url: '/Vendor/VendorFilter',
        data: { professionid: professionid },
        success: function (response) {
            loadview(search, professionid);
            $('#vendortablediv').html(response)
        }
    })
});

$('#searchfiltervendor').on('input', function () {
    search = $(this).val();
    $.ajax({
        url: '/Vendor/VendorFilter',
        data: { search: search },
        success: function (response) {
            loadview(search, professionid);
            $('#vendortablediv').html(response)
        }
    })
});

