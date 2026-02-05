

let CallApi = (url, method = "GET") => {
    return fetch(url, { method: method, headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() } });
}

// Work Request Details APIs
let PutProjectIdOnWorkRequestApi = (workRequestId, projectId) => {
    let url = `${GetUrlBase()}/WorkRequest/PutProjectIdOnWorkRequest?workRequestId=${workRequestId}&projectId=${projectId}`;
    return CallApi(url, "PUT");
}

// View All Work Requests APIs
let GetPendingWorkRequestsApi = () => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/PendingWorkRequests`;
    return CallApi(url);
}
let GetFinalisedWorkRequestsApi = () => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/FinalisedWorkRequests`;
    return CallApi(url);
}
let GetWorkRequestsApi = (loadAll = false) => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/WorkRequests`;

    if (loadAll) {
        url += `?loadAll=true`;
    }
    return CallApi(url);
}
let GetAssignedWorkRequestsApi = (isFinalised, groups) => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/AssignedWorkRequests`;
    let groupsString = ConstructGroupsQueryString(groups);
    if (isFinalised !== null || groupsString !== null)
        url += "?";
    if (isFinalised !== null)
        url += `isFinalised=${isFinalised}`;
    if (groupsString !== null) {
        if (isFinalised !== null)
            url += "&";
        url += `group=${groupsString}`;
    }
    return CallApi(url);
}
let GetUnassignedWorkRequestsApi = (isFinalised) => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/UnassignedWorkRequests`;
    if (isFinalised !== null)
        url += `?isFinalised=${isFinalised}`;
    return CallApi(url);
}
let GetParticipatingWorkRequestsApi = (isFinalised) => {
    let url = `${GetUrlBase()}/ViewAllWorkRequests/ParticipatingWorkRequests`;
    if (isFinalised !== null)
        url += `?isFinalised=${isFinalised}`;
    return CallApi(url);
}
let ConstructGroupsQueryString = (groups) => {
    if (!groups)
        return null;

    let groupsValues = [];
    for (var i = 0; i < groups.length; i++) {
        groupsValues.push(encodeURI(groups[i]));
    }
    groupsValues.sort();
    return groupsValues.join('&group=');
}

let GetSubTasksApi = (loadAll = false) => {
    let url = `${GetUrlBase()}/ViewAllSubTasks/SubTasks`;

    if (loadAll) {
        url += `?loadAll=true`;
    }
    return CallApi(url);
}

let PutAssigneeEmailOnSubTaskApi = (subTaskId, assigneeEmail) => {
    let url = `${GetUrlBase()}/WorkRequest/PutAssigneeEmailOnSubTask?subTaskId=${subTaskId}&assigneeEmail=${assigneeEmail}`;
    return CallApi(url, "PUT");
}



// Files APIs
let GetSupportingFilesApi = (workRequestId) => {
    let url = `${GetUrlBase()}/WorkRequest/FileUploads?workRequestId=${workRequestId}`;
    return CallApi(url);
}
let GetSupportingFileDownloadApi = (fileId) => {
    let url = `${GetUrlBase()}/WorkRequest/DownloadSupportingFile?fileId=${fileId}`;
    return CallApi(url);
}

let DeleteSupportingFileApi  = (fileId) => {
    let url = `${GetUrlBase()}/WorkRequest/DeleteSupportingFile?fileId=${fileId}`;
    return CallApi(url, "DELETE");
}

// Labels APIs
let PutLabelArchiveStateApi = async (labelId, archiveState) => {
    let url = `${GetUrlBase()}/Admin/ArchiveLabel?labelId=${labelId}&archiveState=${archiveState}`;
    return CallApi(url, "PUT");
}
let GetLabelsByWorkRequestIdApi = async (workRequestId, includeOnlyAvailableToAdd = false) => {
    let url = `${GetUrlBase()}/WorkRequest/GetLabelsOnWorkRequest?workRequestId=${workRequestId}&includeOnlyAvailableToAdd=${includeOnlyAvailableToAdd}`;
    return CallApi(url);
}
let GetLabelsApi = async (includeArchived = false) => {
    let url = `${GetUrlBase()}/WorkRequest/GetLabels?includeArchived=${includeArchived}`;
    return CallApi(url);
}
let PutLabelOnWorkRequestApi = async (workRequestId, labelId) => {
    let url = `${GetUrlBase()}/WorkRequest/PutLabelOnWorkRequest?workRequestId=${workRequestId}&labelId=${labelId}`;
    return CallApi(url, "PUT");
}
let PutRemoveLabelOnWorkRequestApi = async (workRequestId, labelId) => {
    let url = `${GetUrlBase()}/WorkRequest/PutRemoveLabelOnWorkRequestApi?workRequestId=${workRequestId}&labelId=${labelId}`;
    return CallApi(url, "PUT");
}

// Assignees APIs
let GetAssigneesByWorkRequestIdApi = async (workRequestId) => {
    let url = `${GetUrlBase()}/WorkRequest/GetAssigneesOnWorkRequest?workRequestId=${workRequestId}`;
    return CallApi(url);
}
let PutAsigneeOnWorkRequestApi = async (workRequestId, assigneeEmail) => {
    let url = `${GetUrlBase()}/WorkRequest/PutAssigneeOnWorkRequest?workRequestId=${workRequestId}&assigneeEmail=${encodeURIComponent(assigneeEmail)}`;
    return CallApi(url, "PUT");
}
let PutRemoveAssigneeOnWorkRequestApi = async (workRequestId, assigneeEmail) => {
    let url = `${GetUrlBase()}/WorkRequest/PutRemoveAssigneeOnWorkRequest?workRequestId=${workRequestId}&assigneeEmail=${encodeURIComponent(assigneeEmail)}`;
    return CallApi(url, "PUT");
}

// Trials APIs
let PutTrialArchiveStateApi = async (trialId, archiveState) => {
    let url = `${GetUrlBase()}/Admin/ArchiveTrial?trialId=${trialId}&archiveState=${archiveState}`;
    return CallApi(url, "PUT");
}


// GitHub APIs
let PutUpdateGithubIssueOnWorkRequestApi = async (workRequestId,repositoryId, githubIssueNumberOrUrl) => {
    let url = `${GetUrlBase()}/WorkRequest/UpdateGitHubIssueOnRequest?workRequestId=${workRequestId}&repositoryId=${repositoryId}&githubIssueNumberOrUrl=${githubIssueNumberOrUrl}`;
    return CallApi(url, "PUT");
}