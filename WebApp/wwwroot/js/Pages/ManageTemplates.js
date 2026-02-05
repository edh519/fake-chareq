$(document).ready(function ()
{
    if ($("#TemplatesTable").length)
        $("#TemplatesTable").DataTable({
            saveState: true,
            responsive: true,
            "oLanguage": {
                "sEmptyTable": "No templates found."
            },
            "scrollX": true, // If view-space is too narrow, adds a scrollbar.
            columnDefs: [
                { targets: 'no-sort', orderable: false }
            ],
            order: [[0, 'asc']],
        });
});


let archiveClick = async (templateId, isArchivedInt) =>
{
    let isArchivedBool = isArchivedInt === 1 ? true : false;

    let result = await PutTemplateArchiveStateApi(templateId, !isArchivedBool);
    if (!result.ok)
    {
        var errorTextElement = document.getElementById("popupMessageErrorText");
        errorTextElement.textContent = `Could not ${isArchived ? "unarchive" : "archive"} this template. Try refreshing this page.`
        runPopupMessages();
        return;
    }

    // TODO: edit the row data instead to avoid a refresh.
    document.location.reload(true);
    return;
}