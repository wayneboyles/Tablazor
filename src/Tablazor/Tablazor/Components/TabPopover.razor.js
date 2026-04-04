/**
 * TabPopover JavaScript interop module.
 * Handles click-outside detection for click-triggered popovers.
 */

// popoverId -> { dotNetRef, clickHandler }
const openPopovers = new Map();

/**
 * Registers a document click listener that closes the popover when the user clicks outside it.
 * @param {DotNetObjectReference} dotNetRef
 * @param {string} popoverId
 * @param {HTMLElement} element
 */
export function registerClickOutside(dotNetRef, popoverId, element) {
    unregister(popoverId);

    const clickHandler = (e) => {
        if (!element.contains(e.target)) {
            document.removeEventListener('click', clickHandler);
            openPopovers.delete(popoverId);
            dotNetRef.invokeMethodAsync('CloseFromJs');
        }
    };

    // Defer so the triggering click does not immediately close the popover
    setTimeout(() => {
        document.addEventListener('click', clickHandler);
        openPopovers.set(popoverId, { dotNetRef, clickHandler });
    }, 0);
}

/**
 * Unregisters the click-outside listener for a popover.
 * @param {string} popoverId
 */
export function unregister(popoverId) {
    const entry = openPopovers.get(popoverId);
    if (entry) {
        document.removeEventListener('click', entry.clickHandler);
        openPopovers.delete(popoverId);
    }
}
