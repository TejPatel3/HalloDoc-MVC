var regextext = /^[a-zA-Z][a-zA-Z-, ]+$/i;
var regionid;
window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-provider-tab').addClass('admin-layout-nav-active');
}
function loadData() {
    $.ajax({
        url: '/Provider/GetProviderTable',
        data: { 'regionid': regionid },
        success: function (data) {
            $('#ProviderTableDiv').html(data);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
loadData();

$('#RegionSearch').on('change', function () {
    regionid = $(this).val();
    console.log("ksd" + regionid)
    loadData();
});