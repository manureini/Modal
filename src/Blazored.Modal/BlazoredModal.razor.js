const el = document.body;

let keyupHandler = null;
let dotNetRef = null;
let openModalCount = 0;
let originalOverflow = '';
let originalPaddingRight = '';

/**
 * Adds event listener for the Escape key and invokes .NET method
 * @param {object} dotNetObjectReference Reference to .NET object that handles the escape key
 */
export function addEscapeKeyHandler(dotNetObjectReference) {
    // Clear state before adding the handler 
    removeEscapeKeyHandler();
    
    dotNetRef = dotNetObjectReference;
    keyupHandler = function (event) {
        if (event.key === 'Escape') {
            const hasOpenModal = document.querySelector('.bm-container, [role="dialog"]') !== null;
            if (hasOpenModal) {
                event.preventDefault();
                event.stopPropagation();
                dotNetRef?.invokeMethodAsync('HandleEscapeKey');
            }
        }
    };

    document.addEventListener('keyup', keyupHandler, true);
}

/**
 * Clears the event listener for the Escape key and resets state
 */
export function removeEscapeKeyHandler() {
    if (keyupHandler) {
        document.removeEventListener('keyup', keyupHandler, true);
        keyupHandler = null;
        dotNetRef = null;
        openModalCount = 0;
    }
}

export function setBodyStyle() {
    const scrollBarWidth = window.innerWidth - document.documentElement.clientWidth;
    originalOverflow = el.style.overflow;
    originalPaddingRight = el.style.paddingRight;

    if (scrollBarWidth > 0) {
        el.style.paddingRight = `${scrollBarWidth}px`;
    }
    
    el.style.overflow = 'hidden';
}

export function removeBodyStyle() {
    el.style.overflow = originalOverflow;
    el.style.paddingRight = originalPaddingRight;
}
