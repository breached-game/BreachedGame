//from https://www.dmcinfo.com/latest-thinking/blog/id/9852/multi-user-video-chat-with-webrtc
// check if have to import WebSocket stuff

var localUuid;
var localDisplayName;
var localStream;
var serverConnection;
var peerConnections = {}; // key is uuid, values are peer connection object and user defined display name string

const peerConnectionConfig = {
  iceServers: [
    { urls: "stun:stun.stunprotocol.org:3478" },
    { urls: "stun:stun.l.google.com:19302" },
  ],
};

mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello world");
  },

  // set up local video stream
  start: function () {
    //need to find a way to assign each client a unique ID - could use players network identity. Something like ...
    console.log("Before getting the network manager");
    localUuid = window.unityInstance.SendMessage(
      "NetworkManager",
      "GetNetworkIdentity"
    );
    console.log("Network Identity = " + localUuid);
    localDisplayName = localUuid; //should have a better name here

    var constraints = {
      video: false,
      audio: true,
    };

    if (navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices
        .getUserMedia(constraints)
        .then(function (stream) {
          localStream = stream;
          console.log("Got MediaStream:", stream);
          window.unityInstance.SendMessage("MicManager", "MicRecieved");
        })
        .catch(function (errorHandler) {
          console.error("Error getting the mic.", errorHandler);
          window.unityInstance.SendMessage("MicManager", "MicRejected");
        })

        // set up websocket and message all existing clients
        .then(function () {
          //serverConnection = new WebSocket('wss://' + window.location.hostname + ':' + WS_PORT);
          serverConnection = new WebSocket(
            "wss://breached-webrtc.icedcoffee.dev:7777"
          ); //may need to add port
          serverConnection.onmessage = gotMessageFromServer;
          serverConnection.onopen = function (event) {
            serverConnection.send(
              JSON.stringify({
                displayName: localDisplayName,
                uuid: localUuid,
                dest: "all",
              })
            );
          };
        })
        .catch(function (errorHandler) {
          console.error("Error setting up websocket connection.", errorHandler);
        });
    } else {
      alert("Your browser does not support getUserMedia API");
    }
  },
});
