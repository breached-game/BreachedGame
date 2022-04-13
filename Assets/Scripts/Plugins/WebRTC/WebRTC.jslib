//from https://www.dmcinfo.com/latest-thinking/blog/id/9852/multi-user-video-chat-with-webrtc
// check if have to import WebSocket stuff

var localUuid;
var localDisplayName;
var localStream;
var serverConnection;

var inWater;
var microphone;
//node constructors
var AudioContext;
var context;
var destination;
var biquadFilter;

mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello world");
  },

  // set up local video stream
  start: function () {
    localUuid = "_" + Math.random().toString(36).substring(2, 11);
    inWater = false;

    console.log("Network Identity = " + localUuid);
    localDisplayName = localUuid; //could have a better name here

    var constraints = {
      video: false,
      audio: true,
    };

    AudioContext = window.AudioContext || window.webkitAudioContext;
    context = new AudioContext();
    destination = context.createMediaStreamDestination();
    biquadFilter = context.createBiquadFilter();
    biquadFilter.type = "lowpass";
    biquadFilter.frequency.value = 300;

    if (navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices
        .getUserMedia(constraints)
        .then(function (stream) {
          if (inWater == true) {
            //setting values of the filter (causes muffled mic sound)

            microphone = context.createMediaStreamSource(stream);
            //connect filter and microphone to destination
            microphone.connect(biquadFilter);
            biquadFilter.connect(destination);
            //assign destination to local stream
            localStream = destination.stream;
          } else {
            microphone = context.createMediaStreamSource(stream);
            microphone.connect(destination);

            //standard stream
            localStream = destination.stream;
          }
          console.log("Got MediaStream:", stream);
          //window.unityInstance.SendMessage("MicManager", "MicRecieved");
        })
        .catch(function (errorHandler) {
          console.error("Error getting the mic.", errorHandler);
          //window.unityInstance.SendMessage("MicManager", "MicRejected");
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

          //attempt at Reconnect
          serverConnection.onclose = ws.onclose = function (e) {
            console.log(
              "Socket is closed. Reconnect will be attempted in 1 second.",
              e.reason
            );
            setTimeout(function () {
              start();
            }, 1000);
          };
        })
        .catch(function (errorHandler) {
          console.error("Error setting up websocket connection.", errorHandler);
        });
    } else {
      alert("Your browser does not support getUserMedia API");
    }
  },

  //Mute Mic button
  muteMic: function () {
    console.log(
      "Initial microphone state: enabled = " +
        localStream.getAudioTracks()[0].enabled
    );
    if (localStream.getAudioTracks()[0].enabled == false) {
      localStream.getAudioTracks()[0].enabled = true;
      console.log("Microphone Unmuted:");
      console.log(
        "Microphone state: enabled = " + localStream.getAudioTracks()[0].enabled
      );
    } else {
      localStream.getAudioTracks()[0].enabled = false;
      console.log("Microphone Muted:");
      console.log(
        "Microphone state: enabled = " + localStream.getAudioTracks()[0].enabled
      );
    }
  },

  waterMic: function () {
    if (inWater == false) {
      microphone.disconnect();
      //destination.disconnect(); //this may be over kill
      //biquadFilter.disconnect();

      microphone.connect(biquadFilter);
      biquadFilter.connect(destination);

      localStream = destination.stream;

      inWater = true;
      console.log("New water state = " + inWater);
    } else {
      microphone.disconnect();
      biquadFilter.disconnect();

      //destination.disconnect();
      // biquadFilter = context.createBiquadFilter();
      // biquadFilter.type = "allpass"
      // microphone.connect(biquadFilter);
      // biquadFilter.connect(destination);

      microphone.connect(destination);

      localStream = destination.stream;

      inWater = false;
      console.log("New water state = " + inWater);
    }
  },
});
