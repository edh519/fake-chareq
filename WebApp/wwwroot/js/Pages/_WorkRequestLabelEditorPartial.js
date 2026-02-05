$(document).ready(function () {
    InitialiseTooltips();
    HideShowLabelsPartial(); // Ensure fields are shown/hidden based on content loaded.
});

/* Generates the label Html based on the passed information.
 * Note: Ensure this is kept up to date with `WebApp\Views\Shared\_LabelPartial.cshtml`
*/
let LabelHtml = (name, description, hexColor, workRequestId, labelId) => {
    return "<span " +
        "class='badge rounded-pill m-1 font-size-1rem fw-normal shadow' " +
        `style='background-color:${hexColor}; color:${CalculateBestTextColor(hexColor)}; border: 2px solid ${hexColor}' ` +
        "data-bs-toggle='tooltip' " +
        `title='${description}' ` +
        `data-workRequestId="${workRequestId}" ` +
        `data-labelId="${labelId}"` +
        "> " +
        name +
        " </span>"
}

/* Click event for Add button next to select dropdown.
 * Adds the selected label to the work request then updates the partial following change.
*/
let AddSelectedLabel = async (btn, workRequestId) => {
    // Get the selected value based on the btn clicked, in-case this view is displayed in more than one place.
    let selectValue = $(btn).parent().prev("select").val();

    // If no selected value or default value, do nothing.
    if (!selectValue && selectValue <= 0) {
        return;
    }

    // Call api to add the label to the work request. Await to ensure data change completed before refreshing.
    let response = await PutLabelOnWorkRequestApi(workRequestId, selectValue);

    // Uncomment if special action on failure to remove.
    //if (!response.ok)
    //    return;

    // Refresh the Labels Partial regardless of success or failure to ensure true state is shown.
    _ = await RefreshLabelsPartial(workRequestId);
    return;
}

/* Click event for Remove/Cancel button.
 * Enables/disables the Remove label functionality based on the text on the clicked button.
 */
let ToggleLabelRemoval = (btn) => {

    let clickedBtn = $(btn)
    if (clickedBtn.text() === "Remove") {
        $("span[data-workRequestId]").on("click", (e) => RemoveClickedLabel(e)).addClass("pointerOnHover badge-dimmed-hovered");
        clickedBtn.removeClass("btn-outline-danger").addClass("btn-secondary text-white").text("Cancel");
        $("[name='addLabelsWrapper']").hide();
        $("[name='removeLabelInfo']").show();

    } else { // Cancel remove labels
        $("span[data-workRequestId]").off("click").removeClass("pointerOnHover badge-dimmed-hovered");
        clickedBtn.removeClass("btn-secondary text-white").addClass("btn-outline-danger").text("Remove");
        $("[name='removeLabelInfo']").hide();

        // should select be shown? count options
        let selects = $("select[name='labelsSelect']")
        if (selects.children('option').length > 1)
            $("[name='addLabelsWrapper']").show();
    }
    return;
}

/* Calls api to perform removal of label on a work request.
 * Refreshes the partial view following update.
 */
let RemoveClickedLabel = async (labelClicked) => {
    let label = $(labelClicked.target);

    let workRequestId = label.attr("data-workRequestId");
    let labelId = label.attr("data-labelId");

    // Call api to remove label from work request. Await to ensure data change completed before refreshing.
    let response = await PutRemoveLabelOnWorkRequestApi(workRequestId, labelId);

    // Uncomment if special action on failure to remove.
    //if (!response.ok)
    //    return;

    // Refresh the Labels Partial regardless of success or failure to ensure true state is shown.
    _ = await RefreshLabelsPartial(workRequestId);

}

/* Gets labels from the api for the work request based on whether to include only those available to be added or those already added.
 * Returns null if there are none or an error occurs.
 */
let GetLabelsFromApi = async (workRequestId, includeOnlyAvailableToAdd) => {
    let response = await GetLabelsByWorkRequestIdApi(workRequestId, includeOnlyAvailableToAdd);
    if (!response.ok)
        return null;
    let json = await response.json();
    let labels = json?.Labels;
    if (!labels)
        return null;

    return labels;
}

/* Performs a full refresh of the Labels partial.
 * Renders the labels and the add labels select dropdown options with fresh api data.
 * Controls visibility of elements within partial based on the content and state.
 */
