document.addEventListener("DOMContentLoaded", () => {
    const textarea = document.getElementById('textarea');
    if (!textarea)
        return;

    const textarea_width_key = 'textarea_w';
    const textarea_height_key = 'textarea_h';

    const saved_width = localStorage.getItem(textarea_width_key);
    const saved_height = localStorage.getItem(textarea_height_key);

    if (saved_width && saved_height)
    {
        textarea.style.width = saved_width + 'px';
        textarea.style.height = saved_height + 'px';
    }

    const resizeObserver = new ResizeObserver(() => {
        localStorage.setItem(textarea_width_key, textarea.offsetWidth);
        localStorage.setItem(textarea_height_key, textarea.offsetHeight);
    });

    resizeObserver.observe(textarea);
});
