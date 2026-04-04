/**
 * TabDropdown JavaScript interop module.
 * Handles click-outside detection and mutual exclusion between open dropdowns.
 */

// dropdownId -> { dotNetRef, clickHandler }
const openDropdowns = new Map();

/**
 * Registers a dropdown as open. Closes any other open dropdowns and registers
 * a document click listener that closes this dropdown when the user clicks outside it.
 * @param {DotNetObjectReference} dotNetRef
 * @param {string} dropdownId
 * @param {HTMLElement} element
 */
export function open(dotNetRef, dropdownId, element) {
    // Close all other open dropdowns
    for (const [id, entry] of openDropdowns) {
        if (id !== dropdownId) {
            document.removeEventListener('click', entry.clickHandler);
            entry.dotNetRef.invokeMethodAsync('CloseFromJs');
        }
    }
    openDropdowns.clear();

    const clickHandler = (e) => {
        if (!element.contains(e.target)) {
            document.removeEventListener('click', clickHandler);
            openDropdowns.delete(dropdownId);
            dotNetRef.invokeMethodAsync('CloseFromJs');
        }
    };

    // Defer listener registration so the click that opened the dropdown is not immediately caught
    setTimeout(() => {
        document.addEventListener('click', clickHandler);
        openDropdowns.set(dropdownId, { dotNetRef, clickHandler });
    }, 0);
}

/**
 * Unregisters the click-outside listener for a dropdown (called when closed from C#).
 * @param {string} dropdownId
 */
export function close(dropdownId) {
    const entry = openDropdowns.get(dropdownId);
    if (entry) {
        document.removeEventListener('click', entry.clickHandler);
        openDropdowns.delete(dropdownId);
    }
}

/**
 * Cleans up all resources for a dropdown (called on component dispose).
 * @param {string} dropdownId
 */
export function dispose(dropdownId) {
    close(dropdownId);
}
