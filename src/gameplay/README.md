# Gameplay on server

For running Unity webgl game on server.


## How this project was made

- Install static-server

	```bash
	# Ref: https://www.npmjs.com/package/static-server

	# Goto the project
	cd gameplay

	# Install npm package globally
	npm -g install static-server
	
	# Run the server
	static-server --port 7001
	```


## Todolist

- Login (test acc: darkcompet@gmail.com / 1234)

	```bash
	- user enter app
		-> [game] call marketplace script to get cookie??
  	-> [marketplace] return access_token, refresh_token to game site (script required)
		-> [game] silent login to server via /api/auth/silentLogin -> get/save_pref access_token and refresh_token
			If failed -> show login require -> jump to marketplace to login.
	```

- Api doc for game (api-server, api-cardano, api-game)
https://ironsky.darkcompet.com/swagger/index.html

Authorization: Bearer access_token

- Decor (header, navigation to marketplace/login, discord, branch, copyright,...) game template around gameplay-mainboard => Thi
- Setup gameplay on server => Co
 * [future] build with unity on web
