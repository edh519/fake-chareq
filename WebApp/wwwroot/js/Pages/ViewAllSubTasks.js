/* For table initialisation we are using jQuery datatables. https://datatables.net/ 
 */
$(document).ready(function () {

    LoadTableAndData();

    $('[data-bs-toggle="tooltip"]').tooltip();

    // Status Option Filter
    $('#statusFilter input[name="statusOption"]').on('change', function () {
        var selectedStatuses = $('#statusFilter input[name="statusOption"]:checked').map(function () {
            return $(this).val();
        }).get();

        updateFilter('statusOption', selectedStatuses);

        applyStatusFilter(selectedStatuses);
    });

    // Trial Dropdown Filter
    $('#trialDropdown').on('change', function () {
        var selectedTrials = $(this).val();
        updateFilter('trial', selectedTrials);
        applyTrialFilter(selectedTrials);
    });

    // Requester Filter
    $('#requesterFilter input[name="requesterOption"]').on('change', function () {
        var selectedRequesters = $('#requesterFilter input[name="requesterOption"]:checked').map(function () {
            return $(this).val();
        }).get();
        updateFilter('requesterOption', selectedRequesters);
        applyRequesterFilter(selectedRequesters);
    });

    $('#requesterSearch').on('keyup', function () {
        var searchTerm = $(this).val();

        // Get selected requesters (can be multiple if both "Me" and "Search Requester" are selected)
        var selectedRequesters = $('#requesterFilter input[name="requesterOption"]:checked').map(function () {
            return $(this).val();
        }).get();
        updateFilter('requesterSearch', searchTerm);
        applyRequesterFilter(selectedRequesters);
    });

    // Assignees Filter
    $('#assigneesFilter input[name="assigneesOption"]').on('change', function () {
        var selectedAssignee = $('#assigneesFilter input[name="assigneesOption"]:checked').map(function () {
            return $(this).val();
        }).get();
        updateFilter('assigneesOption', selectedAssignee);
        applyAssigneesFilter(selectedAssignee);
    });

    $('#assigneesSearch').on('keyup', function () {
        var searchTerm = $(this).val();

        // Get selected assignees (can be multiple)
        var selectedAssignee = $('#assigneesFilter input[name="assigneesOption"]:checked').map(function () {
            return $(this).val();
        }).get();
        updateFilter('assigneesSearch', searchTerm);
        applyAssigneesFilter(selectedAssignee);
    });

    // Reset Filters
    $('#resetFilters').on('click', function () {
        resetFilters();
    });

    // Collapse All Button
    $('#collapseAll').on('click', function () {
        collapseAll();
    });

    $('#searchCheckboxRequester').on('change', function () {
        if (!$(this).prop('checked')) {
            clearRequesterSearch();
            updateFilter('requesterSearch', "")
        }
    });

    $('#searchCheckboxAssignees').on('change', function () {
        if (!$(this).prop('checked')) {
            clearAssigneesSearch();
            updateFilter('assigneesSearch', "")
        }
    });

    initialiseDatePickers();

    PopulateHistoric();
});

// Helper Functions

function updateFilter(key, value) {
    if (value === "all" || value === "") {
        sessionStorage.removeItem("vawr_" + key);
        removeUrlParam(key);
    } else {
        sessionStorage.setItem("vawr_" + key, value);
        updateUrlParam(key, encodeURIComponent(value));
    }
}

function updateUrlParam(key, value) {
    let urlParams = new URLSearchParams(window.location.search);
    urlParams.set(key, value);
    history.replaceState({}, '', `${location.pathname}?${urlParams.toString()}${location.hash}`);
}

function removeUrlParam(key) {
    // Remove from URL
    let urlParams = new URLSearchParams(window.location.search);
    urlParams.delete(key);
    history.replaceState({}, '', `${location.pathname}?${urlParams.toString()}${location.hash}`);

    // Remove from sessionStorage
    sessionStorage.removeItem("vawr_" + key);
}


function getFilterValue(key) {
    let urlParams = new URLSearchParams(window.location.search);
    let urlValue = urlParams.get(key);

    // If URL value is found
    if (urlValue) {
        // Decode and split by commas if the value is a comma-separated string
        return decodeURIComponent(urlValue).split(',').map(item => item.trim());
    }

    // If URL value not found, retrieve from sessionStorage
    let storedValue = sessionStorage.getItem("vawr_" + key);

    // If a value is found in sessionStorage, process it similarly
    if (storedValue) {
        return storedValue.split(',').map(item => item.trim());
    }

    // If no value found, return an empty array
    return [];
}


