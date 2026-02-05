$(document).ready(function () {
    LoadEditor().then(() => {

        // HACK: Only apply event listener on details page. TinyMCE initialisation comes AFTER any page specific files so must be done here or change order of files w/ unknown consequence.
        let regEx = new RegExp("((\/CHAREQ)|(\/CHAREQTRAINING)|())+(\/WORKREQUEST\/WORKREQUESTDETAILS)");
        if (regEx.test(location.pathname.toUpperCase())) {
            var allEditors = tinymce.get();

            allEditors.forEach(editorInstance => {
                const editor = editorInstance;

                if (!editor.id.startsWith('eventTextEntry_')) {
                    return;
                }

                const textBoxId = editor.id.split('_').pop();

                editor.on('keyup change input undo redo', function () {
                    EmptyDescriptionCheck(textBoxId);
                });

                // Run once on load
                EmptyDescriptionCheck(textBoxId);
            });
        }
    });
});


let LoadEditor = async () => {
     if (document.documentElement.getAttribute("data-theme") == "dark") {
        await InitialiseTinyMCE("dark");
    } else {
        await InitialiseTinyMCE("light");
    }
};

let InitialiseTinyMCE = async (mode) => {
    tinymce.init({
        selector: '.htmlForm',
        license_key: 'gpl',
        force_br_newlines: true,
        browser_spellcheck: true,
        plugins: ['code'],
        toolbar: 'code | undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | outdent indent',
        menubar: false,
        content_style: mode == "dark" ? 'body.mce-content-body{background-color: #333; color: #fff} .mce-content-body[data-mce-placeholder]:not(.mce-visualblocks)::before {color: lightgray; }' : 'body{background-color: #fff; color: #000}'
    });
};

// Function to update the theme for all TinyMCE editors
let UpdateTinyMCETheme = async (mode) => {
    let backgroundColor = mode == "dark" ? '#333' : '#fff';
    let textColor = mode == "dark" ? '#fff' : '#000';
    let placeholderColor = mode == "dark" ? 'lightgray' : 'gray';

    var allEditors = tinymce.get();

    allEditors.forEach(editorInstance => {
        const editorBody = editorInstance.getBody();
        editorBody.style.backgroundColor = backgroundColor;
        editorBody.style.color = textColor;

        const editorDoc = editorInstance.getDoc();
        let styleTag = editorDoc.getElementById('tinymce-placeholder-style');
        if (!styleTag) {
            styleTag = editorDoc.createElement('style');
            styleTag.id = 'tinymce-placeholder-style';
            editorDoc.head.appendChild(styleTag);
        }
        styleTag.innerHTML = `.mce-content-body[data-mce-placeholder]:not(.mce-visualblocks)::before {
            color: ${placeholderColor} !important;
        }`;
    });
};
