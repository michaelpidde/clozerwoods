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
    const content = document.querySelector('#pageContent');
    const insertPosition = content.selectionStart;
    console.log(insertPosition);
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
    }

    show() {
        const overlay = this.getOverlay();

        const container = this.getContainer();
        container.appendChild(this.getHeader());
        container.appendChild(this.getContent());

        overlay.appendChild(container);
        document.body.prepend(overlay);

        this.addEvents();
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