$(document).ready(function () {
    $('#contactUsTable').DataTable({
        responsive: true,
        order: [[3, "asc"],[4, "asc"]], // shows the oldest, unactioned submissions at the top
        columnDefs: [
            {
                // id
                targets: 0,
                visible: false
            },
            {
                // email 
                targets: 1,
                orderable: false
            },
            {
                // message 
                targets: 2,
                orderable: false
            },
            {
                // actioned
                targets: 3,
                orderable: true
            },
            {
                // submission date
                targets: 4,
                orderable: true
            },
            {
                // details
                targets: 5,
                orderable: false
            }
        ]
    });
})