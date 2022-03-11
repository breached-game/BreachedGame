mergeInto(LibraryManager.library, {

    Hello: function () {
      window.alert("Hello, world!");
    },
    //Ask the user for permission to use their microphone (don't inclue '=>' in code when defining a function or an error will occur)
    RequestMic: function () { 
      navigator.mediaDevices.getUserMedia({video: false, audio: true}).catch( function(err){
          console.log("u got an error:" + err)
      });
      window.unityInstance.SendMessage('MicManager', 'MicRecieved');
    },
    
  });