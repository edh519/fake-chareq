

let AddLeaveMessage = () => {
    // Check before user leaves page.
    window.onbeforeunload = function () {
        return true;
    };
}

let RemoveLeaveMessage = () => {
    window.onbeforeunload = null;
}


let ResizeTextAreas = () => {
    // Get all textareas and loop through them
    let textAreas = document.getElementsByTagName("textarea");
    if (textAreas.length == 0)
        return;

    for (var i = 0; i < textAreas.length; i++) {

        // Reset the height, then check if the min height of 50px is subceeded.
        textAreas[i].style.height = "";
        if ((textAreas[i].scrollHeight + 3) < 50)
            textAreas[i].style.height = "50px";
        else
            textAreas[i].style.height = (textAreas[i].scrollHeight + 3) + "px"

        // Add event listener onInput for all that aren't readonly.
        if (!textAreas[i].readOnly) {
            textAreas[i].addEventListener("input", function (e) {
                this.style.height = "";
                if ((this.scrollHeight + 3) < 50)
                    this.style.height = "50px";
                else
                    this.style.height = (this.scrollHeight + 3) + "px"
            },
                false);
        }
    }
}


/* Enables the label of files uploaded to appear on file upload fields. */
let EnableCustomFileUploadLabels = () => {
    $('.custom-file input').change(function (e) {
        let fileInputLabel = $(this).next('.custom-file-label');
        let widthAvailableForText = fileInputLabel.width() - 80; // Text "Browse" is 80px.
        var files = [];
        for (var i = 0; i < $(this)[0].files.length; i++) {
            files.push($(this)[0].files[i].name);
        }

        let fileInputFont = GetCanvasFont();
        if (files.length == 0) {
            fileInputLabel.html("Choose Files...");
        } else if (files.length === 1) {
            let prototypeLabelText = files[0];
            let prototypeLabelTextWidth = GetTextWidth(prototypeLabelText, fileInputFont);
            if (prototypeLabelTextWidth >= widthAvailableForText) {
                fileInputLabel.html("1 File Selected...");
            } else {
                fileInputLabel.html(files[0]);
            }
        } else {
            let prototypeLabelText = files.join(', ');
            let prototypeLabelTextWidth = GetTextWidth(prototypeLabelText, fileInputFont);
            if (prototypeLabelTextWidth >= widthAvailableForText) {
                fileInputLabel.html(`${files.length} Files Selected...`);
            } else {
                fileInputLabel.html(prototypeLabelText);
            }
        }
        return;
    });
}

/* Initialises popper tooltips. */
let InitialiseTooltips = () => {
    $('[data-bs-toggle="tooltip"]').tooltip();
}

/* Initialises "Other" field to display when other is selected in dropdown.
 * Also clears field if other is unselected.
 * */
let InitialiseTrialsDropdown = () => {
    // Get the existing Trial Value
    var trialElement = document.getElementById("Trial");

    if (trialElement === null || trialElement === undefined) {
        return;
    }
    // If the Trial Value is Other then show Trial Other Field
    if (trialElement.value == "1000") {
        $("#OtherSpecifyTrial").show();
    }
    else {
        $("#OtherSpecifyTrial").hide();
    }
    $("#Trial").change(function () {
        var value = document.getElementById("Trial").value;
        if (value == "1000") {
            // Clear the Trial Other field on change to set new value
            $("#TrialOther").val('');
            $("#OtherSpecifyTrial").show();
        }
        else {
            $("#OtherSpecifyTrial").hide();
        }
    });
}

function CopyErrorCode() {
    var copyText = document.getElementById("errorCode").innerText;
    var tempInput = document.createElement("input");
    tempInput.value = copyText;
    document.body.appendChild(tempInput);
    tempInput.select();
    document.execCommand("copy");
    document.body.removeChild(tempInput);
}


$(document).ready(function () {
    $('#ytuTMTable').DataTable({
        stateSave: true,
        responsive: {
            details: {
                renderer: function (api, rowIdx, columns) {
                    var data = $.map(columns, function (col, i) {
                        return col.hidden ? getFormattedDataTableChild(col) : '';
                    }).join('');

                    return data ? $('<table/>').append(data) : false;
                },
                type: 'column',
                target: 'tr'
            }
        }
    });
});


//this function formats the child rows of the responsive data tables. shrink a datatable down to mobile size. 
//click on a row and the hidden td (those chopped off when the screen shirnks) will be shown nested below the row.
//if there's no data in the entry it displays it as a -, if there is no th then it displays the data in the middle for niceness
function getFormattedDataTableChild(col) {

    let title = '';
    let data = col.data;

    if (data === "") {
        data = "-";
    }

    if (col.title === "") {
        data = '<td colspan="2" class="text-center">' + data + '</td>'; //if there's no title then display the data in the middle of the two cells
    }
    else {
        title = '<td>' + col.title + ':' + '</td> ';
        data = '<td>' + data + '</td>';
    }

    return '<tr data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' + title + data + '</tr>';
}

function hideContactUsSubmit() {
    // Disable the submit button
    $("#ContactUsSubmit").hide();
    $("#fakeSubmitButton").show();
}