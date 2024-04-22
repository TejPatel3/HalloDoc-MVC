window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-home-tab').addClass('admin-layout-nav-active');
}

$(document).ready(function () {
    $('.SelectedRegionGivePhysicianList').change(function () {
        var regionId = $(this).find(":selected").attr('id'); 
        $.ajax({
            url: '/Admin/GetPhysicianByRegionId', 
            type: 'GET',
            data: { regionId: regionId },
            success: function (data) {
                console.log(data)
                var secondDropdown = $('.physiciandrop'); 
                secondDropdown.empty(); 
                secondDropdown.append($('<option>', {
                    hidden: "hidden",
                    value: "",
                    text: "Select Physician"
                }))
                $.each(data, function (index, item) {
                    secondDropdown.append($('<option>', {
                        value: item.physicianId,
                        text: item.firstName + item.lastName 
                    }));
                });
            }
        });
    });

    $.ajax({
        url: '/ProviderSide/New',
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
                url = "/ProviderSide/New";
                status = 1;
                break;
            case '#status_pending':
                $('#statusnamehead').html('Pending');
                statusnamehead = "Pending";
                url = "/ProviderSide/Pending";
                status = 2;
                break;
            case '#status_active':
                $('#statusnamehead').html('Active');
                url = "/ProviderSide/Active";
                statusnamehead = "Active";
                status = 4
                break;
            case '#status_conclude':
                $('#statusnamehead').html('Conclude');
                statusnamehead = "Conclude";
                url = "/ProviderSide/Conclude";
                status = 6
                break;
            default:
                url = "/ProviderSide/New";
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
    selectedFiles.forEach(function (checkbox) {
        var row = checkbox.closest('tr');
        var fileUrl = row.querySelector('a').getAttribute('href');
        fileUrls.push(fileUrl);
    });
    fileUrls.forEach(function (url) {
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
            header.style.display = ''; 
        } else {
            header.style.display = 'none';
        }
    });
}

document.getElementById('my-search-input').addEventListener('keyup', filterAccordionSearch);