function initialiseDatePickers() {
    function cb(start, end) {
        $(this.element.find('span')).html(start ? start.format('D MMMM,YYYY') + ' - ' + end.format('D MMMM,YYYY') : '');
    }

    const datePickerOptions = {
        autoUpdateInput: false,
        locale: {
            format: 'DD/MM/YYYY',
        },
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    };

    // Initialise Created Date Picker
    $('#createdDatePicker').daterangepicker(datePickerOptions, cb);

    $('#createdDatePicker').on('apply.daterangepicker', function (ev, picker) {
        const createdDateValue = picker.startDate.format('D MMMM,YYYY') + ' - ' + picker.endDate.format('D MMMM,YYYY');
        $('#createdDatePicker span').html(createdDateValue);
        updateFilter('createdDate', createdDateValue);
        applyDateFilters(createdDateValue, $('#finalisedDatePicker span').text());
    });

    $('#createdDatePicker').on('cancel.daterangepicker', function (ev, picker) {
        $('#createdDatePicker span').html('');
        $('#createdDatePicker').val('');
        applyDateFilters('', $('#finalisedDatePicker span').text());
    });

    // Initialise Finalised Date Picker
    $('#finalisedDatePicker').daterangepicker(datePickerOptions, cb);

    $('#finalisedDatePicker').on('apply.daterangepicker', function (ev, picker) {
        const finalisedDateValue = picker.startDate.format('D MMMM,YYYY') + ' - ' + picker.endDate.format('D MMMM,YYYY');
        $('#finalisedDatePicker span').html(finalisedDateValue);
        updateFilter('finalisedDate', finalisedDateValue);
        applyDateFilters($('#createdDatePicker span').text(), finalisedDateValue);
    });

    $('#finalisedDatePicker').on('cancel.daterangepicker', function (ev, picker) {
        $('#finalisedDatePicker span').html('');
        $('#finalisedDatePicker').val('');
        applyDateFilters($('#createdDatePicker span').text(), '');
    });
}

function applyStatusFilter(selectedStatuses) {
    const statusFilterCard = $('#statusFilter').closest('.card').find('.card-header');

    if (selectedStatuses.length === 0) {
        viewAllSubTasksTable.column(10).search('').draw();
        removeUrlParam('statusOption');
        statusFilterCard.removeClass('filter-active');
    } else {
        viewAllSubTasksTable.column(10).search(selectedStatuses.join('|'), true, false).draw();
        statusFilterCard.addClass('filter-active');
    }
}


function applyTrialFilter(selectedTrials) {
    const trialFilterHeader = $('#trialFilter').closest('.card').find('.card-header');

    if (!selectedTrials || selectedTrials.length === 0) {
        viewAllSubTasksTable.column(0).search('').draw();
        removeUrlParam('trial');
        trialFilterHeader.removeClass('filter-active');
    } else {
        const cleanedTrials = selectedTrials.map(trial => trial.replace('(archived)', '').trim());
        const searchQuery = cleanedTrials
            .map(trial => `(^|;)${escapeRegex(trial)}(;|$)`)
            .join('|');
        viewAllSubTasksTable.column(0).search(searchQuery, true, false).draw();
        trialFilterHeader.addClass('filter-active');
    }
}

function applyRequesterFilter(selectedRequesters) {
    const requesterFilterHeader = $('#requesterFilter').closest('.card').find('.card-header');
    $('#requesterSearchContainer').toggle(selectedRequesters.includes('search'));

    if (selectedRequesters.includes('none')) {
        selectedRequesters.push('^$');
    }

    if (selectedRequesters.includes('search')) {
        const searchValue = $('#requesterSearch').val();

        if (searchValue !== '') {
            selectedRequesters.push(searchValue);
        } else {
            removeUrlParam('requesterSearch');
            if (selectedRequesters.length === 1) {
                viewAllSubTasksTable.column(5).search('').draw();
                requesterFilterHeader.removeClass('filter-active');
            }
            return;
        }
    }

    if (selectedRequesters.length === 0) {
        viewAllSubTasksTable.column(5).search('').draw();
        removeUrlParam('requesterOption');
        removeUrlParam('requesterSearch');
        requesterFilterHeader.removeClass('filter-active');
    } else {
        viewAllSubTasksTable.column(5).search(selectedRequesters.join('|'), true, false).draw();
        requesterFilterHeader.addClass('filter-active');
    }
}

