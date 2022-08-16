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