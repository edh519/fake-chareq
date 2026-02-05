$(document).ready(function () {
    $('#user-subscriptions').DataTable({
        stateSave: true
    });
});

document.querySelectorAll(".created-time").forEach(el => {
    const dt = moment(el.dataset.datetime);

    el.title = dt.format("HH:mm D MMM YYYY");
    el.append(dt.fromNow());
});

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("tr.clickable-row").forEach(row => {
        row.addEventListener("click", function (e) {

            if (e.target.closest("a, button, input, select, textarea")) {
                return;
            }

            const url = this.dataset.url;
            if (url) {
                if (e.ctrlKey || e.metaKey || e.button === 1) {
                    window.open(url, "_blank");
                    return;
                }
                window.location.href = url;
            }
        });
    });
});
