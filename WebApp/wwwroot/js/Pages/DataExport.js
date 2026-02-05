$(document).ready(function () {
    // Initialise the manage labels table
    if ($("#dataExportsTable").length)
        $("#dataExportsTable").DataTable({
            saveState: true,
            responsive: true,
            "scrollX": true, // If view-space is too narrow, adds a scrollbar.
            columnDefs: [
                { targets: 'no-sort', orderable: false }
            ],
            order: [[2, 'desc'], [1, 'desc']],
        });

    $('#SelectedTrialId').select2({
        theme: "bootstrap-5",
        selectionCssClass: "form-control",
        dropdownCssClass: "form-control",
        dropdownParent: $('#SelectedTrialId').parent()
    }).on('select2:open', function (e) {
        handleThemeChangeForSelect2();
    });

    handleThemeChangeForSelect2();
});

// This function is a copy from CreateWorkRequest.js to handle dark/light theme changes for Select2
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

// This targets the event created in themeToggleLogic.js switchTheme()
document.addEventListener('themeChanged', handleThemeChangeForSelect2);
