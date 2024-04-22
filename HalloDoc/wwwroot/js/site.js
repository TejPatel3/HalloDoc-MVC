//dark mode

var userThemePreference = localStorage.getItem("theme");
let themeIcon = document.getElementById('theme-toggle');

if (userThemePreference === "dark") {
    document.body.classList.add("dark-mode");
}
function toggleTheme() {
    let themeIcon = document.getElementById('theme-toggle');
    document.body.classList.toggle("dark-mode");
    var currentTheme = document.body.classList.contains("dark-mode")
        ? "dark"
        : "light";
    localStorage.setItem("theme", currentTheme);
    console.log(themeIcon);

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



