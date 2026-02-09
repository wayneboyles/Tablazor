export function extractHtml(demoId) {
    const demoElement = document.getElementById(demoId);
    if (!demoElement) {
        console.error('Demo element not found:', demoId);
        return '';
    }

    // Clone to avoid modifying the original
    const clone = demoElement.cloneNode(true);

    // Remove all Blazor-specific attributes and comments
    cleanupBlazorAttributes(clone);

    // Get the cleaned HTML
    return clone.innerHTML;
}

function cleanupBlazorAttributes(element) {
    // Remove Blazor comments
    const walker = document.createTreeWalker(
        element,
        NodeFilter.SHOW_COMMENT,
        null
    );

    const comments = [];
    let node;
    while (node = walker.nextNode()) {
        comments.push(node);
    }
    comments.forEach(comment => comment.remove());

    // Remove Blazor attributes from all elements
    const allElements = element.getElementsByTagName('*');
    for (let el of allElements) {
        const attributesToRemove = [];
        for (let attr of el.attributes) {
            if (attr.name.startsWith('_bl_') ||
                attr.name.startsWith('b-') ||
                attr.name.startsWith('blazor:')) {
                attributesToRemove.push(attr.name);
            }
        }
        attributesToRemove.forEach(attrName => el.removeAttribute(attrName));

        // Clean up empty class attributes
        if (el.hasAttribute('class') && el.getAttribute('class').trim() === '') {
            el.removeAttribute('class');
        }
    }
}
