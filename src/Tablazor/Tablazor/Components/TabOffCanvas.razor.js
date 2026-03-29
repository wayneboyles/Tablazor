/**
 * TabOffCanvas JavaScript interop module.
 * Minimal JS — only handles body scroll lock, which cannot be done from Blazor.
 */

let openCount = 0;
let originalOverflow = '';
let originalPaddingRight = '';

/**
 * Locks or unlocks body scrolling. Tracks multiple open panels so the scroll
 * is only restored once all panels have closed.
 * @param {boolean} locked
 */
export function setBodyScrollLock(locked) {
    if (locked) {
        if (openCount === 0) {
            originalOverflow = document.body.style.overflow;
            originalPaddingRight = document.body.style.paddingRight;

            const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
            document.body.style.overflow = 'hidden';

            if (scrollbarWidth > 0) {
                document.body.style.paddingRight = `${scrollbarWidth}px`;
            }
        }
        openCount++;
    } else {
        openCount = Math.max(0, openCount - 1);

        if (openCount === 0) {
            document.body.style.overflow = originalOverflow;
            document.body.style.paddingRight = originalPaddingRight;
        }
    }
}
