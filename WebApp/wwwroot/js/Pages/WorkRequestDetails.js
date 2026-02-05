//GLOBAL DOM ELEMENT DECLARATIONS
const lightbox = $('#lightbox');
const lightboxImage = $('#lightbox-image');

$(document).ready(function () {
    // site.js - Set the text area sizes to be the content or 50px, whichever is bigger.
    ResizeTextAreas();
    InitialiseTooltips();

    $("#duration-days-wrapper").hide();

    if ($("#ProcessDeviationReason_Reason"))
        $("#process-deviation-reason-wrapper").hide();

    // Add event listener for tabs (authorisations) when shown
    $(".custom-nav-tab.nav-link").on('shown.bs.tab', function (event) {
        ResizeTextAreas();
    });

    // If editing an authorisation, make sure the user can't leave without confirming.
    let urlParams = new URLSearchParams(window.location.search);
    let status = urlParams.get("editAuthorisation");
    switch (status?.toUpperCase()) {
        case ("INITIALAUTH"):
        case ("FINALAUTH"):
            AddLeaveMessage();
            break;
        default:
            break;
    }

    $("#getUrlForPrint").text(window.location.href);

    //Add event listeners for image lightbox
    $(".chareq-message-box p img").on('click', function (e) {
        e.preventDefault();
        const imageSource = $(this).attr('src');
        lightboxImage.attr('src', imageSource);

        lightbox.show();
        setTimeout(function () {
            lightbox.addClass('active');
        })

        $('body').css('overflow', 'hidden');
    })

    $('.lightbox-close').on('click', closeLightbox);

    $('#lightbox').on('click', function (e) {
        // Check if the click target is the overlay itself or the close button
        if (e.target.id === 'lightbox' || $(e.target).hasClass('lightbox-close')) {
            closeLightbox();
        }
    });

    $(document).on('keydown', function (e) {
        if (e.key === 'Escape' && lightbox.hasClass('active')) {
            closeLightbox();
        }
    });
});

function closeLightbox() {
    lightbox.removeClass('active');
    $('body').css('overflow', '');

    setTimeout(function () {
        lightbox.hide();
    }, 300);
}

let toggleSidePanelCollapse = (element) => {
    // sidepanel-collapse elements are handled by data-bs-toggle="collapse".


    // Toggle inversecollapse elements to hide/show. Can't be triggered by data-bs-toggle="collapse" as they are inverse.
    let elements = $(`[name='sidepanel-inversecollapse']`);
    for (var i = 0; i < elements.length; i++) {
        elements[i].classList.toggle("d-none");
    }

    let icon = element.getElementsByTagName('i')[0];
    if (icon.classList.contains('fa-eye-slash')) {
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
        icon.title = 'Show details panel';
    } else {
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
        icon.title = 'Hide details panel';
    }

    $(icon).tooltip('dispose');
    InitialiseTooltips();
}

let toggleWidescreen = (element) => {

    let container = $('#container');
    let icon = element.getElementsByTagName('i')[0];

    if (container.hasClass('container')) {
        container.removeClass('container');
        icon.classList.remove('fa-expand-arrows-alt');
        icon.classList.add('fa-compress-arrows-alt');
        icon.title = 'Use reader mode';
    } else {
        container.addClass('container');
        icon.classList.remove('fa-compress-arrows-alt');
        icon.classList.add('fa-expand-arrows-alt');
        icon.title = 'Use widescreen mode';
    }

    $(icon).tooltip('dispose');
    InitialiseTooltips();

}

let copyEmailToClipboard = (element) => {
    let email = element.dataset.email;
    navigator.clipboard.writeText(email.toLowerCase());

    clientSidePopupMessage(true, "Email copied to clipboard")
}


let QuickAddEstimate_Click = (value = 0) => {
    let estTimeImpactElement = $("#duration-days");
    let initialValue = estTimeImpactElement.val();

    // RESET Button
    if (value == 0) {
        estTimeImpactElement.val("");
        return;
    }

    // If default value or empty, use the button value.
    if (initialValue == 0) {
        estTimeImpactElement.val(value);
    }
    // Add the value to the initial value, works with negatives too.
    else {
        estTimeImpactElement.val(Number(initialValue) + Number(value));
    }
    estTimeImpactElement.change();
    return;
}