let RefreshLabelsPartial = async (workRequestId) => {

    // renderLabels and renderAddLabelSelects can be run async as they independently call api
    await Promise.all([
        RenderLabels(await GetLabelsFromApi(workRequestId, false)),
        RenderAddLabelSelects(await GetLabelsFromApi(workRequestId, true)),
        reloadPageOnLabelChange()
    ]);

    // hide/show of partial contents is done following renderLabels and renderAddLabelSelects are both completed
    HideShowLabelsPartial();
    return;
}

/* Hide and show elements within the Labels partial based on the content present.
 */
let HideShowLabelsPartial = () => {
    // By default show the add labels select dropdown.
    $("[name='addLabelsWrapper']").show();

    // By default hide the info box for when removing labels. Then update visibility and styling on remove label related functionality.
    $("[name='removeLabelInfo']").hide();
    RenderRemoveLabelBtn();

    // Check if there are no options in the select dropdown, and hide if none.
    let select = $("select[name='labelsSelect']")
    if (!select.children("option[value]").length) {
        $("[name='addLabelsWrapper']").hide();
    }

    // Override visibility on the add and remove functionality if not authoriser. Readonly view essentially.
    let isAuthoriser = $("input[name='isAuthoriser']").first().val();
    if (!isAuthoriser) {
        $("[name='addLabelsWrapper']").hide();
        $("[name='removeLabelsBtn'").hide();
    }
}

/* Using passed label data, generate the label html and enable tooltips.
 * If no labels passed, display message.
 */
let RenderLabels = async (labels) => {

    // Note we want to select all displays of labels when updating.
    let labelWrappers = $("[name='labelsWrapper']");

    // Clear out labels there already, ensuring tooltips are disposed of correctly.
    labelWrappers.children().tooltip("dispose");
    labelWrappers.empty();

    // If no labels, leave a message and finish with that.
    if (!labels || labels.length === 0) {
        labelWrappers.append("<span class='text-muted-theme-toggle font-italic'>There are no labels.</span>");
        return;
    }

    // Loop through labels and add them into the label wrappers using LabelHtml().
    for (var i = 0; i < labels.length; i++) {
        labelWrappers.append(LabelHtml(labels[i].LabelShort, labels[i].LabelDescription, labels[i].HexColor, labels[i].WorkRequestId, labels[i].LabelId));
    }

    // Enable fancy tooltips
    labelWrappers.children().tooltip("enable");
    return;
}

/* Using passed labels data, generate the options within the select dropdowns.
 */
let RenderAddLabelSelects = async (labels) => {
     // Note we want to select all displays of labels when updating.
    let selects = $("select[name='labelsSelect']")

    // Empty out the select and add the default option.
    selects.empty();
    selects.append("<option selected disabled>Select Label</option>");

    // Loop through the labels and add them to the select dropdowns.
    for (var i = 0; i < labels.length; i++) {
        selects.append(`<option value="${labels[i].LabelId}">${labels[i].LabelShort}</option>`)
    }
    return;
}

/* Shows/Hides and styles remove label related functionality based on the labels displayed
 * and whether the remove functionality was enabled already
 */
let RenderRemoveLabelBtn = () => {
    let labels = $("div[name='labelsWrapper']");
    let removeLabelsBtn = $("[name='removeLabelsBtn'");

    // If there are no labels being displayed, hide the remove label button and force reset of Remove/Cancel button.
    if (!labels.children("span[data-workRequestId]").length) {
        removeLabelsBtn.hide();
        removeLabelsBtn.text("Remove");
    } else {
        removeLabelsBtn.show();
    }

    // If text is Remove, removal is NOT occurring.
    // If text is Cancel, removal is occurring.
    if (removeLabelsBtn.text() === "Cancel") {
        $("span[data-workRequestId]").on("click", (e) => RemoveClickedLabel(e)).addClass("pointerOnHover"); // Add click event to labels and styling to indicate clickable.
        $("[name='removeLabelInfo']").show(); // Show remove label info box.
        $("[name='addLabelsWrapper']").hide(); // Override the visibility of add labels select dropdown to be hidden when removal is occurring.
        removeLabelsBtn.removeClass("btn-outline-danger").addClass("btn-secondary text-white"); // Style button with "Cancel" styling.
    } else {
        $("span[data-workRequestId]").off("click").removeClass("pointerOnHover"); // Remove click event from labels and styling to indicate clicable.
        $("[name='removeLabelInfo']").hide(); // Hide remove label info box.
        removeLabelsBtn.removeClass("btn-secondary text-white").addClass("btn-outline-danger"); // Style button with "Remove" styling.
    }
}

let reloadPageOnLabelChange = () => {

    clearTimeout(reloadDelay);
    reloadDelay = setTimeout(function () {
        window.location.reload();
    }, 100)
}