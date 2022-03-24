// Whenever a new message is received, we check if it’s intended for us and whether it’s
// for setting a new peer or initiating the connection.
// SDP stands for Session Description Protocol.
function gotMessageFromServer(message) {
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
}
// Once we have a new peer, we can add them to the peerConnections object
// with the UUID as a key.
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

// New imported function
function checkPeerDisconnect(event, peerUuid) {
  var state = peerConnections[peerUuid].pc.iceConnectionState;
  console.log(`connection with peer ${peerUuid} ${state}`);
  if (state === "failed" || state === "closed" || state === "disconnected") {
    delete peerConnections[peerUuid];
    //document.getElementById('videos').removeChild(document.getElementById('remoteVideo_' + peerUuid));7
    //We need to handle voice here in unity
  }
}

function gotRemoteStream(event, peerUuid) {
  console.log(`got remote stream, peer ${peerUuid}`);
  //assign stream to new HTML video element
  //var vidElement = document.createElement('video');
  //vidElement.setAttribute('autoplay', '');
  //vidElement.setAttribute('muted', '');
  //vidElement.srcObject = event.streams[0];

  //var vidContainer = document.createElement('div');
  //vidContainer.setAttribute('id', 'remoteVideo_' + peerUuid);
  //vidContainer.setAttribute('class', 'videoContainer');
  //vidContainer.appendChild(vidElement);
  //vidContainer.appendChild(makeLabel(peerConnections[peerUuid].displayName));

  //document.getElementById('videos').appendChild(vidContainer);
  // something like UnityInstance.SendMessage(MicManager,initiliase_other_player_voice,event.streams[0])
  // or
  // deal with audio in javascript and play outloud
}

function gotIceCandidate(event, peerUuid) {
  if (event.candidate != null) {
    serverConnection.send(
      JSON.stringify({ ice: event.candidate, uuid: localUuid, dest: peerUuid })
    );
  }
}

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
