﻿var regextext = /^[a-zA-Z-, ]+$/i;
window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-access-tab').addClass('admin-layout-nav-active');
}

$('#rolename-createrole').on('blur', function () {
    let rolename = $('#rolename-createrole').val();
    let roleid = $('#roleid-createrole').val();
    $.ajax({
        url: '/Access/IsRoleExist',
        type: 'POST',
        data: {
            rolename: rolename,
            roleid: roleid
        },
        success: function (response) {
            if (!response) {
                Swal.fire('Role exist');
                $('#rolename-createrole').val('');
            }
        },

    })
})

$('#createaccessrole-btn').on('click', function () {
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
    let accounttype = $('.accounttype-dropdown-createrole').val();
    $('#menuselect-filter').load('/Access/MenuFilterCheck', { accounttype: accounttype })
});

$('#rolename-createrole').on('input', function () {
    let rolename = $('#rolename-createrole').val();
    let selectedmenu = [];
    let allValidation = true; $('input[name="checkbox-createrole"]:checked').each(function () {
        selectedmenu.push($(this).val());
    });
    $('#rolename-createrole-span').html("");
    if (rolename == "") {
        $('#rolename-createrole-span').html("please enter role name");
    }
    else if (!regextext.test(rolename)) {
        $('#rolename-createrole-span').html("role name is not valid");
    }
});

$('#createrole-save').click(function () {
    let rolename = $('#rolename-createrole').val();
    let accounttype = $('.accounttype-dropdown-createrole').val();
    let roleid = $('#roleid-createrole').val();
    let selectedmenu = [];
    let allValidation = true;
    $('input[name="checkbox-createrole"]:checked').each(function () {
        selectedmenu.push($(this).val());
    });

    $('#rolename-createrole-span').html("");
    $('#adminselectedregion-adminprofile-span').html("")
    if (rolename == "") {
        $('#rolename-createrole-span').html("please enter role name");
        allValidation = false;
    }
    if (selectedmenu == "") {
        $('#adminselectedregion-adminprofile-span').html("please select at least one menu");
        allValidation = false;
    }
    if (allValidation) {
        $.ajax({
            url: '/Access/CreateRole',
            type: 'POST',
            data: {
                rolename: rolename,
                accounttype: accounttype,
                selectedmenu: selectedmenu,
                roleid: roleid
            },
            success: function (response) {
                $('#adminLayoutMainDiv').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});

$('.editbtn-accessrole').click(function () {
    let roleid = $(this).val();
    let accounttype = $(this).data('id');
    console.log(accounttype)
    $.ajax({
        url: '/Access/CreateRole',
        data: { roleid: roleid, accounttype: accounttype, },
        success: function (response) {
            $('#accessrole-maindiv').html(response);
            $('#roleid-createrole').val(roleid)
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('.deletebtn-accessrole').click(function () {
    let roleid = $(this).val();
    $.ajax({
        url: '/Access/DeleteRole',
        data: {
            roleid: roleid
        },
        success: function (response) {
            $('#adminLayoutMainDiv').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('#createrole-cancel').click(function () {
    window.location.href = "https://localhost:44370/Access/AccessRole";
});

$('.editbtn-Physician').click(function () {
    var physicianId = $(this).val();
    $.ajax({
        url: '/Provider/EditProviderAccount',
        type: 'POST',
        data: { physicianid :physicianId},
        success: function (response) {
            $('#UserAccess-maindiv').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});
$('.editbtn-Admin').click(function () {
    var adminid = $(this).val();
    $.ajax({
        url: '/Access/AdminProfileFromUserAccess',
        type: 'POST',
        data: { adminid: adminid },
        success: function (response) {
            $('#UserAccess-maindiv').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});