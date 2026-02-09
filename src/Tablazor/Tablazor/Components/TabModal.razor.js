/**
 * TabModal JavaScript interop module
 * Minimal JS - only handles body scroll lock which cannot be done from Blazor
 */

let openModalCount = 0;
let originalOverflow = '';
let originalPaddingRight = '';

/**
 * Sets or removes the body scroll lock.
 * Tracks multiple modals to only unlock when all are closed.
 * @param {boolean} locked - Whether to lock body scroll.
 */
export function setBodyScrollLock(locked) {
    if (locked) {
        if (openModalCount === 0) {
            // Store original values before modifying
            originalOverflow = document.body.style.overflow;
            originalPaddingRight = document.body.style.paddingRight;

            // Calculate scrollbar width to prevent layout shift
            const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

            document.body.style.overflow = 'hidden';
            document.body.classList.add('modal-open');

            if (scrollbarWidth > 0) {
                document.body.style.paddingRight = `${scrollbarWidth}px`;
            }
        }
        openModalCount++;
    } else {
        openModalCount = Math.max(0, openModalCount - 1);

        if (openModalCount === 0) {
            document.body.style.overflow = originalOverflow;
            document.body.style.paddingRight = originalPaddingRight;
            document.body.classList.remove('modal-open');
        }
    }
}
