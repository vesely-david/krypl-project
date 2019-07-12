"use strict";

process.title = 'node-chat';
var webSocketsServerPort = 1337;
var webSocketServer = require('websocket').server;
var uuid = require('node-uuid');
var http = require('http');
var sub = require("redis").createClient({
  host: 'redis',
  port: 6379
});

// list of currently connected clients (users)
var clientChannels = {};

var server = http.createServer(function(request, response) {} ); 

server.listen(webSocketsServerPort, function() {
  console.log((new Date()) + " Server is listening on port " + webSocketsServerPort);
});

var wsServer = new webSocketServer({
  httpServer: server
});

sub.subscribe('1min');
sub.subscribe('30min');

sub.on("message", function (channel, message) {
  if(clientChannels[channel]) clientChannels[channel].forEach(o => o.sendUTF(
    `{ "channel": "${channel}", "data": ${message} }`
  ))
});

wsServer.on('request', function(request) {
  console.log('New client connected');

  var connection = request.accept(null, request.origin); 
  var id = uuid.v4();
  connection.id = id;
  var subsAccepted = false;

  connection.on('message', function(message) {
    if (message.type === 'utf8') {
      let channels;
      try {
        channels = JSON.parse(message.utf8Data);
      } catch (e) {}

      if(channels && channels.length > 0 && !subsAccepted){
        subsAccepted = true
        channels.forEach(o => {
          if(!clientChannels[0]) clientChannels[o] = [];
          clientChannels[o].push(connection)
        });
      }
    }
  });

  connection.on('close', function() {
    console.log('Client disconnected');
    Object.keys(clientChannels).forEach(o => {
      clientChannels[o] = clientChannels[o].filter(o => o.id !== id);
    })
  });
});
console.log('Server started');