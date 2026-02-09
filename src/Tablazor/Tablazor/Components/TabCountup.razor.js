/**
 * TabCountup scroll spy functionality.
 * Uses IntersectionObserver for efficient viewport detection.
 */

const observers = new Map();

/**
 * Sets up scroll spy for a countup element.
 * @param {string} elementId - The ID of the element to observe.
 * @param {object} dotNetRef - The .NET object reference for callbacks.
 */
export function setupScrollSpy(elementId, dotNetRef) {
    const element = document.getElementById(elementId);

    if (!element) {
        console.warn(`TabCountup: Element with id '${elementId}' not found.`);
        return;
    }

    // Clean up any existing observer
    cleanup(elementId);

    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    dotNetRef.invokeMethodAsync('OnElementVisible');
                }
            });
        },
        {
            threshold: 0.1, // Trigger when 10% of element is visible
            rootMargin: '0px'
        }
    );

    observer.observe(element);
    observers.set(elementId, { observer, dotNetRef });
}

/**
 * Cleans up the observer for an element.
 * @param {string} elementId - The ID of the element to stop observing.
 */
export function cleanup(elementId) {
    const data = observers.get(elementId);

    if (data) {
        data.observer.disconnect();
        observers.delete(elementId);
    }
}
