/* Trims a number to a string with a maximum allowable value.
 * Used when large numbers are undesirable for display and should
 * be trimmed.
 * E.g. Under default optional parameters
 * value: 120 => return: '99+'
 * */
let TrimNumberToString = (value, maxValue = 99, appendChar = '+') => {
    if (value > maxValue) {
        return `${maxValue}${appendChar}`;
    } else {
        return value;
    }
}

/* Trims a string when the maximum length is exceeded.
 * E.g. 
 * string: 'A long string', maxLength: 10
 * => 
 * return: 'A long str...'
 * */
let TrimStringToMaxLength = (string, maxLength = 99, appendChars = '...') => {
    if (string.length > maxLength) {
        return `${string.substring(0, maxLength - 1)}${appendChars}`;
    } else {
        return string;
    }
}

/* Trims the domain off an email address
 * e.g.
 * user.name@example.com => user.name
 * */
let RemoveDomainFromEmail = (email) => {
    return email.split('@')[0];
}


/*
 * Methods used to calculate the width of text displayed on screen.
 * Taken from: https://stackoverflow.com/questions/118241/calculate-text-width-with-javascript
 * Call like: GetTextWidth("hello there!", GetCanvasFont(element))
 */
let GetTextWidth = (text, font) => {
    // re-use canvas object for better performance
    const canvas = GetTextWidth.canvas || (GetTextWidth.canvas = document.createElement("canvas"));
    const context = canvas.getContext("2d");
    context.font = font;
    const metrics = context.measureText(text);
    return metrics.width;
}
let GetCssStyle = (element, prop) => {
    return window.getComputedStyle(element, null).getPropertyValue(prop);
}
let GetCanvasFont = (el = document.body) => {
    const fontWeight = GetCssStyle(el, 'font-weight') || 'normal';
    const fontSize = GetCssStyle(el, 'font-size') || '16px';
    const fontFamily = GetCssStyle(el, 'font-family') || 'Times New Roman';

    return `${fontWeight} ${fontSize} ${fontFamily}`;
}



/* Gets the domain of the url with the first path if live or training, else just the domain.
 * If the site is running on live or training the url will be in the form
 * "<domain>/ChaReq" OR "<domain>/ChaReqTraining".
 * On dev/localhost it will not have a first path.
 * Returns "<domain>/ChaReq" OR "<domain>/ChaReqTraining" on live and training.
 * Returns "<domain>" on dev/localhost/
 * */
let GetUrlBase = () => {
    let domain = window.location.origin;
    let firstPath = window.location.pathname.split('/')[1];
    let firstPathRegex = new RegExp("(?:^CHAREQ$)|(?:^CHAREQTRAINING$)");

    if (firstPath && firstPathRegex.test(firstPath.toUpperCase()))
        return domain + '/' + firstPath
    else
        return domain;
}


/* Calculates whether white or black text will contast better on a given background colour.
 * This is an estimate, and could be improved on.
 * Calculated based on percieved brightness of colours to the human eye, and compared.
 * This functionality is duplicated server-side in CommonHelpers.CalculateBestTextColor(string hexColor)
 * See: https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color
 */
let CalculateBestTextColor = (hexColor) => {
    var color = (hexColor.charAt(0) === '#') ? hexColor.substring(1, 7) : hexColor;
    var r = parseInt(color.substring(0, 2), 16); // hexToR
    var g = parseInt(color.substring(2, 4), 16); // hexToG
    var b = parseInt(color.substring(4, 6), 16); // hexToB
    return (((r * 0.299) + (g * 0.587) + (b * 0.114)) > 186) ?
        "#000000" : "#ffffff";
}

/* Copies the contents of a passed elementId to the clipboard.
 * See: https://stackoverflow.com/questions/47931843/javascript-copy-to-clipboard-not-working
 */ 
let CopyElementTextToClipboard = (elementId) => {
    var range, selection, worked;

    let element = document.getElementById(elementId);

    if (document.body.createTextRange) {
        range = document.body.createTextRange();
        range.moveToElementText(element);
        range.select();
    } else if (window.getSelection) {
        selection = window.getSelection();
        range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);
    }

    try {
        document.execCommand('copy');
        // Uncomment below for debugging copy functionality.
        /*alert('text copied');*/
    }
    catch (err) {
        alert('Unable to copy text');
    }
}

/* Searches the classes of the element for begining with the passed in filter RegExp.
 * Note: for special chars, you need to escape them 'btn\-' for 'btn-'.
 * See: https://stackoverflow.com/a/40158516
 */ 
$.fn.removeClassStartingWith = function (filter) {
    $(this).removeClass(function (index, className) {
        return (className.match(new RegExp("\\S*" + filter + "\\S*", 'g')) || []).join(' ')
    });
    return this;
};


/* Source: https://stackoverflow.com/a/14853974
 * Use for comparing arrays. Rather than having to construct a for loop and compare each time, this is premade.
 */
// Warn if overriding existing method
if (Array.prototype.equals)
    console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.");
// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
    // if the other array is a falsy value, return
    if (!array)
        return false;
    // if the argument is the same array, we can be sure the contents are same as well
    if (array === this)
        return true;
    // compare lengths - can save a lot of time 
    if (this.length != array.length)
        return false;

    for (var i = 0, l = this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i]))
                return false;
        }
        else if (this[i] != array[i]) {
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;
        }
    }
    return true;
}
// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", { enumerable: false });