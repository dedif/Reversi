spa.data = (function ($) {
    let configMap;

    function init(environment = "development") {
        configMap = {
            environment: environment,
            endpoints: {
            }
        };
        if (environment === "production") {
            configMap.endpoints.game = "https://localhost:5001/api/game";
            configMap.endpoints.player = "https://localhost:5001/api/player";
            configMap.endpoints.loginregister = "https://localhost:5001/loginregister"
        }
        return true;
    }

    function getSpellen() {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.game != undefined) {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: configMap.endpoints.game + "/games",
                    success: function (data, response) {
                        data = removeUnavailableGames(data);
                        resolve({ succeeded: true, result: data });
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        reject({ succeeded: false, result: textStatus + ": " + errorThrown });
                    }
                });
            } else {
                resolve();
            }
        });
    }

    function removeUnavailableGames(data) {
        return data.filter(g =>
            g.blackPlayer.avatar !== spa.model.user.avatar &&
            g.blackPlayer.currentlyPlayingGame === null
        );
    }

    function logout() {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.player != undefined) {
                $.post(configMap.endpoints.player + "/logout",
                    function (d) {
                        console.log(d);
                        resolve("logged out");
                    })
                    .fail(
                        function () {
                            reject("not logged out");
                        });
            } else {
                resolve("logged out");
            }
        });
    }

    function userIsLoggedIn() {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.player != undefined) {
                $.get(configMap.endpoints.player + "/isLoggedIn",
                    function (response) {
                        resolve(response);
                    })
                    .fail(function () {
                        reject();
                    });
            } else {
                resolve(true);
            }
        });
    }

    function getUser() {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.player != undefined) {
                $.get(configMap.endpoints.player + "/getLoggedInPlayer",
                    function (player) {
                        resolve(player);
                    })
                    .fail(function () {
                        reject()
                    });
            } else {
                resolve()
            }
        });
    }

    function createGame(description) {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.player != undefined) {
                $.ajax({
                    type: "POST",
                    url: configMap.endpoints.game + "/create",
                    contentType: "application/json",
                    data: JSON.stringify(description),
                    success: function (response) {
                        resolve(response);
                    },
                    error: function (response) {
                        reject(response);
                    }
                });
            } else {
                resolve();
            }
        });
    }

    function joinGame(gameId) {
        return new Promise(function (resolve, reject) {
            if (configMap.endpoints.player != undefined) {
                $.ajax({
                    type: "POST",
                    url: configMap.endpoints.game + "/join",
                    contentType: "application/json",
                    data: JSON.stringify(gameId),
                    success: function (response) {
                        resolve(response);
                    },
                    error: function (response) {
                        reject(response);
                    }
                });
            } else {
                resolve();
            }
        });
    }

    return {
        init: init,
        configMap: function () {
            return configMap;
        },
        getSpellen: getSpellen,
        logout: logout,
        userIsLoggedIn: userIsLoggedIn,
        createGame: createGame,
        getUser: getUser,
        joinGame: joinGame
    };
})(jQuery);