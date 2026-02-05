/*IMPORTANT: script required to be first thing run on page to prevent
* light theme flicker when in dark mode when loading
*/

//sets the data-theme on the document to trigger css theme change
function setDataTheme(theme) {
    if (theme === "dark") {
        document.documentElement.setAttribute("data-theme", theme);
    } else {
        document.documentElement.removeAttribute("data-theme");
    }
}

//local storage is used to override OS theme settings
if (localStorage.getItem("theme") == "dark") {
    setDataTheme("dark");
} else if (localStorage.getItem("theme") == "light") {
    setDataTheme("light");
} else if (window.matchMedia("(prefers-color-scheme: dark)").matches) {
    //OS theme setting detected as dark
    setDataTheme("dark");
} else {
    //default to light
    setDataTheme("light");
}

//toggles the theme between light and dark
function switchTheme() {
    if (document.documentElement.getAttribute("data-theme") != "dark") {
        //set localstorage and css theme to dark
        localStorage.setItem('theme', 'dark');
        document.documentElement.setAttribute('data-theme', 'dark');
        UpdateTinyMCETheme("dark");
    } else {
        //set localstorage and css theme to light
        localStorage.setItem('theme', 'light');
        document.documentElement.removeAttribute("data-theme");
        UpdateTinyMCETheme("light");
    }
    document.dispatchEvent(new Event('themeChanged'));
}