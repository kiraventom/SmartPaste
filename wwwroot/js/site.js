(function () {
    function setupAutoSize(textarea) {
        const maxHeight =
            parseInt(getComputedStyle(textarea).maxHeight, 10) || 400;

        function autoResize() {
            textarea.style.height = 'auto';

            if (textarea.scrollHeight > maxHeight) {
                textarea.style.height = maxHeight + 'px';
                textarea.style.overflowY = 'auto';
            } else {
                textarea.style.height = textarea.scrollHeight + 'px';
                textarea.style.overflowY = 'hidden';
            }
        }

        textarea.addEventListener('input', autoResize);
        autoResize(); // initial size (e.g., when model pre-fills Text)
    }

    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('textarea.ta-autosize').forEach(setupAutoSize);
    });
})();
