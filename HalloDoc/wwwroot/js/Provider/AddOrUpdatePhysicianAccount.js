var regextext = /^[a-zA-Z][a-zA-Z0-9 ]+$/i;
var regexlicense = /^[a-zA-Z0-9][a-zA-Z0-9 ]+$/i;
var regexemail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
var regexphone = /^[1-9][0-9]{9}$/;
var regexstrongpassword = /[A-Za-z\d@$!%*?&]{8,}/
window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-provider-tab').addClass('admin-layout-nav-active');
}
var actualBtn = document.getElementById('actual-btn-editprovideraccount');
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
});
var actualBtnsign = document.getElementById('actual-btn-sign');
var fileChosensign = document.getElementById('file-chosen-sign');
actualBtnsign.addEventListener('change', function () {
    var filesname = this.files[0].name;
    console.log(filesname)
    for (var i = 1; i < this.files.length; i++) {
        filesname = filesname + " + " + this.files[i].name;
    }
    fileChosensign.style.fontSize = "15px";
    fileChosensign.style.fontWeight = "bold"
    fileChosensign.textContent = filesname;
    fileChosensign.ariaPlaceholder = filesname;
    console.log("run")
});
$('#username-editprovideraccount').on('input', function () {
    let username = $('#username-editprovideraccount').val();
    $('#username-editprovideraccount-span').html("");
    if (username == "") {
        $('#username-editprovideraccount-span').html("please enter username");
    }
    else if (!regextext.test(username)) {
        $('#username-editprovideraccount-span').html("username not valid");

    }
});
$('#Aieditbtn-editprovideraccount').on('click', function () {
    $('#Aisavebtn-editprovideraccount').removeClass('d-none')
    $('.aidisabled').removeAttr('disabled', false);
    $(this).addClass('d-none')
});
$('#Aisavebtn-editprovideraccount').on('click', function () {
    let username = $('#username-editprovideraccount').val();
    let physicianid = $('#physicianid-editprovideraccount').val();
    console.log(physicianid)
    var model = {
        physicianid: physicianid,
        username: username,
    }

    if ($('#username-editprovideraccount-span').html() == "") {
        $('#Aieditbtn-editprovideraccount').removeClass('d-none')
        $('.aidisabled').attr('disabled', true);
        $(this).addClass('d-none');

        $.ajax({
            url: '/Provider/EditProviderAccount',
            type: 'POST',
            data: model,
            success: function (response) {
                //$('#nav-profile').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});
$('#pieditbtn-editprovideraccount').on('click', function () {

    $('.pidisabled').removeAttr('disabled', false);
    $('#pisavebtn-editprovideraccount').removeClass('d-none');
    $(this).addClass('d-none');
});


//$('#firstname-editprovideraccount').on('input', function () {
//    let firstname = $('#firstname-editprovideraccount').val();

//    $('#firstname-editprovideraccount-span').html("");
//    if (firstname == "") {
//        $('#firstname-editprovideraccount-span').html("please enter firstname");
//        allvalidation = false;
//    }
//    else if (!regextext.test(firstname)) {
//        $('#firstname-editprovideraccount-span').html("firstname not valid");
//        allvalidation = false;

//    }
//});

//$('#lastname-editprovideraccount')
//$('#phone-editprovideraccount')

//$('#medicallicense-editprovideraccount')
//$('#npinumber-editprovideraccount')




$('#pisavebtn-editprovideraccount').on('click', function () {
    let physicianid = $('#physicianid-editprovideraccount').val();

    let firstname = $('#firstname-editprovideraccount').val();
    let lastname = $('#lastname-editprovideraccount').val();
    let email = $('#email-editprovideraccount').val();
    let phonenumber = $('#phone-editprovideraccount').val();
    let medicallicense = $('#medicallicense-editprovideraccount').val();
    let npinum = $('#npinumber-editprovideraccount').val();
    let regionlist = $('#username-editprovideraccount').val();
    let allvalidation = true;
    var selectedregion = [];

    $('input[type="checkbox"]:checked').each(function () {
        selectedregion.push($(this).val());
    });
    var model = {
        physicianid: physicianid,
        firstname: firstname,
        lastname: lastname,
        email: email,
        phonenumber: phonenumber,
        medicallicencenumber: medicallicense,
        npinumber: npinum,
        selectedregion: selectedregion
    }
    $('#lastname-editprovideraccount-span').html("");
    $('#email-editprovideraccount-span').html("");
    $('#phone-editprovideraccount-span').html("");
    $('#medicallicense-editprovideraccount-span').html("");
    $('#npinumber-editprovideraccount-span').html("");
    $('#firstname-editprovideraccount-span').html("");
    if (firstname == "") {
        $('#firstname-editprovideraccount-span').html("please enter firstname");
        allvalidation = false;
    }
    else if (!regextext.test(firstname)) {
        $('#firstname-editprovideraccount-span').html("firstname not valid");
        allvalidation = false;

    }
    if (lastname == "") {
        $('#email-editprovideraccount-span').html("please enter lastname");
        allvalidation = false;
    }
    else if (!regextext.test(lastname)) {
        $('#email-editprovideraccount-span').html("lastname not valid");
        allvalidation = false;
    }
    if (email == "") {
        $('#firstname-editprovideraccount-span').html("please enter email");
        allvalidation = false;
    }
    else if (!regexemail.test(email)) {
        $('#firstname-editprovideraccount-span').html("email not valid");
        allvalidation = false;
    }
    if (phonenumber == "") {
        $('#phone-editprovideraccount-span').html("please enter phonenumber");
        allvalidation = false;
    }
    else if (!regexphone.test(phonenumber)) {
        $('#phone-editprovideraccount-span').html("phonenumber not valid");
        allvalidation = false;
    }
    if (medicallicense == "") {
        $('#medicallicense-editprovideraccount-span').html("please enter medical license");
        allvalidation = false;
    }
    else if (!regexlicense.test(medicallicense)) {
        $('#medicallicense-editprovideraccount-span').html("medical license not valid");
        allvalidation = false;
    }
    if (npinum == "") {
        $('#npinumber-editprovideraccount-span').html("please enter NPI number");
        allvalidation = false;
    }
    else if (!regexlicense.test(npinum)) {
        $('#npinumber-editprovideraccount-span').html("NPI number not valid");
        allvalidation = false;
    }
    if (allvalidation) {
        $('#pieditbtn-editprovideraccount').removeClass('d-none')
        $('.pidisabled').attr('disabled', true);
        $(this).addClass('d-none');
        $.ajax({
            url: '/Provider/EditProviderAccount',
            type: 'POST',
            data: model,
            success: function (response) {
                //$('#nav-profile').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });


    }
});

$('#password-editprovideraccount').on('input', function () {
    let password = $('#password-editprovideraccount').val();
    $('#resetpassword-editprovideraccount-span').html("");
    if (password == "") {

        $('#resetpassword-editprovideraccount-span').html("please enter password");
    }
    else if (!regexstrongpassword.test(password)) {
        $('#resetpassword-editprovideraccount-span').html("password should contain uppercase, lowercase, digit, symbol and atleast 8 character");
    }
});
$('#Airesetpasswordbtn-editprovideraccount').on('click', function () {
    $('#password-editprovideraccount').removeAttr('disabled', false);
    $(this).addClass('d-none');
    $('#Aiupdatebtn-editprovideraccount').removeClass('d-none');
});
$('#Aiupdatebtn-editprovideraccount').on('click', function () {
    let password = $('#password-editprovideraccount').val();
    console.log("ajhsjia" + password)
    let physicianid = $('#physicianid-editprovideraccount').val();
    var model = {
        physicianid: physicianid,
        password: password

    };
    $('#resetpassword-editprovideraccount-span').html("");
    if (password == "") {

        $('#resetpassword-editprovideraccount-span').html("please enter password");
    }
    else if (!regexstrongpassword.test(password)) {
        alert("Please Enter Strong Password that contain\nAt least one uppercase letter\nAt least one lowercase letter\nAt least one digit\nAt least one special character\nAt least 8 characters in length")
    }
    else {
        $.ajax({
            url: '/Provider/EditProviderAccount',
            type: 'POST',
            data: model,
            success: function (response) {
                //$('#nav-profile').html(response);
                $('#password-editprovideraccount').attr('disabled', true);
                $('#Aiupdatebtn-editprovideraccount').addClass('d-none');
                $('#Airesetpasswordbtn-editprovideraccount').removeClass('d-none');
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});




(function fun() {
    window.requestAnimFrame = (function (callback) {
        return window.requestAnimationFrame ||
            window.webkitRequestAnimationFrame ||
            window.mozRequestAnimationFrame ||
            window.oRequestAnimationFrame ||
            window.msRequestAnimaitonFrame ||
            function (callback) {
                window.setTimeout(callback, 1000 / 60);
            };
    })();

    var canvas = document.getElementById("sig-canvas");
    var ctx = canvas.getContext("2d");
    ctx.strokeStyle = "#222222";
    ctx.lineWidth = 4;

    var drawing = false;
    var mousePos = {
        x: 0,
        y: 0
    };
    var lastPos = mousePos;

    canvas.addEventListener("mousedown", function (e) {
        drawing = true;
        lastPos = getMousePos(canvas, e);
    }, false);

    canvas.addEventListener("mouseup", function (e) {
        drawing = false;
    }, false);

    canvas.addEventListener("mousemove", function (e) {
        mousePos = getMousePos(canvas, e);
    }, false);

    // Add touch event support for mobile
    canvas.addEventListener("touchstart", function (e) {

    }, false);

    canvas.addEventListener("touchmove", function (e) {
        var touch = e.touches[0];
        var me = new MouseEvent("mousemove", {
            clientX: touch.clientX,
            clientY: touch.clientY
        });
        canvas.dispatchEvent(me);
    }, false);

    canvas.addEventListener("touchstart", function (e) {
        mousePos = getTouchPos(canvas, e);
        var touch = e.touches[0];
        var me = new MouseEvent("mousedown", {
            clientX: touch.clientX,
            clientY: touch.clientY
        });
        canvas.dispatchEvent(me);
    }, false);

    canvas.addEventListener("touchend", function (e) {
        var me = new MouseEvent("mouseup", {});
        canvas.dispatchEvent(me);
    }, false);

    function getMousePos(canvasDom, mouseEvent) {
        var rect = canvasDom.getBoundingClientRect();
        return {
            x: mouseEvent.clientX - rect.left,
            y: mouseEvent.clientY - rect.top
        }
    }

    function getTouchPos(canvasDom, touchEvent) {
        var rect = canvasDom.getBoundingClientRect();
        return {
            x: touchEvent.touches[0].clientX - rect.left,
            y: touchEvent.touches[0].clientY - rect.top
        }
    }

    function renderCanvas() {
        if (drawing) {
            ctx.moveTo(lastPos.x, lastPos.y);
            ctx.lineTo(mousePos.x, mousePos.y);
            ctx.stroke();
            lastPos = mousePos;
        }
    }

    // Prevent scrolling when touching the canvas
    document.body.addEventListener("touchstart", function (e) {
        if (e.target == canvas) {
            e.preventDefault();
        }
    }, false);
    document.body.addEventListener("touchend", function (e) {
        if (e.target == canvas) {
            e.preventDefault();
        }
    }, false);
    document.body.addEventListener("touchmove", function (e) {
        if (e.target == canvas) {
            e.preventDefault();
        }
    }, false);

    (function drawLoop() {
        requestAnimFrame(drawLoop);
        renderCanvas();
    })();

    function clearCanvas() {
        canvas.width = canvas.width;
    }

    // Set up the UI
    var clearBtn = document.getElementById("sig-clearBtn");
    var submitBtn = document.getElementById("sig-submitBtn");
    clearBtn.addEventListener("click", function (e) {
        clearCanvas();
    }, false);
    submitBtn.addEventListener("click", function (e) {
        var dataUrl = canvas.toDataURL();
        let providerid = $('#physicianid-editprovideraccount').val();

        console.log(dataUrl);
        $.ajax({
            url: '/Provider/EditProviderSign',
            type: 'POST',
            data: { providerid: providerid, base64string: dataUrl },
            success: function (response) {
                $('#Cancelbutton').trigger('click');
                $('#provider-maindiv').html(response);
            }
        });
    }, false);

})();



$('#provider-sign').on('click', function () {
    debugger
    var fileInput = document.querySelector('#actual-btn-sign');
    var file = fileInput.files[0];
    var reader = new FileReader();
    let providerid = $('#physicianid-editprovideraccount').val();

    reader.onloadend = function () {
        var base64String = reader.result;
        $.ajax({
            url: '/Provider/EditProviderSign',
            type: 'POST',
            data: { providerid, base64String },
            success: function (response) {
                $('#provider-maindiv').html(response);

            }
        });
    };
    reader.readAsDataURL(file);
});


$('#provider-photo').on('click', function () {
    debugger
    var fileInput = document.querySelector('#actual-btn-editprovideraccount');
    var file = fileInput.files[0];
    var reader = new FileReader();
    let providerid = $('#physicianid-editprovideraccount').val();

    reader.onloadend = function () {
        var base64String = reader.result;
        $.ajax({
            url: '/Provider/EditProviderPhoto',
            type: 'POST',
            data: { providerid, base64String },
            success: function (response) {
                $('#provider-maindiv').html(response);

            }
        });
    };
    reader.readAsDataURL(file);
});

$('#editbtn-adminnote-editprovider').on('click', function () {
    $(this).addClass('d-none');
    $('#adminnote-editprovider').removeAttr('disabled', false);
    $('#savebtn-adminnote-editprovider').removeClass('d-none');
});
$('#adminnote-editprovider').on('input', function () {
    let adminnote = $('#adminnote-editprovider').val();
    $('#adminnote-editprovider-span').html("");
    if (adminnote == "") {
        $('#adminnote-editprovider-span').html("Please enter adminnote");

    }
    else if (!regextext.test()) {

    }
});
$('#savebtn-adminnote-editprovider').on('click', function () {

});