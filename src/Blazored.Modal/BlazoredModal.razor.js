const el = document.body;
const computedBodyStyle = getComputedStyle(el);
const originalProps = { overflow: computedBodyStyle.overflow, paddingRight: computedBodyStyle.paddingRight };

let keyupHandler = null;
let dotNetRef = null;

const getScrollBarWidth = () => {
    let el = document.createElement("div");
    el.style.cssText = "overflow:scroll; visibility:hidden; position:absolute;";
    document.body.appendChild(el);
    let width = el.offsetWidth - el.clientWidth;
    el.remove();
    return width;
}
const isScrollbarPresent = () => {
    const beforeScrollbarHidden = document.body.clientWidth;
    const overflowState = document.body.style.overflow;
    document.body.style.overflow = 'hidden';
    const afterScrollbarHidden = document.body.clientWidth;
    document.body.style.overflow = overflowState;
    return beforeScrollbarHidden !== afterScrollbarHidden;
};

/**
 * Adds event listener for the Escape key and invokes .NET method
 * @param {object} dotNetObjectReference Reference to .NET object that handles the escape key
 */
export function addEscapeKeyHandler(dotNetObjectReference) {
    // Clear state before adding the handler 
    removeEscapeKeyHandler()
    
    dotNetRef = dotNetObjectReference;
    keyupHandler = function (event) {
        if(event.key === 'Escape') {
            event.preventDefault()
            event.stopPropagation()
            dotNetRef.invokeMethodAsync('HandleEscapeKey')
        }
    }

    document.addEventListener('keyup', keyupHandler, true)
}

/**
 * Clears the event listener for the Escape key and resets state
 */
export function removeEscapeKeyHandler() {
    if(keyupHandler) {
        document.removeEventListener('keyup', keyupHandler, true)
        keyupHandler = null
        dotNetRef = null
    }
}

export function setBodyStyle() {
    if (isScrollbarPresent()) {
        el.style.paddingRight = `${getScrollBarWidth()}px`;
    }
    
    el.style.overflow = 'hidden';
}

export function removeBodyStyle() {
    el.style.overflow = originalProps.overflow || 'auto';
    el.style.paddingRight = originalProps.paddingRight;
}
