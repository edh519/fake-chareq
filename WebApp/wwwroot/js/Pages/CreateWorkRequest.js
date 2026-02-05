$(document).ready(function () {
    // site.js - Enables the label of files uploaded to appear on file upload fields.
    EnableCustomFileUploadLabels();

    // site.js - Initialises Popper tooltips
    InitialiseTooltips();

    // site.js - Initialises "Other" field visibility and clearing.
    InitialiseTrialsDropdown();

    // site.js - Set the text area sizes to be the content or 50px, whichever is bigger.
    ResizeTextAreas();

    $("#spinner").hide();

    $('.carousel').carousel();

    $('#Trial').select2({
        theme: "bootstrap-5",
        selectionCssClass: "form-control",
        dropdownCssClass: "form-control",
        dropdownParent: $('body')
    }).on('select2:open', function (e) {
        handleThemeChangeForSelect2();
    });

    $('#TrialMultiSelect').select2({
        theme: "bootstrap-5",
        selectionCssClass: "form-control",
        dropdownCssClass: "form-control",
        dropdownParent: $('body'),
        placeholder: "-- Select Trial Name(s) --"
    }).on('select2:open', function (e) {
        handleThemeChangeForSelect2();
    });

    handleThemeChangeForSelect2();
});

function ShowSpinner(btn) {
    $("#spinner").show();
    $(btn).hide();
};

//Copy of select2 onopen
function handleThemeChangeForSelect2() {
    var theme = document.documentElement.getAttribute('data-theme');
    var isDark = theme === 'dark';

    $('.select2-container').each(function () {
        var container = $(this);
        if (isDark) {
            container.attr('data-theme', 'dark');
        } else {
            container.removeAttr('data-theme');
        }
    });

    var dropdown = $('.select2-dropdown');
    if (dropdown.length > 0) {
        if (isDark) {
            dropdown.attr('data-theme', 'dark');
        } else {
            dropdown.removeAttr('data-theme');
        }
    }
}

//This targets event created in themeToggleLogic.js switchTheme()
document.addEventListener('themeChanged', handleThemeChangeForSelect2);