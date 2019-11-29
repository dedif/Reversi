function Widget() {
    function showMessage(title, message, colour, crossButtonFunction = function () {}, buttons = []) {
        const thereIsAWidget = document.getElementsByClassName("widget").length > 0;
        if (thereIsAWidget) {
            const oldWidget = document.getElementsByClassName("widget").item(0);
            oldWidget.parentNode.removeChild(oldWidget);
        }
        // let oldWidget =
        // document.getElementsByClassName("widget").item(0).parentNode.removeChild
        const widget = document.createElement("div");
        const icon = document.createElement("span");
        const containerForText = document.createElement("div");
        const crossButton = document.createElement("div");
        const containerForButtons = document.createElement("div");

        widget.className += "widget";
        if (colour === "green") {
            widget.id = "okWidget";
            icon.innerHTML = "<i class='fas fa-check'></i>";
        } else {
            widget.id = "errorWidget";
            icon.innerHTML = "<i class='fas fa-exclamation'></i>";
        }

        icon.id = "widgetImage";
        widget.appendChild(icon);

        const head = document.createElement("h1");
        head.innerText = title;
        containerForText.appendChild(head);

        crossButton.id = "crossButton";
        crossButton.innerHTML = "<i class='fas fa-times-circle'></i>";
        crossButton.onclick = function () {
            hideMessage();
            crossButtonFunction();
        };
        waitForClick(crossButton);
        widget.appendChild(crossButton);

        containerForText.innerHTML += "<br>";
        containerForText.innerHTML += message;

        containerForText.id = "container";

        widget.appendChild(containerForText);

        containerForButtons.id = "containerForButtons";
        buttons = buttons || [];
        buttons.reverse();
        buttons.forEach(function(buttonData) {
            const buttonInContainer = document.createElement("button");
            buttonInContainer.innerText = buttonData.name;
            buttonInContainer.onclick = function() {
                buttonData.function();
                hideMessage();
            }
            containerForButtons.appendChild(buttonInContainer);
        });
        widget.appendChild(containerForButtons);

        document.body.appendChild(widget);
    }

    function shake(crossButton) {
        crossButton.classList.add("shakingButton");
    }

    function waitForClick(crossButton) {
        new Promise((resolve) => {
            setTimeout(() => {
                resolve();
            },2000);
        }).then(() => {
            shake(crossButton);
        });
    }

    function storeMessage(message) {
        if (localStorage.widget) {
            let localStorageItems = JSON.parse(localStorage.widget);
            localStorageItems.push(message);

            if (localStorageItems.length > 10) {
                localStorageItems = localStorageItems.slice(-10);
            }
            localStorage.widget = JSON.stringify(localStorageItems);
        } else {
            const localStorageItems = [message];
            localStorage.setItem("widget", JSON.stringify((localStorageItems)));
        }
    }

    function hideMessage() {
        const widget = document.getElementsByClassName("widget").item(0);
        widget.classList.add("onDeletion");
        // _widget.parentNode.removeChild(_widget);
        storeMessage("Gesloten");
    }

    return {
        showMessage: showMessage,
        hideMessage: hideMessage
    };
};