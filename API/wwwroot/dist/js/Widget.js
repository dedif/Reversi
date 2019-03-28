let widget = (function() {
    function showMessage(title, message, colour) {
        let thereIsAWidget = document.getElementsByClassName("widget").length > 0;
        if (thereIsAWidget) {
            let oldWidget = document.getElementsByClassName("widget").item(0);
            oldWidget.parentNode.removeChild(oldWidget)
        }
        // let oldWidget =
        // document.getElementsByClassName("widget").item(0).parentNode.removeChild
        let _widget = document.createElement("div");
        let icon = document.createElement("span");
        let containerForText = document.createElement("div");
        let crossButton = document.createElement("div");

        _widget.className += "widget";
        if (colour === "green") {
            _widget.id = "okWidget";
            icon.innerHTML = "<i class='fas fa-check'></i>";
        } else {
            _widget.id = "errorWidget";
            icon.innerHTML = "<i class='fas fa-exclamation'></i>";
        }

        icon.id = "widgetImage";
        _widget.appendChild(icon);

        let head = document.createElement("h1");
        head.innerHTML = title;
        containerForText.appendChild(head);

        containerForText.innerHTML += "<br>";
        containerForText.innerHTML += message;

        containerForText.id = "container";

        _widget.appendChild(containerForText);

        crossButton.id = "crossButton";
        crossButton.innerHTML = "<i class='fas fa-times-circle'></i>";
        crossButton.onclick = function() {
            hideMessage()
        };
        _widget.appendChild(crossButton);

        document.body.appendChild(_widget);
    }

    function _storeMessage(message) {
        if (localStorage.widget) {
            let localStorageItems = JSON.parse(localStorage.widget);
            localStorageItems.push(message);

            if (localStorageItems.length > 10) {

            }
            localStorage.widget = JSON.stringify(localStorageItems);
        } else {
            let localStorageItems = [message];
            alert("Hoi");
            localStorage.setItem("widget", JSON.stringify((localStorageItems)))
            // localStorageItems.widget = JSON.stringify(localStorageItems);
        }
    }

    function hideMessage() {
        let _widget = document.getElementsByClassName("widget").item(0);
        _widget.parentNode.removeChild(_widget);
        _storeMessage("Gesloten");
    }

    return {
        showMessage: showMessage,
        hideMessage: hideMessage
    }
})();