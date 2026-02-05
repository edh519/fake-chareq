$(document).ready(function () {
    $("#spinner.github-btn").hide();
});

function CreateIssueForWorkRequest(btn) {
    $("#spinner.github-btn").show();
    $(btn).hide();

    var token = $("input[name='__RequestVerificationToken']").val();

    var dataRequestUrl = $(btn).data('request-url');

    var workRequestId = $(btn).data('workrequestid');

    var repositoryId = $(btn).data('repositoryid');

    if (repositoryId == null) {
        // get the selected repository in the dropdown id = trialGitHubInfoSelect get the selected entry
        repositoryId = $("#trialGitHubInfoSelect").val();

    }

    var data = { workRequestId: workRequestId, repositoryId: repositoryId };

    $.ajax({
        url: dataRequestUrl,
        type: 'POST',
        headers:
        {
            "RequestVerificationToken": token
        },
        data: data,
        success: function (response) {
            $("#spinner.github-btn").hide();
            window.open(response, "_blank");
            location.reload();
        },
        error: function () {
            location.reload();
        }
    });
}

async function UpdateGithubIssue(workRequestId, repositoryId, githubIssueNumberOrUrl) {

    if (repositoryId == null) {
        // get the selected repository in the dropdown id = trialGitHubInfoSelect get the selected entry
        repositoryId = $("#trialGitHubInfoSelect").val();
    }

    let result = await PutUpdateGithubIssueOnWorkRequestApi(workRequestId, repositoryId, githubIssueNumberOrUrl);
    if (!result.ok) {
        clientSidePopupMessage(false, `Could not update linked GitHub issue. Try refreshing this page.`)
        return;
    }

    document.location.reload(true);
    return;
}

function ResetGitHubInfo(btn) {
    let token = $("input[name='__RequestVerificationToken']").val();
    let dataRequestUrl = $(btn).data('request-url');
    let workRequestId = $(btn).data('workrequestid');
    $.ajax({
        url: dataRequestUrl,
        type: 'POST',
        headers:
        {
            "RequestVerificationToken": token
        },
        data: { workRequestId: workRequestId },
        success: function (result) {
            location.reload();
        },
        error: function (error) {
            location.reload();
        }
    });
}