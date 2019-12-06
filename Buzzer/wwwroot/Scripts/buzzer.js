$(function () {
    var buzzerHub = new signalR.HubConnectionBuilder().withUrl("/buzzerHub").build();
    var roomCode = getParameterByName("roomCode");
    var playerName = getParameterByName("playerName");

    $('#game-text').text("Rumkod: " + roomCode);

    buzzerHub.on("playerJoin", function (playerName) {
        alert(playerName);
        var playerNameId = playerName.replace(/\s/g, '');
        var html = '<div id="' + playerNameId + '" class="player-icon-wrapper">';
        html += '<div class="img"></div>';
        html += ' <div class="player-name">' + playerName + '</div>';
        html += '</div>';
        $('#player-wrapper').append(html);
    });

    buzzerHub.on("playerLeave", function (playerName) {
        var playerNameId = playerName.replace(/\s/g, '');
        $('#' + playerNameId).remove();
    });

    buzzerHub.on("playerBuzz", function () {
        setTimeout(function () {
            buzzerHub.invoke("GetFirstBuzzer", roomCode);
        }, 1000);
    });

    buzzerHub.on("showFirstBuzzer", function (playerName) {
        alert(playerName);
        playerName = playerName.replace(/\s/g, '');
        $('#' + playerName).effect("shake");
        $('#' + playerName).addClass('buzzed');
    });

    buzzerHub.on("resetBuzzers", function () {
        $('.buzzed').removeClass('buzzed');
    });

    buzzerHub.start().then(function () {
        buzzerHub.invoke("OnConnected", roomCode, playerName);
    });

    $('#buzzer').click(function () {
        $(this).attr('disabled', true);
        buzzerHub.invoke("Buzz", roomCode);
        setTimeout(function () {
            $(this).attr('disabled', false);
        },1500);
    });

    $('#reset-buzzer').click(function () {
        buzzerHub.invoke("ResetBuzzer", roomCode);
    });

});


function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function lol() {
    var playerName = "bosse";
    var playerNameId = playerName.replace(/\s/g, '');
    var html = '<div class="player-icon-wrapper">';
    html += '<div id="' + playerNameId + '" class="img"></div>';
    html += ' <div class="player-name">' + playerName + '</div>';
    html += '</div>';
    $('#player-wrapper').append(html);
}