let EmptyDescriptionCheck = (index) => {
    const addBtn = $(`#textboxButton_${index}`);
    const eventTypeSelect = $(`#eventTypeSelect_${index}`);
    const eventTypeValue = eventTypeSelect.val();
    const addBtnWrapper = $(`#textboxButtonWrapper_${index}`);
    const message = $(`#eventTextEntry_${index}_ifr`).contents().find('body').text().trim();

    if (message.length === 0) {
        addBtn.prop("disabled", true);
        addBtn.addClass('disabled');

        addBtnWrapper
            .attr('title', 'Message content cannot be empty')
            .tooltip('dispose')
            .tooltip({ trigger: 'hover' });

    } else if (eventTypeValue == null){
        addBtn.prop("disabled", true);
        addBtn.addClass('disabled');

        addBtnWrapper
            .attr('title', 'Select a message type')
            .tooltip('dispose')
            .tooltip({ trigger: 'hover' });
    }
    else {
    
        addBtn.prop("disabled", false);
    addBtn.removeClass('disabled');

    addBtnWrapper
        .attr('title', '')
        .tooltip('dispose');
    }
};




$("#duration-days").change(function () {
    let estTimeImpactUnitsElement = $("#duration-days-units");

    if ($(this).val() === "1") {
        estTimeImpactUnitsElement.html("day");
    } else {
        estTimeImpactUnitsElement.html("days");
    }
});

//Event Listener for new conversation type select - change colour of add button
//NB: Default colour is Primary. Only change for Enquiry, Complete or Closed
$("#eventTypeSelect").change(function () {

    let addBtn = $("#eventAddButton");
    let eventTypeSelect = $("#eventTypeSelect");
    let durationDaysWrapper = $("#duration-days-wrapper");
    let durationDaysInput = $("#duration-days");

    let processDeviationReason = $("#ProcessDeviationReason_Reason");
    let processDeviationReasonWrapper = $("#process-deviation-reason-wrapper");

    addBtn.removeClassStartingWith('btn\-'); //Regex extension method, so escape special chars.

    //None
    if (eventTypeSelect.val() == 1) {
        addBtn.addClass("btn-primary");
        durationDaysWrapper.hide();
        durationDaysInput.val("");

        if (processDeviationReason) {
            processDeviationReason.val("");
            processDeviationReasonWrapper.hide();
        }
    }
    //Enquiry
    else if (eventTypeSelect.val() == 2) {
        addBtn.addClass("btn-warning");
        durationDaysWrapper.hide();
        durationDaysInput.val("");
    }
    //Approve
    else if (eventTypeSelect.val() == 3) {
        addBtn.addClass("btn-primary");
        durationDaysWrapper.show();
    }
    //Complete
    else if (eventTypeSelect.val() == 4) {
        addBtn.addClass("btn-success");
        durationDaysWrapper.show();

        if (processDeviationReason) {
            processDeviationReason.prop("required", true);
            processDeviationReasonWrapper.show();
        }

    }
    //Close
    else if (eventTypeSelect.val() == 5) {
        addBtn.addClass("btn-danger");
        durationDaysWrapper.hide();
        durationDaysInput.val("");
    }
    else {
        addBtn.addClass("btn-primary");
        durationDaysWrapper.hide();
        durationDaysInput.val("");
    }
})

function SubscribeUnsubscribeUserWorkRequest(btn) {

    let token = $("input[name='__RequestVerificationToken']").val();

    let workRequestId = $(btn).data("workrequestid");

    let currentUserId = $(btn).data("currentuserid");
    let targetUserId = $(btn).data("targetuserid");

    var dataRequestUrl = $(btn).data('request-url');

    var data = { workRequestId: workRequestId, currentUserId: currentUserId, targetUserId: targetUserId };
    $.ajax({
        url: dataRequestUrl,
        type: 'POST',
        headers:
        {
            "RequestVerificationToken": token
        },
        data: data,
        success: function (response) {
            location.reload();
        },
        error: function () {
            alert("Error processing subscription request.");
        }
    });

    return;
}

let SubscribeOtherUser = (btn) => {
    let modal = new bootstrap.Modal(document.getElementById('subscribeOtherUserModal'));
        modal.show();
};

