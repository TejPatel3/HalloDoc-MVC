//const { isEmptyObject } = require("jquery");

var actualBtn = document.getElementById('actual-btn-viewupload');
var fileChosen = document.getElementById('file-chosen');
actualBtn.addEventListener('change', function () {
    var filesname = this.files[0].name;
    console.log(filesname)
    for (var i = 1; i < this.files.length; i++) {
        filesname = filesname + " + " + this.files[i].name;
    }
    fileChosen.style.fontSize = "15px";
    fileChosen.style.fontWeight = "bold"
    fileChosen.textContent = filesname;
    fileChosen.ariaPlaceholder = filesname;
    console.log("run")
})

$('.uploadbtn').on('click', function (e) {
    var formData = new FormData();
    for (var i = 0; i < actualBtn.files.length; i++) {
        formData.append('file', actualBtn.files[i]); // Append each selected file
    }
    formData.append('id', $('.RequestsId').val());
    console.log(formData);
    // Add any other data you need (e.g., RequestsId)
    $.ajax({
        url: '../Admin/UploadButton', // Replace with your controller action URL
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $('#nav-home').html(response);
        },
        error: function (error) {
            console.error('Error uploading files:', error);
        }
    });

});

//$('.uploadbtn').on('click', function (e) {
//    e.preventDefault();
//    var formData = new FormData();
//    for (var i = 0; i < actualBtn.files.length; i++) {
//        formData.append('file', actualBtn.files[i]); // Append each selected file
//    }
//    var reqid = $('.RequestsId').val();

//    formData.append('id', reqid);
//    console.log(formData);
//    // Add any other data you need (e.g., RequestsId)
//    $.ajax({
//        url: '../Admin/UploadButton', // Replace with your controller action URL
//        type: 'POST',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (response) {
//            $('#nav-home').html(response);
//        },
//        error: function (error) {
//            console.error('Error uploading files:', error);
//        }
//    });
//});


$('#downloadall').click(function () {
    $('.childCheckbox').prop('checked', $(this).prop('checked'));
});

// Handle individual checkbox clicks to update the leader checkbox state
$('.childCheckbox').click(function () {
    $('#downloadall').prop('checked', $('.childCheckbox:checked').length === $('.childCheckbox').length);
});

$('#DeleteAll').click(function () {
    console.log("lol")
    $('.childCheckbox:checked').each(function () {
        var wiseFileId = $(this).attr('id');
        var reqid = $('.RequestsId').val();
        console.log(reqid);
        console.log(wiseFileId);

        $.ajax({
            url: '/Admin/DeleteDoc', // Replace with your controller action URL
            type: 'POST',
            data: {
                wiseid: wiseFileId,
                reqId: reqid
            },

            success: function (response) {
                $('#nav-home').html(response);

            },
            error: function (error) {
                console.error('Error uploading files:', error);
            }
        });
    });
});

$('.DeleteDoc').on('click', function () {
    console.log("hello")
    var wiseFileId = $(this).attr('id');
    var reqid = $('.RequestsId').val();
    console.log(wiseFileId)

    console.log(reqid)
    $.ajax({
        url: '/Admin/DeleteDoc', // Replace with your controller action URL
        type: 'POST',
        data: {
            wiseid: wiseFileId,
            reqId: reqid
        },

        success: function (response) {
            $('#nav-home').html(response);

        },
        error: function (error) {
            console.error('Error uploading files:', error);
        }
    });
});

$('#sendMail').on('click', function (e) {
    e.preventDefault();

    var formData = new FormData();

    $('input[name="checkbox"]:checked').each(function () {
        var wiseid = $(this).attr('id');
        formData.append('wiseFileId', wiseid);
    });
    var reqid = $('.RequestsId').val();

    formData.append('reqid', reqid);
    console.log(formData);

    $.ajax({
        url: '/Admin/SendMail', // Replace with your controller action URL
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {
            $('#nav-home').html(response);

        },
        error: function (error) {
            console.error('Error uploading files:', error);
        }
    });
})




//// Event listener for download button
//document.getElementById('downloadSelected').addEventListener('click', function (event) {
//    event.preventDefault();
//    downloadFiles();
//});







function download() {
    var selectedFiles = document.querySelectorAll('input[name="checkbox"]:checked');
    var fileUrls = [];


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



