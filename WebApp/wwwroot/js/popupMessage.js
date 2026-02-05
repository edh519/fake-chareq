// Counts number of words in a string.
function countWords(str) {
    return str.trim().split(/\s+/).length;
}

// Variables for setTimeout event and for the milliseconds for the event.
var popupMessageSetTimeout;
var popupMessageTimeoutMilliseconds;

// Run on load, hides messages if empty. Auto-hides success after 1 seconds * number of words + 3 seconds.
function runPopupMessages() {
    var successTextElement = document.getElementById("popupMessageSuccessText");
    if (successTextElement.textContent.trim().length === 0) {
        $("#popupMessageSuccess").alert("close");
    } else {
        // Calculate timeout 1 seconds per word + 3 seconds for "Success" at start.
        popupMessageTimeoutMilliseconds = (countWords(successTextElement.textContent) + 3) * 1000;
        popupMessageSetTimeout = setTimeout(function () { $("#popupMessageSuccess").alert("close"); }, popupMessageTimeoutMilliseconds);
    }

    var errorTextElement = document.getElementById("popupMessageErrorText");
    if (errorTextElement.textContent.trim().length === 0) {
        $("#popupMessageError").alert("close");
    }
}

// Cancel auto-hiding of success popup if hovered.
function popupMessageOnMouseOver() {
    clearTimeout(popupMessageSetTimeout);
}

// Re-enable auto-hiding of success popup when no longer hovered.
function popupMessageOnMouseOut() {
    popupMessageSetTimeout = setTimeout(function () { $("#popupMessageSuccess").alert("close"); }, popupMessageTimeoutMilliseconds);
}

let clientSidePopupMessage = (isSuccessful, message) => {//(bool isSuccessful, string message)
    let popupHTML = `<div id="popupMessageSuccess"
                            class="alert alert-success alert-dismissible" role="alert"
                            onmouseover="popupMessageOnMouseOver()" onmouseout="popupMessageOnMouseOut()">
                        <strong><i class="far fa-smile"></i> Success.</strong>
                        <span id="popupMessageSuccessText">${isSuccessful ? message : ""}</span>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">
                        </button>
                        </div>
                        <div id="popupMessageError"
                            class="alert alert-danger alert-dismissible" role="alert"
                            style="display:flex; justify-content:space-between">
                        <div class="text-start px-0">
                            <strong>
                                <i class="far fa-frown"></i>
                                We're sorry but that didn't work.
                            </strong>
                            <span id="popupMessageErrorText">${isSuccessful ? "" : message}</span>
                        </div>
                        <div class="text-end">
                            Please contact <a href="mailto:ytu-techsupport-group@york.ac.uk">YTU Tech Support</a> if the problem persists.
                        </div>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">
                        </button>
                    </div>`;

    $('.popup-message-div').append(popupHTML);

    runPopupMessages();
}