let UpdateWorkRequestProject = async (workRequestId, projectId) => {
    if (!workRequestId) {
        clientSidePopupMessage(false, `Work Request not found. Could not update linked project. Try refreshing this page.`)
        return;
    } else if (!projectId) {
        clientSidePopupMessage(false, `Project not found. Could not update linked project. Try refreshing this page.`)
        return;
    }

    let result = await PutProjectIdOnWorkRequestApi(workRequestId, projectId);
    if (!result.ok) {
        clientSidePopupMessage(false, `Could not update linked project. Try refreshing this page.`)
        return;
    }

    document.location.reload(true);
    return;
}

function ShowSubscriberSpinner(btn) {
    if ($('#subscribeOtherUserForm').valid()) {
        $("#subscriberSpinner").show();
        $(btn).hide();
    }
}

$('#SubscribableUsersMultiSelect').select2({
    theme: "bootstrap-5",
    selectionCssClass: "form-control",
    dropdownCssClass: "form-control",
    dropdownParent: $('#subscribeOtherUserModal'),
    placeholder: "-- Select user(s) to subscribe --"
}).on('select2:open', function (e) {
    handleThemeChangeForSelect2();
});

let UpdateSubTaskAssignee = async (subTaskId, assigneeEmail, button) => {
    if (button) {
        button.style.display = "none";
    }

    let dropdown = document.getElementById(`subTaskAssigneeSelect-${subTaskId}`);
    if (dropdown) {
        dropdown.disabled = true;
    }

    if (!subTaskId) {
        clientSidePopupMessage(false, `Sub Task not found. Could not update sub task assignee. Try refreshing this page.`)
        return;
    } else if (!subTaskId) {
        clientSidePopupMessage(false, `Sub Task not found. Could not update linked sub task assignee. Try refreshing this page.`)
        return;
    }

    let result = await PutAssigneeEmailOnSubTaskApi(subTaskId, assigneeEmail);
    if (!result.ok) {
        clientSidePopupMessage(false, `Could not update linked sub task assignee. Try refreshing this page.`)
        return;
    }

    document.location.reload(true);
    return;
}

$('#copyUrlElement').on('click', function () {
    var currentUrl = window.location.href;

    //Can only 'really' copy to clipboard from DOM element
    //Create temp element, paste url, copy to clipboard, delete element
    var tempInput = document.createElement('input');
    tempInput.value = currentUrl;
    document.body.appendChild(tempInput);
    tempInput.select();
    tempInput.setSelectionRange(0, 99999); // For mobile devices

    document.execCommand('copy');

    clientSidePopupMessage(true, "Request URL copied to clipboard")

    document.body.removeChild(tempInput);
})

function toggleAssigneeDropdown(id) {
    const pill = document.getElementById(`assignee-pill-${id}`);
    const dropdown = document.getElementById(`assignee-dropdown-${id}`);
    pill.classList.add('d-none');
    dropdown.classList.remove('d-none');
}

function assignUser(select, id) {
    const selectedEmail = select.value;

    // TODO: Add AJAX or form post here to actually assign the user
    console.log(`Assigning user ${selectedEmail} to subtask ${id}`);

    // You can also update the badge text and show it again
    const pill = document.getElementById(`assignee-pill-${id}`);
    pill.querySelector('i').nextSibling.textContent = selectedEmail.split('@')[0]; // simple domain removal
    pill.setAttribute('title', selectedEmail);
    pill.classList.remove('d-none');
    select.classList.add('d-none');
}


function showAdditionalMessageTextBox(isExported) {
    const textbox = document.getElementById("mainTextBoxWrapper");
    const button = document.getElementById("additionalMessageButton");
    const disclaimer = document.getElementById("disclaimer");
    const dangerButton = document.getElementById("eventAddButton");
    const exportDisclaimer = document.getElementById("exportDisclaimer");

    const isHidden = textbox.style.display === "none" || textbox.style.display === "";

    if (isHidden) {
        textbox.style.display = "block";
        button.textContent = "Hide text box";
        button.classList.remove("btn-primary");
        button.classList.add("btn-danger");

        dangerButton.classList.remove("btn-primary");
        dangerButton.classList.add("btn-danger");

        if (isExported === true) {
            exportDisclaimer.style.display = "block";
        }
        else {
            disclaimer.style.display = "block";
        }

    } else {
        textbox.style.display = "none";
        button.textContent = "Add additional message";
        button.classList.remove("btn-danger");
        button.classList.add("btn-primary");

        disclaimer.style.display = "none";
        exportDisclaimer.style.display = "none";

        dangerButton.classList.remove("btn-danger");
        dangerButton.classList.add("btn-primary");
    }
}

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

