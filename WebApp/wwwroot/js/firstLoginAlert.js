$(document).ready(function () {
    showFirstTimeAlert();
});

function showFirstTimeAlert() {
    Swal.fire({
        title: 'First Visit',
        html: "Before using ChaReq for the first time, we recommend reading our <a href=\"https://uoy.atlassian.net/wiki/spaces/YTUGuides/pages/24773343/ChaReq+-+Change+Request+Manager\" rel=\"noopener\" target=\"_blank\">ChaReq Guide</a>.",
        icon: 'info',
        confirmButtonText: 'OK'
    });
}
