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

  // Once we have a new peer, we can add them to the peerConnections object
  // with the UUID as a key.
  setUpPeer: function (peerUuid, displayName, initCall = false) {
    peerConnections[peerUuid] = {
      displayName: displayName,
      pc: new RTCPeerConnection(peerConnectionConfig),
    };
    peerConnections[peerUuid].pc.onicecandidate = function (event) {
      gotIceCandidate(event, peerUuid);
    };
    peerConnections[peerUuid].pc.ontrack = function (event) {
      gotRemoteStream(event, peerUuid);
    };
    peerConnections[peerUuid].pc.oniceconnectionstatechange = function (event) {
      checkPeerDisconnect(event, peerUuid);
    };
    peerConnections[peerUuid].pc.addStream(localStream);

    if (initCall) {
      peerConnections[peerUuid].pc
        .createOffer()
        .then(function (description) {
          createdDescription(description, peerUuid);
        })
        .catch(errorHandler);
    }
  },

  // Whenever a new message is received, we check if it’s intended for us and whether it’s
  // for setting a new peer or initiating the connection.
  // SDP stands for Session Description Protocol.

  gotMessageFromServer: function (message) {
    var signal = JSON.parse(message.data);
    var peerUuid = signal.uuid;

    // Ignore messages that are not for us or from ourselves
    if (
      peerUuid == localUuid ||
      (signal.dest != localUuid && signal.dest != "all")
    )
      return;

    if (signal.displayName && signal.dest == "all") {
      // set up peer connection object for a newcomer peer
      setUpPeer(peerUuid, signal.displayName);
      serverConnection.send(
        JSON.stringify({
          displayName: localDisplayName,
          uuid: localUuid,
          dest: peerUuid,
        })
      );
    } else if (signal.displayName && signal.dest == localUuid) {
      // this is sent back to user after they join server
      // initiate call if we are the newcomer peer
      setUpPeer(peerUuid, signal.displayName, true);
    } else if (signal.sdp) {
      peerConnections[peerUuid].pc
        .setRemoteDescription(new RTCSessionDescription(signal.sdp))
        .then(function () {
          // Only create answers in response to offers
          if (signal.sdp.type == "offer") {
            peerConnections[peerUuid].pc
              .createAnswer()
              .then(function (description) {
                createdDescription(description, peerUuid);
              })
              .catch(errorHandler);
          }
        })
        .catch(errorHandler);
    } else if (signal.ice) {
      peerConnections[peerUuid].pc
        .addIceCandidate(new RTCIceCandidate(signal.ice))
        .catch(errorHandler);
    }
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
            "wss://breached-webrtc.icedcoffee.dev:8888"
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
