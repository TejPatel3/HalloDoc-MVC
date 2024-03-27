window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-access-tab').addClass('admin-layout-nav-active');
}

$('#createaccessrole-btn').on('click', function () {
    console.log("jhdgsjhsg")

    $.ajax({
        url: '/Access/CreateRole',
        success: function (response) {
            $('#accessrole-maindiv').html(response); 
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('.accounttype-dropdown-createrole').on('change', function () {
    console.log($('.accounttype-dropdown-createrole').val());
    let accounttype = $('.accounttype-dropdown-createrole').val();
    $('#menuselect-filter').load('/Access/MenuFilterCheck', {accounttype:accounttype})

    //$.ajax({
    //    url: '/Access/MenuFilterCheckbox',
    //    data: { accounttype: accounttype },
    //    dataType:'html',
    //    succcess: function (response) {
    //        $('#menuselect-filter').empty();
    //        $('#menuselect-filter').html(response);
    //    },
    //    error: function (xhr, status, error) {
    //        console.error(error);
    //    }
    //})
});

$('#createrole-save').click(function () {
    console.log("run role")
    let rolename = $('#rolename-createrole').val();
    let accounttype = $('.accounttype-dropdown-createrole').val();
    let roleid = $('#roleid-createrole').val();

    let selectedrole = [];

    $('input[name="checkbox-createrole"]:checked').each(function () {
        selectedrole.push($(this).val());
    });

    console.log(rolename)
    console.log(accounttype)
    console.log(selectedrole)

    $.ajax({
        url: '/Access/CreateRole',
        type: 'POST',
        data: {
            rolename: rolename,
            accounttype: accounttype,
            selectedrole: selectedrole,
            roleid: roleid
        },
        success: function (response) {
            $('#accessrole-maindiv').html(response); 

        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    })

});

$('.editbtn-accessrole').click(function () {
    let roleid = $(this).val();
    console.log("sdsDS" + roleid);
    $.ajax({
        url: '/Access/CreateRole',
        data: { roleid: roleid },
        success: function (response) {
            $('#accessrole-maindiv').html(response);
            $('#roleid-createrole').val(roleid)
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});