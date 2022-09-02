const generateThumbnailPreview = (element) => {
    const parent = element.parentNode;

    let image = document.createElement('img');
    image.src = element.href;

    let preview = document.createElement('div');
    preview.className = 'thumbPreview';
    preview.appendChild(image);
    preview.style.top = parent.style.bottom;
    preview.style.left = parent.style.left;
    parent.appendChild(preview);
}

const showThumbnailPreview = (element) => {
    const preview = element.parentNode.getElementsByClassName('thumbPreview')[0];
    preview.style.display = 'block';
}

const hideThumbnailPreview = (element) => {
    const preview = element.parentNode.getElementsByClassName('thumbPreview')[0];
    preview.style.display = 'none';
}

const showGuideModal = (content) => {
    const modal = new Modal('Formatting Guide', () => {
        return content;
    });
    modal.show();
}

const showMediaItemModal = () => {
    fetch("/maingate/mediaitems/all")
        .then(response => response.json())
        .then(data => {
            const modal = new Modal('Insert Media Item', () => {
                let items = '';
                data.items.forEach(
                    item => items += `<div><img src="${data.mediaUrl}/${item.thumbnail}" data-id="${item.id}"></div>`
                );
                return `<div id="media-item-grid">${items}</div>`;
            });

            document.querySelectorAll("#media-item-grid > div img").forEach(img => {
                img.onclick = () => {
                    navigator.clipboard.writeText(`[img ${img.dataset.id}]`);
                    // TODO: Show temporary modal that it's copied...
                };
            })

            modal.show();
        });
}

let Modal = class {
    #modalOverlayId = 'modal-overlay';
    #modalContainerId = 'modal-container';
    #modalCloseId = 'modal-close';
    #modalHeaderId = 'modal-header';
    #modalContentId = 'modal-content';

    constructor(title, contentCallback) {
        this.title = title;
        if (typeof (contentCallback) !== 'function') {
            console.error('Argument contentCallback of class Modal must be of type function.');
        }
        this.contentCallback = contentCallback;

        this.create();
    }

    create() {
        const overlay = this.getOverlay();

        const container = this.getContainer();
        container.appendChild(this.getHeader());
        container.appendChild(this.getContent());

        overlay.appendChild(container);
        document.body.prepend(overlay);

        this.addEvents();
    }

    show() {
        document.getElementById(this.#modalOverlayId).style.display = 'block';
    }

    close() {
        document.getElementById(this.#modalOverlayId).remove();
    }

    addEvents() {
        document.getElementById(this.#modalCloseId).addEventListener('click', () => {
            this.close();
        });
    }

    getClose() {
        const close = document.createElement('div');
        close.id = this.#modalCloseId;
        return close;
    }

    getContent() {
        const content = document.createElement('div');
        content.id = this.#modalContentId;
        content.innerHTML = this.contentCallback();
        return content;
    }

    getHeader() {
        const header = document.createElement('div');
        header.id = this.#modalHeaderId;
        if (this.title.length) {
            header.innerHTML = `<h1>${this.title}</h1>`;
        }
        header.appendChild(this.getClose());
        return header;
    }

    getContainer() {
        const container = document.createElement('div');
        container.id = this.#modalContainerId;
        container.style.width = `${this.width}px`;
        container.style.height = `${this.height}px`;
        return container;
    }

    getOverlay() {
        const overlay = document.createElement('div');
        overlay.id = this.#modalOverlayId;
        return overlay;
    }
};