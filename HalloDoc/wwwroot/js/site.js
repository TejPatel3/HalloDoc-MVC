

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Dark Mode JS
//function myFunction() {
//    var element = document.body;
//    element.classList.toggle("dark-mode");
//}


//Login page JS show password
const togglePassword = document
    .querySelector('#togglePassword');
const password = document.querySelector('#password');
togglePassword.addEventListener('click', () => {
    // Toggle the type attribute using
    // getAttribure() method
    const type = password
        .getAttribute('type') === 'password' ?
        'text' : 'password';
    password.setAttribute('type', type);
    // Toggle the eye and bi-eye icon
    this.classList.toggle('bi-eye');
});


//dark mode

const userThemePreference = localStorage.getItem("theme");
let themeIcon = document.getElementById('theme-toggle');

if (userThemePreference === "dark") {
    document.body.classList.add("dark-mode");
}

function toggleTheme() {
    let themeIcon = document.getElementById('theme-toggle');


    document.body.classList.toggle("dark-mode");

    const currentTheme = document.body.classList.contains("dark-mode")
        ? "dark"
        : "light";
    localStorage.setItem("theme", currentTheme);

    console.log(currentTheme);
    if (currentTheme === 'light') {
        console.log("hello ji");
        themeIcon.setAttribute('src', '/images/night-mode.png');
    }
    else if (currentTheme === 'dark') {
        themeIcon.setAttribute('src', '/images/light-mode.png');
    }
    else {
        console.log("Can not change the image");
    }

}


// Contact number country option js

