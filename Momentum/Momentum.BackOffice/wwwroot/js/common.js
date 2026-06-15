window.common = {
    downloadFileFromStreamAsync: async function (fileName, contentStreamReference) {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    },

    positionDatePickerPanel: function (anchorElement, popupElement, gap) {
        if (!anchorElement || !popupElement) {
            return;
        }

        const position = () => {
            const rect = anchorElement.getBoundingClientRect();
            const spacing = gap ?? 8;
            const popupWidth = Math.max(rect.width, 280);
            const popupHeight = popupElement.offsetHeight || 320;

            let top = rect.bottom + spacing;
            let left = rect.left;

            if (top + popupHeight > window.innerHeight - spacing) {
                top = Math.max(spacing, rect.top - popupHeight - spacing);
            }

            if (left + popupWidth > window.innerWidth - spacing) {
                left = window.innerWidth - popupWidth - spacing;
            }

            left = Math.max(spacing, left);

            popupElement.style.position = 'fixed';
            popupElement.style.top = `${top}px`;
            popupElement.style.left = `${left}px`;
            popupElement.style.width = `${popupWidth}px`;
            popupElement.style.zIndex = '1060';
        };

        position();
        requestAnimationFrame(position);
    }
}