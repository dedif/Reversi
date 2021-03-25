let spa = (function ($) {
    let configMap;
    let widget;

    function init(environment = "development") {
        widget = new Widget();
        spa.data.init(environment);
        console.log(spa.data.configMap);
        spa.model.init();
        $("body").append("<h1>Welkom bij Reversi!</h1>");
        spa.data.userIsLoggedIn().then(result => {
            if (result) {
                spa.data.getUser().then(
                    u => {
                        spa.model.user = new spa.model.User(u.avatar);
                    }
                );
                const logoutLink = document.createElement("span");
                logoutLink.innerHTML = "<a href='javascript:void(0)'>Uitloggen</a>";
                logoutLink.onclick = function () {
                    spa.data.logout().then(() => {
                        showLoginDialog();
                    });
                }
                $("body").append(logoutLink);
                showGameJoinOrMakeDialog();
            } else {
                showLoginDialog();
            }
        });
        return true;
    }

    // Make a HTML table row for the given game
    function printGame(game) {
        const gameRow = document.createElement("tr");
        gameRow.className = "gameRow";

        // Hidden field for the id of the game
        //const idCell = document.createElement("id");
        //idCell.id = "idCell";
        ////idCell.innerText = game.id;
        //gameRow.appendChild(idCell);

        const opponentCell = document.createElement("td");
        opponentCell.innerText = game.blackPlayer.avatar;
        gameRow.appendChild(opponentCell);

        const descriptionCell = document.createElement("td");
        descriptionCell.innerText = game.description;
        gameRow.appendChild(descriptionCell);

        const joinButton = document.createElement("button");
        joinButton.innerText = "Meedoen";
        joinButton.onclick = function () { joinGame(game.id); }
        gameRow.appendChild(joinButton);

        $("#gameTable").append(gameRow);
    }

    function joinGame(gameId) {
        spa.data.joinGame(gameId).then(response => console.log(response)).catch(response => console.log(response));
        console.log("We gaan meedoen aan een spel");
    }

    function showGameJoinOrMakeDialog() {
        widget.showMessage(
            "Spel kiezen of maken",
            "Kies een al uitstaand spel of maak er zelf één",
            "green",
            function () {
                showGameJoinOrMakeDialog();
            },
            [
                {
                    name: "Spel kiezen",
                    function: function () {
                        showGameList();
                    }
                },
                {
                    name: "Spel maken",
                    function: function () {
                        createGame();
                    }
                }
            ]
        );
    }

    function showLoginDialog() {
        widget.showMessage(
            "Log in of registreer",
            "Klik op de knop om in te loggen of je te registreren",
            "green",
            function () {
                showLoginDialog();
            },
            [
                {
                    name: "Inloggen of registreren",
                    function: function () {
                        window.location = spa.data.configMap.endpoints.loginregister;
                    }
                }
            ]
        );
    }

    function createGame() {
        const createGameForm = document.createElement("div");
        createGameForm.id = "createGameForm";
        $("body").append(createGameForm);

        $("#createGameForm").append("<h2>Maak een nieuw spel</h2>");

        const descriptionInputField = document.createElement("input");
        descriptionInputField.id = "Game_Description";
        descriptionInputField.name = "Game.Description";
        $("#createGameForm").append(descriptionInputField);

        const createGameButton = document.createElement("button");
        createGameButton.id = "createGameButton";
        createGameButton.innerText = "Verzenden";
        createGameButton.onclick = function () {
            onCreateGameClick();
        }
        $("#createGameForm").append(createGameButton);

        const backButton = document.createElement("button");
        backButton.id = "backButton";
        backButton.innerText = "Terug";
        backButton.onclick = function () {
            showGameJoinOrMakeDialog();
            $("#createGameForm").remove();
        }
        $("#createGameForm").append(backButton);
    }

    function showGameList() {
        spa.data.getSpellen()
            .then(function (response) {
                if (response.succeeded) {
                    if (response.result == undefined || response.result.length === 0) {
                        widget.showMessage(
                            "Geen spellen beschikbaar",
                            "Er zijn nog geen spellen gemaakt waar jij niet al in zit",
                            "red",
                            function () {
                                showGameJoinOrMakeDialog();
                            },
                            [
                                {
                                    name: "Spel maken",
                                    function: function () {
                                        createGame();
                                    }
                                }
                            ]
                        )
                    } else {
                        printGameHeader();
                        response.result.forEach(function (game) {
                            printGame(game);
                        });
                    }
                }
            })
            .catch(function (response) {
                widget.showMessage("Fout bij het ophalen van spellen", response.result);
            });
    }

    function printGameHeader() {
        const gameTableContainer = document.createElement("div");
        gameTableContainer.id = "gameTableContainer";
        $("body").append(gameTableContainer);

        $("#gameTableContainer").append("<h2>Doe mee aan een spel van een ander</h2>");

        const gameTable = document.createElement("table");
        gameTable.id = "gameTable";
        $("#gameTableContainer").append(gameTable);

        const tableHead = document.createElement("tr");
        tableHead.id = "tableHead";
        $("#gameTable").append(tableHead);

        $("#tableHead").append("<th>Avatar tegenstander</th>")
        $("#tableHead").append("<th>Beschrijving spel</th>")
    }

    function onCreateGameClick() {
        spa.data.createGame($("#Game_Description").val()).then(function () {
            widget.showMessage(
                "Gelukt!",
                "Je spel is nu opgeslagen!",
                "green",
                function () {
                    showGameJoinOrMakeDialog();
                });
        }).catch(function () {
            widget.showMessage(
                "Mislukt...",
                "Er ging iets mis bij het opslaan van het spel...",
                "red",
                function () {
                    showGameJoinOrMakeDialog();
                });
            // ReSharper disable once PossiblyUnassignedProperty
        }).finally(function () {
            $("#createGameForm").remove();
        });
    }

    return {
        init: init,
    }
})(jQuery);