function applyAssigneesFilter(selectedAssignee) {
    const assigneesFilterHeader = $('#assigneesFilter').closest('.card').find('.card-header');
    $('#AssigneeSearchContainer').toggle(selectedAssignee.includes('search'));

    if (selectedAssignee.includes('none')) {
        selectedAssignee.push('^$');
    }

    if (selectedAssignee.includes('search')) {
        const searchValue = $('#assigneesSearch').val();

        if (searchValue !== '') {
            selectedAssignee.push(searchValue);
        } else {
            removeUrlParam('assigneesSearch');
            if (selectedAssignee.length === 1) {
                viewAllSubTasksTable.column(4).search('').draw();
                assigneesFilterHeader.removeClass('filter-active');
            }
            return;
        }
    }

    if (selectedAssignee.length === 0) {
        viewAllSubTasksTable.column(4).search('').draw();
        removeUrlParam('assigneesOption');
        removeUrlParam('assigneesSearch');
        assigneesFilterHeader.removeClass('filter-active');
    } else {
        viewAllSubTasksTable.column(4).search(selectedAssignee.join('|'), true, false).draw();
        assigneesFilterHeader.addClass('filter-active');
    }
}

function applyDateFilters(createdDates, finalisedDates) {
    const createdDateHeader = $('#createdDateFilter').closest('.card').find('.card-header');
    const finalisedDateHeader = $('#finalisedDateFilter').closest('.card').find('.card-header');

    // Clear existing search filters
    $.fn.dataTable.ext.search = [];

    // Filter function to be pushed
    const filterFunction = function (settings, data, dataIndex) {
        let createdDateMatch = true;
        let finalisedDateMatch = true;

        // Created Date Filter Logic
        if (createdDates && createdDates !== '') {
            const createdDatesArr = createdDates.split(' - ');
            const minCreatedDate = moment(createdDatesArr[0], 'MMMM D,YYYY');
            const maxCreatedDate = moment(createdDatesArr[1], 'MMMM D,YYYY').endOf('day');
            const columnCreatedDate = moment(data[6], 'YYYY-MM-DDTHH:mm:ss.SSSSSSS');

            createdDateMatch = (
                (minCreatedDate === null && maxCreatedDate === null) ||
                (minCreatedDate === null && columnCreatedDate.isSameOrBefore(maxCreatedDate)) ||
                (minCreatedDate.isSameOrBefore(columnCreatedDate) && maxCreatedDate === null) ||
                (minCreatedDate.isSameOrBefore(columnCreatedDate) && columnCreatedDate.isSameOrBefore(maxCreatedDate))
            );
        }

        // Finalised Date Filter Logic
        if (finalisedDates && finalisedDates !== '') {
            const finalisedDatesArr = finalisedDates.split(' - ');
            const minFinalisedDate = moment(finalisedDatesArr[0], 'MMMM D,YYYY');
            const maxFinalisedDate = moment(finalisedDatesArr[1], 'MMMM D,YYYY').endOf('day');
            const columnFinalisedDate = moment(data[8], 'YYYY-MM-DDTHH:mm:ss.SSSSSSS');

            finalisedDateMatch = (
                (minFinalisedDate === null && maxFinalisedDate === null) ||
                (minFinalisedDate === null && columnFinalisedDate.isSameOrBefore(maxFinalisedDate)) ||
                (minFinalisedDate.isSameOrBefore(columnFinalisedDate) && maxFinalisedDate === null) ||
                (minFinalisedDate.isSameOrBefore(columnFinalisedDate) && columnFinalisedDate.isSameOrBefore(maxFinalisedDate))
            );
        }

        return createdDateMatch && finalisedDateMatch;
    };

    // Push the filter function
    $.fn.dataTable.ext.search.push(filterFunction);

    // Clear column searches if date inputs are empty
    if (!createdDates || createdDates === '') {
        viewAllSubTasksTable.column(6).search('').draw();
        removeUrlParam('createdDate');
        createdDateHeader.removeClass('filter-active');
    } else {
        createdDateHeader.addClass('filter-active');
    }

    if (!finalisedDates || finalisedDates === '') {
        viewAllSubTasksTable.column(8).search('').draw();
        removeUrlParam('finalisedDate');
        finalisedDateHeader.removeClass('filter-active');
    } else {
        finalisedDateHeader.addClass('filter-active');
    }

    // Redraw the table to apply filters
    viewAllSubTasksTable.draw();
}
function collapseAll() {
    $('.collapse').collapse('hide');
};

