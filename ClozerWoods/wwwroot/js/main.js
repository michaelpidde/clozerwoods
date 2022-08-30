const attachItemSwitchEvent = (id) => {
    const element = document.getElementById(id);
    if (element != null) {
        element.onchange = () => {
            var url = location.protocol + '//' + location.host + location.pathname;
            if (element.value != "") {
                url += `?${id}=${element.value}`;
            }
            location.href = url;
        }
    }
}

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

const showHelpModal = () => {
    console.log('help modal');
    const modal = new Modal(() => { });
    modal.show();
}

const showMediaItemModal = () => {
    console.log('media item modal');
    const content = document.querySelector('#pageContent');
    const insertPosition = content.selectionStart;
    console.log(insertPosition);
}

let Modal = class {
    width = 400;
    height = 300;

    #modalOverlayId = 'modal-overlay';
    #modalContainerId = 'modal-container';
    #modalCloseId = 'modal-close';

    constructor(contentCallback) {
        if (typeof (contentCallback) !== 'function') {
            console.error('Argument contentCallback of class Modal must be of type function.');
        }
        this.contentCallback = contentCallback;
        // Store a local reference to onResizeEvent with a bound 'this' context so addEventListener
        // and removeEventListener can reference it properly
        this.resizeHandler = this.onResizeEvent.bind(this);
    }

    show() {
        const overlay = this.getOverlay();

        const container = this.getContainer();
        container.appendChild(this.getClose());

        overlay.appendChild(container);
        document.body.prepend(overlay);

        this.addEvents();
    }

    close() {
        this.removeEvents();
        document.getElementById(this.#modalOverlayId).remove();
    }

    onResizeEvent() {
        this.positionContainer();
    }

    addEvents() {
        window.addEventListener('resize', this.resizeHandler);
        document.getElementById(this.#modalCloseId).addEventListener('click', () => {
            this.close();
        });
    }

    removeEvents() {
        // Remove any events that won't otherwise be removed by destroying this object
        window.removeEventListener('resize', this.resizeHandler);
    }

    positionContainer(container) {
        if (container === undefined) {
            container = document.getElementById(this.#modalContainerId);
        }
        container.style.top = (window.innerHeight / 2) - (this.height / 2) + 'px';
        container.style.left = (window.innerWidth / 2) - (this.width / 2) + 'px';
    }

    getClose() {
        const close = document.createElement('div');
        close.id = this.#modalCloseId;
        return close;
    }

    getContainer() {
        const container = document.createElement('div');
        container.id = this.#modalContainerId;
        container.style.width = `${this.width}px`;
        container.style.height = `${this.height}px`;
        this.positionContainer(container);
        return container;
    }

    getOverlay() {
        const overlay = document.createElement('div');
        overlay.id = this.#modalOverlayId;
        return overlay;
    }
};