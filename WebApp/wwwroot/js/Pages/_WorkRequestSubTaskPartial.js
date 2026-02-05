$(document).ready(function () {
    InitialiseTooltips();
});

let AddSubTask = (btn) => {
    let clickedBtn = $(btn);
    if (clickedBtn.text() === "Add") {
        let modal = new bootstrap.Modal(document.getElementById('addSubTaskModal'));
        modal.show();
    }
};

let saveSubTask = () => {
    let title = $('#subTaskTitle').val().trim();
    let assignedUser = $('#assignedUser').val().trim();

    if (!title || !assignedUser) {
        alert("Please fill in both fields.");
        return;
    }

    // Example: Clear inputs and close modal
    $('#subTaskTitle').val('');
    $('#assignedUser').val('');
    bootstrap.Modal.getInstance(document.getElementById('addSubTaskModal')).hide();
}; 

function ShowSpinner(btn) {
    if ($('#addSubTaskForm').valid()) {
        $("#spinner").show();
        $(btn).hide();
    }
}

function openAccordion(headingId) {
    const heading = document.getElementById(headingId);
    if (!heading) return;

    const allAccordions = document.querySelectorAll('.accordion-collapse.show');
    allAccordions.forEach(acc => {
        if (!heading.contains(acc)) {
            new bootstrap.Collapse(acc, {
                toggle: true
            }).hide();
        }
    });

    const button = heading.querySelector('button[data-bs-toggle="collapse"]');
    const targetSelector = button?.getAttribute('data-bs-target');
    const target = document.querySelector(targetSelector);

    if (target && !target.classList.contains('show')) {
        const collapse = new bootstrap.Collapse(target, {
            toggle: true
        });

        setTimeout(() => {
            heading.scrollIntoView({ behavior: 'smooth', block: 'start' });
        },200);
    } else {
        heading.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
}

const textbox = document.getElementById("mainTextBoxWrapper");
const showTextBoxButton = document.getElementById("showTextBoxButton");

function checkAccordions() {
    const openAccordions = document.querySelectorAll(".accordion-collapse.show");
    const anyOpen = openAccordions.length > 0;

    textbox.style.display = anyOpen ? "none" : "block";
    showTextBoxButton.classList.toggle("d-none", !anyOpen);
}

function collapseAllSubtasks() {
    document.querySelectorAll(".accordion-collapse.show").forEach(acc => {
        const collapseInstance = bootstrap.Collapse.getInstance(acc) || new bootstrap.Collapse(acc, { toggle: false });
        collapseInstance.hide();
    });

    textbox.style.display = "block";
    showTextBoxButton.classList.add("d-none");
}

document.querySelectorAll('.accordion-collapse').forEach(acc => {
    acc.addEventListener('shown.bs.collapse', checkAccordions);
    acc.addEventListener('hidden.bs.collapse', checkAccordions);
});