function resetFilters() {
    const wasHistoric = localStorage.getItem("loadAll") === "true";

    clearStatusFilter();
    clearTrialFilter();
    clearRequesterFilter();
    clearAssigneesFilter();
    $('#requesterSearch').val('');
    $('#assigneesSearch').val('');
    clearDateFilter('createdDate');
    clearDateFilter('finalisedDate');
    localStorage.removeItem("loadAll");
    $('#historic-checkbox').prop('checked', false);
    $('#headingHistoric').removeClass('filter-active');
    viewAllSubTasksTable.search('').columns().search('').draw();
    collapseAll();
    storage.removeItem("vawr_orderBy");
    let url = location.pathname;
    history.replaceState({}, '', url);

    if (wasHistoric) {
        if ($.fn.DataTable.isDataTable('#subTaskTable')) {
            $('#subTaskTable').DataTable().destroy();
        }
        $('#subTaskTable').empty();
        LoadTableAndData();
    }
}

function clearStatusFilter() {
    $("input[name='statusOption']").prop('checked', false);
    removeUrlParam('statusOption');
    applyStatusFilter('');
}

function clearTrialFilter() {
    $('#trialDropdown').val([]);
    removeUrlParam('trial');
    applyTrialFilter('');
}
function clearRequesterFilter() {
    $("input[name='requesterOption']").prop('checked', false);
    removeUrlParam('requesterOption');
    clearRequesterSearch();
    applyRequesterFilter('');
}

function clearRequesterSearch() {
    $('#requesterSearch').val('');
    removeUrlParam('requesterSearch');
    $('#requesterSearchContainer').hide();
}

function clearAssigneesFilter() {
    $("input[name='assigneesOption']").prop('checked', false);
    removeUrlParam('assigneesOption');
    clearAssigneesSearch();
    applyAssigneesFilter('');
}

function clearAssigneesSearch() {
    $('#assigneesSearch').val('');
    removeUrlParam('assigneesSearch');
    $('#assigneesSearchContainer').hide();
}

function clearDateFilter(filterType) {
    if (filterType === 'createdDate') {
        $('#createdDatePicker').data('daterangepicker').setStartDate(moment().startOf('day'));
        $('#createdDatePicker').data('daterangepicker').setEndDate(moment().endOf('day'));
        $('#createdDatePicker span').html('');
        $("#createdDatePicker").val('');
        removeUrlParam('createdDate');
    } else if (filterType === 'finalisedDate') {
        $('#finalisedDatePicker').data('daterangepicker').setStartDate(moment().startOf('day'));
        $('#finalisedDatePicker').data('daterangepicker').setEndDate(moment().endOf('day'));
        $('#finalisedDatePicker span').html('');
        $("#finalisedDatePicker").val('');
        removeUrlParam('finalisedDate');
    }

    // Reapply filters (to remove the cleared filter)
    applyDateFilters($('#createdDatePicker span').text(), $('#finalisedDatePicker span').text());
}

/* For checkbox-dropdown hybrid functionality. 
 * Prevents click events within dropdown from dismissing dropdown.
 */
$(document).on('click', '.allow-focus', function (e) {
    e.stopPropagation();
});


/* Holds the object for the datatable and initial state of checkbox-menu (calc in datatable init)
 * */
let viewAllSubTasksTable;
let storage = window.sessionStorage; // Can be changed to localStorage if more persistence required.


/* Determines whether the Finalised columns will be displayed in the datatable.
 * There's no point displaying an always empty column on pending requests.
 */
let displayFinalisedColumns = true;


/* Calls the api using apiCalls.js, then displays the data based on
 * the response.
 */
