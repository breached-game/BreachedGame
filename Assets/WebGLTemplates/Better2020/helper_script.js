//Inspired by https://www.dmcinfo.com/latest-thinking/blog/id/9852/multi-user-video-chat-with-webrtc

/*
VOICE CONNECTIONS AND COMMUNTICATIONS MANAGER
Contributors: Daniel Savidge and Luke Benson 
*/

//Key is uuid, values are peer connection object and user defined display name string
var peerConnections = {};

const peerConnectionConfig = {
  //Defines server to send ICE candidates
  iceServers: [
    {
      urls: "turn:breached-coturn.icedcoffee.dev:7777",
      username: "test123",
      credential: "test",
    },
    //{ urls: "stun:stun.services.mozilla.com" },
  ],
};

//Deals with messages from peers via the signalling server
function gotMessageFromServer(message) {
  var signal = JSON.parse(message.data);
  var peerUuid = signal.uuid;

  //Ignore messages that are not for us or from ourselves
  if (
    peerUuid == localUuid ||
    (signal.dest != localUuid && signal.dest != "all")
  )
    return;

  if (signal.displayName && signal.dest == "all") {
    //Set up peer connection object for a newcomer peer
    setUpPeer(peerUuid, signal.displayName);
    serverConnection.send(
      JSON.stringify({
        displayName: localDisplayName,
        uuid: localUuid,
        dest: peerUuid,
      })
    );
  } else if (signal.displayName && signal.dest == localUuid) {
    //This is sent back to user after they join server AND initiates call if we are the newcomer peer
    setUpPeer(peerUuid, signal.displayName, true);
  } else if (signal.sdp) {
    //Deal with offers to establish peer connection
    peerConnections[peerUuid].pc
      .setRemoteDescription(new RTCSessionDescription(signal.sdp))
      .then(function () {
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
}
// Once we have a new peer, we can add them to the peerConnections object with the UUID as a key.
function setUpPeer(peerUuid, displayName, initCall = false) {
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
}

//Checks for disconnection
function checkPeerDisconnect(event, peerUuid) {
  var state = peerConnections[peerUuid].pc.iceConnectionState;
  console.log(`connection with peer ${peerUuid} ${state}`);
  //Sends status update through unity captain command line
  window.unityInstance.SendMessage(
    "MicManager",
    "PrintMsgCaptain",
    String(state)
  );
  if (state === "failed" || state === "closed" || state === "disconnected") {
    delete peerConnections[peerUuid];
  }
}

//Creates html elements to assign incoming peer audio
function gotRemoteStream(event, peerUuid) {
  console.log(`got remote stream, peer ${peerUuid}`);
  console.log(event.streams[0]);
  var audioElement = document.createElement("audio");
  audioElement.setAttribute("autoplay", "");
  audioElement.setAttribute("id", peerUuid);
  audioElement.srcObject = event.streams[0];
  document.getElementsByTagName("body")[0].appendChild(audioElement);
}

//Forwards on ICE candidates to other peers
function gotIceCandidate(event, peerUuid) {
  if (event.candidate != null) {
    serverConnection.send(
      JSON.stringify({ ice: event.candidate, uuid: localUuid, dest: peerUuid })
    );
  }
}

//Sets up description for ICE candidates
function createdDescription(description, peerUuid) {
  console.log(`got description, peer ${peerUuid}`);
  peerConnections[peerUuid].pc
    .setLocalDescription(description)
    .then(function () {
      serverConnection.send(
        JSON.stringify({
          sdp: peerConnections[peerUuid].pc.localDescription,
          uuid: localUuid,
          dest: peerUuid,
        })
      );
    })
    .catch(errorHandler);
}

//Error handler
function errorHandler(error) {
  console.log(error);
}
