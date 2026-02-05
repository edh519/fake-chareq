$(document).ready(function () {
    // 2 hours (in milliseconds) to show the logout alert. Configured in Startup.cs ConfigureAuth
    setTimeout(showLogoutAlert, 2 * 60 * 60 * 1000);
});

function showLogoutAlert() {
    Swal.fire({
        title: 'Session Expired',
        text: "Your session has expired due to inactivity. Please save any unsaved work before logging out.",
        icon: 'warning',
        confirmButtonText: 'OK'
    });
}