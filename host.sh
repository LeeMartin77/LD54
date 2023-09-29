#!/bin/sh

podman run -it --rm -p 8080:80 -v ./dist/web:/usr/share/nginx/html:ro docker.io/nginx