# Cumin API

## Notes
- ***Websocket connection uses simple token mechanism for security.***

## Todo
- [x] Remove "Roadmap" concept altogether, project have epics and paths.
- [x] Throw relevant custom Exceptions from dbServices.
	- [x] Forward the error as Response.
	- [ ] Middleware also throw Relevant error.
- [ ] Check if all the entities' primary key are auto-generated.

## Bugs
- [x] Fix dependant entity behaviour if prinicpal entity is deleted.
- [x] Connected users not saved.
	- ~Remote Ip in token and request.remoteIp does not match!~ *Removed Ip address check.*
	- ~If not valid token throw error -> reject connection.~