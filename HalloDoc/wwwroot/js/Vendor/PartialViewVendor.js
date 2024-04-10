

$('.editvendor').on('click', function () {
    console.log("djshvdhvsahv")
    var vendorid = $(this).val();
    $('#vendorIdEditVendorInput').val(vendorid);
    $.ajax({
        url: '/Vendor/EditVendorAccount',
        data: { vendorid: vendorid },

        success: function (response) {
            $('#vendormaindiv').html(response)
        }
    })
});

$('.deletevendor').on('click', function () {
    var vendorid = $(this).val();

    $.ajax({
        url: '/Vendor/VendorFilter',
        data: { vendorid: vendorid },

        success: function (response) {
            loadview(search, professionid);
            $('#vendortablediv').html(response)
            toastr.success("Account Deleted Successfully")
        }
    })
});