let LoadTableAndData = async () => {
    let response;

    // NB: url parameters take precedence to ensure predictable navigation and bookmarking
    let urlHash = window.location.hash;
    let orderBy = ""
    let orderDirection = "desc";
    let search = "";
    if (urlHash) {
        let hashComponents = urlHash.split('#')[1].split('&');
        for (var i = 0; i < hashComponents.length; i++) {
            if (hashComponents[i].includes("orderBy=")) {
                orderBy = hashComponents[i].split('=')[1];
            } else if (hashComponents[i].includes("orderDirection=")) {
                orderDirection = hashComponents[i].split('=')[1];
            } else if (hashComponents[i].includes("search=")) {
                search = decodeURIComponent(hashComponents[i].split('=')[1]);
            } else {
                // do nothing
            }
        }
    }
    if (orderBy.length == 0)
        orderBy = storage.getItem("vawr_orderBy") ?? "created_date";
    if (orderDirection === "desc") //nb: default value
        orderDirection = storage.getItem("vawr_orderDirection") ?? "desc";
    if (search.length == 0) {
        search = storage.getItem("vawr_search");
        search = search ? decodeURIComponent(search) : null;
    }

    if (localStorage.getItem("loadAll") == "true") {
        //Get all requests from all time
        response = await GetSubTasksApi(true);
    } else {
        //By default just get requests from within 1 year
        response = await GetSubTasksApi();
    }

    if (!response.ok) // If not okay, want to display empty table.
        return await DisplayData({ "SubTasks": [] }, orderBy, orderDirection, search);

    let jsonResponse = await response.json();
    await DisplayData(jsonResponse, orderBy, orderDirection, search);
    return;
}


/* Displays the retrieved data onto the page.
 * Renders a datatable instance and hides the loading spinner.
 */
