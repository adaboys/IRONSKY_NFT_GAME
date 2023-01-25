# IronSky Marketplace

Frontend server.


## Setup server

Since backend and frontend are located at same server, please complete README-log.md
of backend repo first before continue.


#### Create service for nextjs server

- Firstly, pull source code of nextjs

	```bash
	# Please config in advance to use gitlab SSH connection
	cd /var/www
	sudo mkdir tmp
	sudo chown ubuntu:ubuntu -R tmp
	cd tmp
	git clone git@gitlab.com:darkcompet-ironsky/marketplace.git
	cd ..
	sudo mv tmp/marketplace .
	sudo rm -rf tmp

	# Setup env
	cd marketplace
	git checkout develop
	mkdir local
	npm install --legacy-peer-deps
	cp .env.sample .env
	nano .env
	```

- Tell nginx points to our nextjs app

	```bash
	# Create and Fill with content-ironsky-marketplace
	sudo nano /etc/nginx/sites-available/ironsky-marketplace.config

	# Enable our site
	sudo ln -s /etc/nginx/sites-available/ironsky-marketplace.config /etc/nginx/sites-enabled/

	# Validate config grammar
	sudo nginx -t

	# Done, test by browser to below url to verify our site is working or not.
	sudo service nginx reload
	http://ironsky-marketplace.darkcompet.com/
	```

	※ content-ironsky-marketplace:

	```bash
	#The Nginx server instance
	server {
		listen 80;
		server_name ironsky-marketplace.darkcompet.com;

		location / {
			proxy_pass http://127.0.0.1:8310;
			proxy_http_version 1.1;
			proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
			proxy_set_header Upgrade $http_upgrade;
			proxy_set_header Connection 'upgrade';
			proxy_set_header Host $host;
			proxy_cache_bypass $http_upgrade;
		}

		location /admin {
			proxy_pass http://127.0.0.1:8311;
			proxy_http_version 1.1;
			proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
			proxy_set_header Upgrade $http_upgrade;
			proxy_set_header Connection 'upgrade';
			proxy_set_header Host $host;
			proxy_cache_bypass $http_upgrade;
		}
	}
	```

- Create service for nextjs server.

	```bash
	# Create new sh file with content-startServer.
	# Then make that file executable.
	nano /var/www/marketplace/local/startServer.sh
	chmod +x /var/www/marketplace/local/startServer.sh

	# Create service file with content-ironsky_marketplaceService
	sudo nano /etc/systemd/system/ironsky_marketplaceService.service

	# Enable service start when machine boots.
	# To disable, just change enable -> disable.
	sudo systemctl enable ironsky_marketplaceService

	# Reload services
	sudo systemctl daemon-reload

	# Controls service
	sudo systemctl status ironsky_marketplaceService
	sudo systemctl start ironsky_marketplaceService
	sudo systemctl stop ironsky_marketplaceService
	sudo systemctl restart ironsky_marketplaceService

	# [Optional] See service log
	journalctl --unit=ironsky_marketplaceService --follow
	journalctl --unit=ironsky_marketplaceService --since=yesterday
	journalctl --unit=ironsky_marketplaceService --since=today
	journalctl --unit=ironsky_marketplaceService --since='2020-07-29 00:00:00' --until='2020-07-29 12:00:00'
	```

	※ content-startServer:

	```bash
	cd /var/www/marketplace;
	npm run dev;
	```

	※ content of ironsky_marketplaceService: Use `ubuntu` user for ec2, `compet` user for gcloud !

	```bash
	[Unit]
	Description=Frontend for Marketplace
	After=network.target

	[Service]
	Environment=NODE_PORT=8310
	Type=simple
	User=ubuntu
	ExecStart=/bin/bash -c '/var/www/marketplace/local/startServer.sh'
	Restart=on-failure

	[Install]
	WantedBy=multi-user.target
	```

## Setup ssh for sites

- Setup ssh for marketplace site

	```bash
	# Ref: https://www.digitalocean.com/community/tutorials/how-to-secure-nginx-with-let-s-encrypt-on-ubuntu-22-04

	# Before start, assure both of the following DNS records (at google domain site) set up for own server:
	- An A record with `ironsky-marketplace.darkcompet.com` pointing to own server’s public IP address.
	- An A record with `www.ironsky-marketplace.darkcompet.com` pointing to own server’s public IP address.

	# Install certbot
	# For uninstall: sudo apt remove certbot
	sudo snap install core
	sudo snap refresh core
	sudo snap install --classic certbot
	# Above will auto mapping for us: sudo ln -s /snap/bin/certbot /usr/bin/certbot

	# Certbot needs to be able to find the correct server block in your Nginx configuration
	# for it to be able to automatically configure SSL. Specifically, it does this by looking for
	# a server_name directive that matches the domain you request a certificate for.
	sudo nano /etc/nginx/sites-available/ironsky-marketplace.config
	# Edit and Make sure config files contain:
	# server_name ironsky-marketplace.darkcompet.com www.ironsky-marketplace.darkcompet.com;

	# Check grammar and Reload nginx
	sudo nginx -t
	# If failed, maybe we use long domain name??
	# just comment out and change 64 -> 128: server_names_hash_bucket_size 128;
	# at: /etc/nginx/nginx.conf

	# Reload nginx config
	sudo service nginx reload

	# Obtain ssh cert
	sudo certbot --nginx -d ironsky-marketplace.darkcompet.com -d www.ironsky-marketplace.darkcompet.com
	# Select 1 (reinstall existing cert)
	# Select 2 (redirect all requests from http to https).

	# Make cert is auto renewal by install crontab by default
	# By default, cert is only valid for 90 days.
	sudo systemctl status certbot.timer
	# [Optional] Renew all sites manually
	sudo certbot renew --dry-run
	```
