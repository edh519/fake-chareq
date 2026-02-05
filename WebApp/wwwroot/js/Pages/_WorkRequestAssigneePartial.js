$(document).ready(function () {
    InitialiseTooltips();

    $("[name='removeAssigneeInfo'").hide();

    let select = $("select[name='assigneeSelect']")
    if (!select.children("option[value]").length) {
        $("[name='addAssigneeWrapper']").hide();
    }

    let assignees = $("div[name='assigneesWrapper']");
    if (!assignees.children("span").length) {
        $("[name='toggleAssigneeRemovalBtn'").hide();
        assignees.append("<span class='text-muted-theme-toggle font-italic'>There are no assignees.</span>");
    }

    let isAuthoriser = $("input[name='isAuthoriser']").first().val();
    if (!isAuthoriser) {
        $("[name='addAssigneeWrapper']").hide();
        $("[name='toggleAssigneeRemovalBtn'").hide();
    }


});

let reloadDelay;

/* NOTE: Maintain html along with `WebApp\Views\Shared\_WorkRequestAssigneePartial.cshtml` */
let assigneeHtml = (email, username, workRequestId) => {
    return "<span " +
        "class='badge rounded-pill my-1 font-size-1rem fw-normal assignee-badge-pill text-color-theme-important' " +
        "data-bs-toggle='tooltip' " +
        `title='${email}' ` +
        `data-workRequestId="${workRequestId}" ` +
        `data-userEmail="${email}"` +
        "><i class='fas fa-user-circle pe-1'></i> " +
        TrimStringToMaxLength(RemoveDomainFromEmail(email), 16) +
        " </span>"
}
let addSelectedAssignee = async (btn, workRequestId) => {
    let selectValue = $(btn).parent().prev("select").val();

    if (!selectValue && selectValue <= 0) {
        return;
    }

    let response = await PutAsigneeOnWorkRequestApi(workRequestId, selectValue);
    if (!response.ok)
        return;

    let assignees = await GetAssigneesFromApi(workRequestId)
    await Promise.all([
        renderAssignees(assignees?.AssignedUsers, workRequestId),
        renderAddAssigneeSelects(assignees?.AssignableUsers),
        reloadPageOnAssignChange()
    ])
    showHideAssigneeInputs(assignees);
    return;
}

let toggleAssigneeRemoval = (btn) => {

    let clickedBtn = $(btn)
    if (clickedBtn.text() === "Unassign") {
        $("span[data-userEmail]").on("click", (e) => removeClickedAssignee(e)).addClass("pointerOnHover badge-dimmed-hovered");
        clickedBtn.removeClass("btn-outline-danger").addClass("btn-secondary text-white").text("Cancel");
        $("[name='addAssigneeWrapper']").hide();
        $("[name='removeAssigneeInfo'").show();

    } else { // Cancel remove labels
        $("span[data-userEmail]").off("click").removeClass("pointerOnHover badge-dimmed-hovered");
        clickedBtn.removeClass("btn-secondary text-white").addClass("btn-outline-danger").text("Unassign");
        $("[name='addAssigneeWrapper']").show();
        $("[name='removeAssigneeInfo'").hide();
    }

    let select = $("select[name='assigneeSelect']")
    if (!select.children("option[value]").length) {
        $("[name='addAssigneeWrapper']").hide();
    }
    return;
}

let removeClickedAssignee = async (assigneeClicked) => {
    let assignee = $(assigneeClicked.target);

    let workRequestId = assignee.attr("data-workRequestId");
    let asigneeEmail = assignee.attr("data-userEmail");

    let response = await PutRemoveAssigneeOnWorkRequestApi(workRequestId, asigneeEmail);
    if (!response.ok)
        return;

    let assignees = await GetAssigneesFromApi(workRequestId)
    await Promise.all([
        renderAssignees(assignees?.AssignedUsers, workRequestId),
        renderAddAssigneeSelects(assignees?.AssignableUsers),
        reloadPageOnAssignChange()
    ]);

    let parentBtn = assignee.parent("div.assignees-wrapper").prev("div").children("a.btn");
    if (parentBtn.text() === "Unassign") {
        $("span[data-userEmail]").off("click").removeClass("pointerOnHover");
        $("[name='addAssigneeWrapper']").show();
        $("[name='removeAssigneeInfo'").hide();
    } else { // Cancel user unassigning text
        $("[name='addAssigneeWrapper']").hide();
        $("[name='removeAssigneeInfo'").show();
    }

    showHideAssigneeInputs(assignees);
    return;
}

let reloadPageOnAssignChange = () => {

    clearTimeout(reloadDelay);
    reloadDelay = setTimeout(function () {
        window.location.reload();
    }, 100)
}

let GetAssigneesFromApi = async (workRequestId) => {
    let response = await GetAssigneesByWorkRequestIdApi(workRequestId);
    if (!response.ok)
        return null;
    let assignees = await response.json();
    if (!assignees)
        return null;

    return assignees;
}

let renderAssignees = async (assignees, workRequestId) => {

    // Note we want to select all displays of labels when updating.
    let assigneesWrappers = $("[name='assigneesWrapper']");
    assigneesWrappers.children().tooltip("dispose");
    assigneesWrappers.empty();

    if (!assignees || assignees.length == 0)
        assigneesWrappers.append("<span class='text-muted-theme-toggle font-italic'>There are no assignees.</span>");


    for (var i = 0; i < assignees.length; i++) {
        assigneesWrappers.append(assigneeHtml(assignees[i].Email, assignees[i].Username, workRequestId));
    }

    assigneesWrappers.children().tooltip("enable");

    return;
}

let renderAddAssigneeSelects = async (assignees) => {
    let selects = $("select[name='assigneeSelect']")

    selects.empty();
    selects.append("<option selected disabled>Select Assignee</option>");
    for (var i = 0; i < assignees.length; i++) {
        selects.append(`<option value="${assignees[i].Email}">${assignees[i].Email}</option>`)
    }

    if (assignees.length === 0)
        $("[name='addAssigneeWrapper']").hide();
    else
        $("[name='addAssigneeWrapper']").show();

    return;
}

let showHideAssigneeInputs = (assignees) => {

    $("[name='addAssigneeWrapper']").show();

    if (!assignees?.AssignedUsers || assignees?.AssignedUsers.length == 0) {
        // No users assigned.
        $("[name='toggleAssigneeRemovalBtn']").text("Unassign").addClass("btn-outline-danger").removeClass("btn-secondary text-white").hide();
        $("span[data-userEmail]").off("click").removeClass("pointerOnHover");
        $("[name='removeAssigneeInfo'").hide();
    } else {
        if ($("[name='toggleAssigneeRemovalBtn']").text() === "Unassign") {
            $("span[data-userEmail]").off("click").removeClass("pointerOnHover");
            $("[name='toggleAssigneeRemovalBtn']").addClass("btn-outline-danger").removeClass("btn-secondary text-white").show();
            $("[name='addAssigneeWrapper']").show();
            $("[name='removeAssigneeInfo'").hide();
        } else {
            $("span[data-userEmail]").on("click", (e) => removeClickedAssignee(e)).addClass("pointerOnHover");
            $("[name='toggleAssigneeRemovalBtn']").addClass("btn-secondary text-white").removeClass("btn-outline-danger").show();
            $("[name='addAssigneeWrapper']").hide();
            $("[name='removeAssigneeInfo'").show();
        }
    }

    if (!assignees?.AssignableUsers || assignees?.AssignableUsers.length == 0) {
        $("[name='addAssigneeWrapper']").hide();
    }


}