/*
SIGNALLING SERVER USED TO FORWARD MESSAGES TO CLIENTS
Contributors: Daniel Savidge and Luke Benson 
*/


var https = require('https');
var fs = require('fs');
var WebSocket = require('ws');

var express = require('express');
var expressWs = require('express-ws');

const certDir = `/etc/letsencrypt/live`;
const domain = `breached-webrtc.icedcoffee.dev`;


//Using certficates to authenticate server
const options = {
  key: fs.readFileSync(`${certDir}/${domain}/privkey.pem`),
  cert: fs.readFileSync(`${certDir}/${domain}/fullchain.pem`),
};


//Using express to create secure websocket server
var app = express();
var server = https.createServer(options, app);
var expressWs = expressWs(app, server);
var aWss = expressWs.getWss('/');

app.use(function (req, res, next) {
  req.testing = 'testing';
  return next();
});

app.get('/', function(req, res, next){
  console.log('get route', req.testing);
  res.end();
});

app.ws('/', function(ws, req) {
  console.log("Connection Created");
  console.log("There are "+aWss.clients.size+" active clients");

  ws.on("message", function (message) {
    // Broadcast any received message to all clients
    console.log("received: %s", message);
    ws.broadcast(message);
  });

  ws.on("error", (e) => console.log("there has been an error"+e) && ws.terminate());

  ws.broadcast = function (data) {
    console.log("in broadcast function");
    aWss.clients.forEach(function (client) {
      if (client.readyState === WebSocket.OPEN) {
        client.send(data);
      }
    });
  };
});


server.listen(7777);
