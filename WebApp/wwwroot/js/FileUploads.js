const fileUploadInput = $('#fileUpload');
const fileUploadLabel = $('#fileUploadLabelText');
const fileUploadWarningText = $('#fileUploadWarning');
const fileUploadList = $('#fileUploadTitles');

let fileStore = new DataTransfer();

$(document).ready(function () {
    //Event Listener for file upload
    fileUploadInput.on('change', function () {
        for (let i = 0; i < this.files.length; i++) {
            fileStore.items.add(this.files[i]);
        }

        this.files = fileStore.files;

        const fileCount = this.files.length;

        if (fileCount > 0) {
            fileUploadWarningText.removeClass('d-none');
        } else {
            fileUploadWarningText.addClass('d-none');
        }

        displayFileInfo(this.files);
    })
});

function displayFileInfo(files) {
    fileUploadList.empty();

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const listItem = document.createElement('li');
        listItem.textContent = `✅ ${file.name} (${(file.size / 1024 / 1024).toFixed(2)} MB) `;

        const deleteButton = document.createElement('a');
        deleteButton.innerHTML = '<i class="fas fa-trash text-danger"></i>';
        deleteButton.href = '#';
        deleteButton.classList.add('pointerOnHover');
        deleteButton.onclick = function (e) {
            e.preventDefault();
            removeFileFromFileList(i);
        };

        listItem.appendChild(deleteButton);
        fileUploadList.append(listItem);
    }
}

function removeFileFromFileList(index) {
    const dt = new DataTransfer();
    const files = fileUploadInput[0].files;

    for (let i = 0; i < files.length; i++) {
        if (index !== i) {
            dt.items.add(files[i]);
        }
    }

    fileUploadInput[0].files = dt.files;
    fileStore = dt;

    displayFileInfo(fileUploadInput[0].files);

    if (fileUploadInput[0].files.length === 0) {
        fileUploadWarningText.addClass('d-none');
    }
}

let confirmDelete = (btn, fileName) => {
    Swal.fire({
        title: "Are you sure?",
        html: `You are about to delete the file: <strong>${fileName}</strong>. This action cannot be undone.`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dc3545", // Bootstrap danger
        cancelButtonColor: "#6c757d", // Bootstrap secondary
        confirmButtonText: "Yes!"
    }).then((result) => {
        if (result.isConfirmed) {
            $(btn).closest('form').submit();
        }
    })
}

let DownloadFile = async (fileId, fileName) => {
    // Get the file in a fetch.
    let response = await GetSupportingFileDownloadApi(fileId);

    // Make sure the file was retrieved. If not, do nothing!
    if (response.ok && response.status !== 204) {
        // Get the blob, and create a URL for it.
        let blob = await response.blob();
        const blobUrl = window.URL.createObjectURL(blob);

        // Create a dummy <a> tag for it. Add the href etc to link to the file.
        const link = document.createElement("a");
        link.href = blobUrl;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();

        // Remove from the page, as it's done with now.
        link.remove();
    }
}