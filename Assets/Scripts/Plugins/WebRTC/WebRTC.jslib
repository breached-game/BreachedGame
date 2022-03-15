
//from https://www.dmcinfo.com/latest-thinking/blog/id/9852/multi-user-video-chat-with-webrtc
// check if have to import WebSocket stuff

mergeInto(LibraryManager.library, {
  // set up local video stream
  function start() {
    //need to find a way to assign each client a unique ID - could use players network identity. Something like ...

    localUuid = window.unityInstance.SendMessage('NetworkManager', 'GetCurrentID');
    localDisplayName = localUuid //should have a better name here

    var constraints = {
    video: false,
    audio: true,
    };

    if (navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices.getUserMedia(constraints)
        .then(stream => {
          localStream = stream;
          console.log('Got MediaStream:', stream);
          window.unityInstance.SendMessage('MicManager', 'MicRecieved');
        }).catch(errorHandler =>{
          console.error('Error getting the mic.',errorHandler);
          window.unityInstance.SendMessage('MicManager', 'MicRejected');
        })

        // set up websocket and message all existing clients
        .then(() => {
          //serverConnection = new WebSocket('wss://' + window.location.hostname + ':' + WS_PORT);
          serverConnection = new WebSocket('wss://breached-webrtc.icedcoffee.dev'); //may need to add port
          serverConnection.onmessage = gotMessageFromServer;
          serverConnection.onopen = event => {
            serverConnection.send(JSON.stringify({ 'displayName': localDisplayName, 'uuid': localUuid, 'dest': 'all' }));
          }
        }).catch(errorHandler); =>{
          console.error('Error setting up websocket connection.', errorHandler);
        });

    } else {
      alert('Your browser does not support getUserMedia API');
    }
  }

  // Whenever a new message is received, we check if it’s intended for us and whether it’s
  // for setting a new peer or initiating the connection.
  // SDP stands for Session Description Protocol.

  function gotMessageFromServer(message) {
    var signal = JSON.parse(message.data);
    var peerUuid = signal.uuid;

    // Ignore messages that are not for us or from ourselves
    if (peerUuid == localUuid || (signal.dest != localUuid && signal.dest != 'all')) return;

    if (signal.displayName && signal.dest == 'all') {
      // set up peer connection object for a newcomer peer
      setUpPeer(peerUuid, signal.displayName);
      serverConnection.send(JSON.stringify({ 'displayName': localDisplayName, 'uuid': localUuid, 'dest': peerUuid }));

    } else if (signal.displayName && signal.dest == localUuid) {
      // this is sent back to user after they join server
      // initiate call if we are the newcomer peer
      setUpPeer(peerUuid, signal.displayName, true);

    } else if (signal.sdp) {
      peerConnections[peerUuid].pc.setRemoteDescription(new RTCSessionDescription(signal.sdp)).then(function () {
        // Only create answers in response to offers
        if (signal.sdp.type == 'offer') {
          peerConnections[peerUuid].pc.createAnswer().then(description => createdDescription(description, peerUuid)).catch(errorHandler);
        }
      }).catch(errorHandler);

    } else if (signal.ice) {
      peerConnections[peerUuid].pc.addIceCandidate(new RTCIceCandidate(signal.ice)).catch(errorHandler);

    }
  }

  // Once we have a new peer, we can add them to the peerConnections object
  // with the UUID as a key.
  function setUpPeer(peerUuid, displayName, initCall = false) {
    peerConnections[peerUuid] = { 'displayName': displayName, 'pc': new RTCPeerConnection(peerConnectionConfig) };
    peerConnections[peerUuid].pc.onicecandidate = event => gotIceCandidate(event, peerUuid);
    peerConnections[peerUuid].pc.ontrack = event => gotRemoteStream(event, peerUuid);
    peerConnections[peerUuid].pc.oniceconnectionstatechange = event => checkPeerDisconnect(event, peerUuid);
    peerConnections[peerUuid].pc.addStream(localStream);

    if (initCall) {
      peerConnections[peerUuid].pc.createOffer().then(description => createdDescription(description, peerUuid)).catch(errorHandler);
    }
  }
});