let DisplayData = async (data, orderBy, orderDirection, search) => {
    viewAllSubTasksTable = $('#subTaskTable').DataTable({
        saveState: true,
        "search": {
            "search": search ?? ""
        },
        responsive: true,
        "oLanguage": {
            "sEmptyTable": "No requests found."
        },
        "scrollX": true, // If view-space is too narrow, adds a scrollbar.
        data: data.SubTasks,
        columns: [
            // NB: Columns defined with
            // - title: Display name,
            // - data: Variable name in 'data' object (optional, if title === data),
            // - render: Special rendering rules. (optional)
            {
                title: "Trial",
                data: "Trial",
                name: "trial"
            },
            {
                title: "Sub Task",
                data: "SubTaskTitle",
                name: "sub_task_title",
                render: function (data, type, row, meta) {
                    return `<span class="accessible-blue-text">${data}</span>`;
                }
            },
            {
                title: "Status",
                data: "Progress",
                name: "status",
                render: function (data, type, row, meta) {
                    let outputHtml = row.Status;
                    let titleDisplay = row.Status;
                    let progressClass = "bg-primary";
                    switch (row.Status) {
                        case "Abandoned":
                            outputHtml = "<div class='btn rounded-pill bg-danger text-white py-0'>Closed</div>";
                            progressClass = "bg-danger progress-bar-striped";
                            titleDisplay = "Closed";
                            break;
                        case "Rejected":
                            outputHtml = "<div class='btn rounded-pill bg-danger text-black py-0'>Rejected</div>";
                            progressClass = "bg-danger";
                            break;
                        case "Approved":
                            outputHtml = "<div class='btn rounded-pill bg-success text-white py-0'>Approved</span></div>";
                            progressClass = "bg-success";
                            break;
                        case "Open":
                            outputHtml = "<div class='btn rounded-pill bg-primary text-white py-0'>Open</div>";
                            progressClass = "bg-primary";
                            break;
                        default:
                            outputHtml = data;
                            break;
                    }

                    if (data === null) return "";
                    return type === 'display' ?
                        // Assumes the progress is a percentage.
                        // Possible outcomes:
                        // Negative => striped red danger full bar
                        // Positive => blue primary partial bar
                        // NB: Math.abs(data) to ensure percentage progress displayed is positive.
                        `<div class="progress my-1" title='${titleDisplay}'><div class="progress-bar ${progressClass}" style="width:${Math.abs(data)}%"></div></div>` :
                        data;
                }
            },
            {
                title: "Hidden Searchable Assignee",
                data: "Assignee",
                visible: false,
                render: function (data, type, row, meta) {
                    if (data === null) return "";
                    else {
                        let outputHtml = "";
                        for (var i = 0; i < data.length; i++) {
                            outputHtml += data[i].Email + " ";
                        }
                        return outputHtml;
                    }
                }
            },
            {
                title: "Assignee",
                data: "Assignee",
                name: "assignee",
                render: function (data, type, row, meta) {
                    if (data === null) return "";
                    if (type !== "display") return data;
                    else {
                        return "<span class='badge rounded-pill mx-1 mb-1 font-size-1rem fw-normal assignee-badge-pill text-color-theme' " +
                                `data-bs-toggle='tooltip' title='${data}@york.ac.uk'>` +
                                `<i class="fas fa-user-circle pe-1 "></i> ${data}` +
                                "</span>";
                    }
                }
            },
            {
                title: "Requester",
                data: "Requester",
                name: "requester"
            },
            {
                title: "Created Date",
                data: "CreatedDateTime",
                name: "created_date",
                render: function (data, type, row, meta) {
                    if (data === null) return "";
                    return type === 'display' ?
                        // NB: Hidden span added before to allow for the column sort to order correctly by datetime. 
                        // There's a hidden json date string before the formatted date string.
                        `<span title='${moment(data).format('HH:mm D MMM YYYY')}'><span style="display:none;">${data}</span>${moment(data).fromNow()}</span>` :
                        data;
                }
            },
            {
                title: "Created Date - formatted",
                data: "CreatedDateTime",
                name: "created_date_formatted",
                visible: false
            },
            {
                title: "Finalised Date",
                data: "CompletedDateTime",
                name: "finalised_date",
                visible: displayFinalisedColumns,
                render: function (data, type, row, meta) {
                    if (data === null) return "<span class='text-muted-theme-toggle'>N/A</span>";
                    return type === 'display' ?
                        // NB: Hidden span added before to allow for the column sort to order correctly by datetime. 
                        // There's a hidden json date string before the formatted date string.
                        `<span title='${moment(data).format('HH:mm D MMM YYYY')}'><span style="display:none;">${data}</span>${moment(data).fromNow()}</span>` :
                        data;
                }
            },
            {
                title: "Finalised Date - formatted",
                data: "CompletedDateTime",
                name: "finalised_date_formatted",
                visible: false
            },
            {
                title: "Hidden Searchable Status",
                data: "Status",
                visible: false,
                render: function (data, type, row, meta) {
                    if (data === null) return "";
                    return data;
                }
            },
            {
                title: "Hidden Work Request Id",
                data: "WorkRequestId",
                visible: false,
                render: function (data, type, row, meta) {
                    if (data === null) return "";
                    return data;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex, cells) {
            // Make row clickable and style to accomodate. The whole row is clickable, but only the first column has any styling changes from default.
            // All td in row have anchor tag with href. Could not use js onClick as we want right click functionality.
            //
            // Select row, find all td within and set padding 0;
            // wrap content of all td with anchor tag;
            // select second column (.eq(1)) anchor tag and allow default hyperlink styling.
            let url = `${GetUrlBase()}/WorkRequest/WorkRequestDetails?workRequestId=${data.WorkRequestId}`;
            $(row).css("cursor", "pointer").on("click", function (e) {
                if (e.ctrlKey || e.metaKey || e.button === 1) {
                    window.open(url, "_blank");
                } else {
                    window.location.href = url;
                }
            });
        },
        "initComplete": function () {
            // Hide the loading indicating spinner when finished.
            $("#spinner").hide();
        }
    });

    $("table thead tr").on('click', 'th', async function () {
        // HACK: Delay to ensure sort has occurred before checking values. Otherwise getting current rather than new values.
        setTimeout(() => {
            orderedColumnDetails = viewAllSubTasksTable.context[0].aaSorting[0];
            let orderedColumnName = viewAllSubTasksTable.settings().init().columns[orderedColumnDetails[0]].name;

            if (orderedColumnDetails[1].length == 0) {
                // no sorting, clear values saved
                storage.removeItem("vawr_orderBy");
                storage.removeItem("vawr_orderDirection");
                location.hash = `${decodeURIComponent(storage.getItem("vawr_search")) === null ? "" : "#search=" + decodeURIComponent(storage.getItem("vawr_search"))}`;
            } else {
                // save to session storage and to url hash
                storage.setItem("vawr_orderBy", orderedColumnName);
                storage.setItem("vawr_orderDirection", orderedColumnDetails[1]);
                location.hash = `#orderBy=${orderedColumnName}&orderDirection=${orderedColumnDetails[1]}${decodeURIComponent(storage.getItem("vawr_search")) === null ? "" : "&search=" + decodeURIComponent(storage.getItem("vawr_search"))}`;
            }
        }, 500);
    });

    // Listen for the search event
    viewAllSubTasksTable.on('search.dt', function () {
        var searchTerm = viewAllSubTasksTable.search();
        storage.setItem("vawr_search", searchTerm);

        let orderBy = storage.getItem("vawr_orderBy");
        let orderDirection = storage.getItem("vawr_orderDirection");

        let hash = "";
        if (orderBy && orderDirection) {
            hash = `#orderBy=${orderBy}&orderDirection=${orderDirection}`;
        }

        if (searchTerm.trim()) {
            hash += (hash ? "&" : "#") + "search=" + encodeURIComponent(searchTerm);
        }

        location.hash = hash;
    });

    let orderByIndex = viewAllSubTasksTable.settings().init().columns.findIndex(x => x.name === orderBy?.toLowerCase());
    if (orderByIndex === -1) {
        // Don't order if column can't be found. Ignore.
        storage.removeItem("vawr_orderBy");
        storage.removeItem("vawr_orderDirection");
        location.hash = `${storage.getItem("vawr_search") === null ? "" : "#search=" + storage.getItem("vawr_search")}`;
        return;
    }

    orderDirection = orderDirection?.toLowerCase();
    if (orderDirection !== "asc") {
        orderDirection = "desc";
    }

    // Order by defined params, redraw to show result.
    viewAllSubTasksTable.order([orderByIndex, orderDirection]).search(search ?? "").draw();

    PopulateFiltersUponRefresh();

    return;
}

let PopulateFiltersUponRefresh = async () => {

    // Status Filter
    var storedStatuses = getFilterValue('statusOption') || [];
    $('#statusFilter input[name="statusOption"]').each(function () {
        $(this).prop('checked', storedStatuses.includes($(this).val()));
    });
    updateFilter('statusOption', storedStatuses);
    applyStatusFilter(storedStatuses);

    // Trial  Filter
    var storedTrials = getFilterValue('trial') || [];
    $('#trialDropdown').val(storedTrials).trigger('change');
    $('#trialDropdown').on('change', function () {
        var selectedTrials = $(this).val();
        updateFilter('trial', selectedTrials);
        applyTrialFilter(selectedTrials);
    });
    // Requester Filter
    var storedRequesters = getFilterValue('requesterOption') || [];
    var storedSearchTerm = getFilterValue('requesterSearch') || '';  // Retrieve the stored search term

    $('#requesterFilter input[name="requesterOption"]').each(function () {
        $(this).prop('checked', storedRequesters.includes($(this).val()));
    });

    $('#requesterSearch').val(storedSearchTerm);  // Set the search field with the stored value

    updateFilter('requesterOption', storedRequesters);
    applyRequesterFilter(storedRequesters, storedSearchTerm);  // Apply the filter with the selected requesters and search term

    // Requester Search Filter
    $('#requesterSearch').on('keyup', function () {
        var searchTerm = $(this).val();  // Get the search term entered by the user

        var selectedRequesters = $('#requesterFilter input[name="requesterOption"]:checked').map(function () {
            return $(this).val();
        }).get();

        updateFilter('requesterSearch', searchTerm);

        applyRequesterFilter(selectedRequesters, searchTerm);
    });

    // Assignees Filter
    var storedAssignees = getFilterValue('assigneesOption') || [];
    var storedAssigneeSearchTerm = getFilterValue('assigneesSearch') || '';

    $('#assigneesFilter input[name="assigneesOption"]').each(function () {
        $(this).prop('checked', storedAssignees.includes($(this).val()));
    });

    $('#assigneesSearch').val(storedAssigneeSearchTerm);

    updateFilter('assigneesOption', storedAssignees);
    applyAssigneesFilter(storedAssignees, storedAssigneeSearchTerm);

    // Assignees Search Filter
    $('#assigneesSearch').on('keyup', function () {
        var searchTerm = $(this).val();

        var selectedAssignees = $('#assigneesFilter input[name="assigneesOption"]:checked').map(function () {
            return $(this).val();
        }).get();

        updateFilter('assigneesSearch', searchTerm);
        applyAssigneesFilter(selectedAssignees, searchTerm);
    });

    // Dates Filter
    var storedCreatedDate = getFilterValue('createdDate') || '';
    var storedFinalisedDate = getFilterValue('finalisedDate') || '';

    if (storedCreatedDate) {
        var createdDateArray = storedCreatedDate.join(', ');
        $('#createdDatePicker span').html(createdDateArray);
        updateFilter('createdDate', createdDateArray);
        applyDateFilters(createdDateArray, $('#finalisedDatePicker span').text());
    }

    if (storedFinalisedDate) {
        var finalisedDateArray = storedFinalisedDate.join(', ');
        $('#finalisedDatePicker span').html(finalisedDateArray);
        updateFilter('finalisedDate', finalisedDateArray);
        applyDateFilters($('#createdDatePicker span').text(), finalisedDateArray);
    }

    // Initialize the storedSearchTerm variable with the current filter value or an empty string
    var storedSearchTerm = getFilterValue('search') || '';

    // Set the value of the search input to the stored search term and trigger the keyup event
    $('#searchInput').val(storedSearchTerm).trigger('keyup');

    // Event listener for keyup event on the search input
    $('#searchInput').on('keyup', function () {
        var searchTerm = $(this).val(); // Get the search term
        updateFilter('search', searchTerm); // Update the filter with the search term
        applySearchFilter(searchTerm); // Apply the search filter with the search term
    });
}

function HistoricCheckboxClick(chk) {
    if ($(chk).is(":checked")) {
        localStorage.setItem("loadAll", "true");
        $('#headingHistoric').addClass('filter-active');
    } else {
        localStorage.setItem("loadAll", "false");
        $('#headingHistoric').removeClass('filter-active');
    }

    //clear table and re-initalize
    if ($.fn.DataTable.isDataTable('#subTaskTable')) {
        $('#subTaskTable').DataTable().destroy();
    }
    $('#subTaskTable').empty();

    LoadTableAndData();
}

function PopulateHistoric() {
    // If URL value not found, retrieve from localStorage
    let storedValue = localStorage.getItem("loadAll");

    // If a value is found in localStorage, process it similarly
    if (storedValue) {
        if (storedValue == "true") {
            $('#historic-checkbox').prop("checked", true);
            $('#headingHistoric').addClass('filter-active');
        }
    }
}

function escapeRegex(text) {
    return text.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

window.addEventListener('beforeunload', function () {
    sessionStorage.clear();
});

document.addEventListener("DOMContentLoaded", function () {
    const toggleInside = document.getElementById("sidebarToggle");
    const toggleFloating = document.getElementById("sidebarToggleFloating");
    const filterPanel = document.getElementById("filterPanel");
    const mainContent = document.getElementById("mainContent");

    function hideSidebar() {
        filterPanel.classList.add("d-none");
        mainContent.classList.remove("col-md-9", "col-lg-10");
        mainContent.classList.add("col-12");
        toggleInside.classList.add("d-none");
        toggleFloating.classList.remove("d-none");
        if ($.fn.DataTable.isDataTable('#subTaskTable')) {
            $('#subTaskTable').DataTable().columns.adjust();
        }
    }

    function showSidebar() {
        filterPanel.classList.remove("d-none");
        mainContent.classList.remove("col-12");
        mainContent.classList.add("col-md-9", "col-lg-10");
        toggleInside.classList.remove("d-none");
        toggleFloating.classList.add("d-none");
        if ($.fn.DataTable.isDataTable('#subTaskTable')) {
            $('#subTaskTable').DataTable().columns.adjust();
        }
    }

    toggleInside.addEventListener("click", hideSidebar);
    toggleFloating.addEventListener("click", showSidebar);
});


$(window).on('resize', function () {
    if ($.fn.DataTable.isDataTable('#subTaskTable')) {
        $('#subTaskTable').DataTable().columns.adjust();
    }
});
