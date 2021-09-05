# Cumin API

## Notes
- ***Websocket connection uses simple token mechanism for security.***

## Bugs
- [ ] Fix dependant entity behaviour if prinicpal entity is deleted.
- [x] Connected users not saved.
	- ~Remote Ip in token and request.remoteIp does not match!~ *Removed Ip address check.*
	- ~If not valid token throw error -> reject connection.~