//Inspired by https://www.dmcinfo.com/latest-thinking/blog/id/9852/multi-user-video-chat-with-webrtc

/*
CONTAINS WEBRTC JAVASCRIPT FUNCTIONS THAT ARE ACCESSIBLE FROM UNITY. INCLUDING VOICE EFFECTS AND MUTE MICROPHONE FUNCTIONALITY.
THIS FILE USES HELPER JAVASCRIPT FUNCTIONS WHICH IS REFERENCED IN OUR index.html FILE FOR HANDLING CONNECTIONS AND COMMUNTICATIONS.   
Contributors: Daniel Savidge and Luke Benson 
*/

var localUuid;
var localDisplayName;
var localStream;
var serverConnection;

var inWater;
//Node constructors
var microphone;
var gainNode;
var AudioContext;
var context;
var destination;
var biquadFilter;

mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello world");
  },

  //Initialises voice connection
  start: function () {
    //Sets up users id
    localUuid = "_" + Math.random().toString(36).substring(2, 11);
    inWater = false;

    console.log("Network Identity = " + localUuid);
    localDisplayName = localUuid;

    //Specifies we only need uses audio input
    var constraints = {
      video: false,
      audio: true,
    };

    //Created audio context so audio nodes can be defined and implemented
    AudioContext = window.AudioContext || window.webkitAudioContext;
    context = new AudioContext();
    destination = context.createMediaStreamDestination();
    //Defines filter node (muffles microphone)
    biquadFilter = context.createBiquadFilter();
    biquadFilter.type = "lowpass";
    biquadFilter.frequency.value = 400;
    //Defines gain node (volume)
    gainNode = context.createGain();
    gainNode.gain.value = 1.5;

    if (navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices
        .getUserMedia(constraints)
        .then(function (stream) {
          if (inWater == true) {
            microphone = context.createMediaStreamSource(stream);
            //Connect filter and microphone to destination
            microphone.connect(gainNode);
            gainNode.connect(biquadFilter);
            biquadFilter.connect(destination);
            //Assign destination to local stream
            localStream = destination.stream;
          } else {
            microphone = context.createMediaStreamSource(stream);
            microphone.connect(destination);
            localStream = destination.stream;
          }
          console.log("Got MediaStream:", stream);
        })
        .catch(function (errorHandler) {
          console.error("Error getting the mic.", errorHandler);
        })
        .then(function () {
          //Checks microphone permission is allowed
          if (microphone == null) {
            return;
          }
          //Set up websocket and message all existing clients
          serverConnection = new WebSocket(
            "wss://breached-webrtc.icedcoffee.dev:7777"
          );
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

          //Reconnects client on disconnection
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

  //Changes users microphone state to and from muted/unmuted
  muteMic: function () {
    if (microphone == null) {
      return;
    }
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

  //Applies water muffled effect on users voice
  waterMicOn: function () {
    if (microphone == null) {
      return;
    }
    microphone.disconnect();

    microphone.connect(gainNode);
    gainNode.connect(biquadFilter);
    biquadFilter.connect(destination);

    localStream = destination.stream;
  },

  //Disables water muffled effect on users voice
  waterMicOff: function () {
    if (microphone == null) {
      return;
    }
    microphone.disconnect();
    biquadFilter.disconnect();
    gainNode.disconnect();

    microphone.connect(destination);

    localStream = destination.stream;

    inWater = false;
    console.log("New water state = " + inWater);
  },
});
