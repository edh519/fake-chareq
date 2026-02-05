$(document).ready(function () {
    // site.js - Enables the label of files uploaded to appear on file upload fields.
    EnableCustomFileUploadLabels();

    // site.js - Initialises Popper tooltips
    InitialiseTooltips();

    // site.js - Initialised "Other" field visibility and clearing.
    InitialiseTrialsDropdown();

    // site.js - Set the text area sizes to be the content or 50px, whichever is bigger.
    ResizeTextAreas();

    // site.js - Set a message if the user tried to leave page. Cancel with AddLeaveMessage() on onclick event.
    AddLeaveMessage();
});
