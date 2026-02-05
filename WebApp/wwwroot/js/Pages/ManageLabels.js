$(document).ready(function () {
    InitialiseTooltips();

    // If add OR edit page. Do not shuffle hex color on edit page.
    if ($("#HexColor").length) {
        if ($("#LabelId").length === 0)
            shuffleHexColor();
        updatePreview();
    }


    // Initialise the manage labels table
    if ($("#LabelsTable").length)
        $("#LabelsTable").DataTable({
            saveState: true,
            responsive: true,
            "oLanguage": {
                "sEmptyTable": "No labels found."
            },
            "scrollX": true, // If view-space is too narrow, adds a scrollbar.
            columnDefs: [
                { targets: 'no-sort', orderable: false }
            ],
            order: [[1, 'asc']],
        });
});

let archiveClick = async (labelId, isArchivedInt) => {
    let isArchivedBool = isArchivedInt === 1 ? true : false; 

    let result = await PutLabelArchiveStateApi(labelId, !isArchivedBool);
    if (!result.ok) {
        var errorTextElement = document.getElementById("popupMessageErrorText");
        errorTextElement.textContent = `Could not ${isArchived ? "unarchive" : "archive"} this label. Try refreshing this page.`
        runPopupMessages();
        return;
    }

    // TODO: edit the row data instead to avoid a refresh.
    document.location.reload(true);
    return;
}

// Taken from: https://www.tutorialspoint.com/generating-random-hex-color-in-javascript
let shuffleHexColor = () => {
    let color = '#';
    for (let i = 0; i < 6; i++) {
        const random = Math.random(); // NOSONAR this is not security related
        const bit = (random * 16) | 0;
        color += (bit).toString(16);
    };
    $("#HexColor").val(color);

    updatePreview();
    return;
}

let updatePreview = () => {
    let labelText = $("#LabelShort").val();
    let labelTooltip = $("#LabelDescription").val();
    let labelHexColor = $("#HexColor").val();

    let labelPreview = $("#labelPreview");
    labelPreview.tooltip("dispose"); // Dispose of popper tooltip before changes are made.

    // Change values on label preview.
    labelPreview.css({ "background-color": labelHexColor, "color": CalculateBestTextColor(labelHexColor) });
    labelPreview.attr("title", labelTooltip);
    labelPreview.text(labelText);

    labelPreview.tooltip("enable"); // Re-enable popper tooltip now changes are complete.
    return;
}
