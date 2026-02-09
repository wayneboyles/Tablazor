export function highlightCode() {
    document.querySelectorAll('pre code').forEach((block) => {
        hljs.highlightElement(block);
    });
}

export function copyToClipboard(text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
        return navigator.clipboard.writeText(text);
    }

    // Fallback for older browsers
    const textArea = document.createElement('textarea');
    textArea.value = text;
    textArea.style.position = 'fixed';
    textArea.style.left = '-999999px';
    document.body.appendChild(textArea);
    textArea.select();

    try {
        document.execCommand('copy');
    } catch (err) {
        console.error('Failed to copy:', err);
    }

    document.body.removeChild(textArea);
}
