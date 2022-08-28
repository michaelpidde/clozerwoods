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