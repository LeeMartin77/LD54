FROM docker.io/nginx
# Bad form, but not building in the container
# Just using it to host
COPY dist/web /usr/share/nginx/html
RUN cp /usr/share/nginx/html/index.html /usr/share/nginx/html/game.html 
COPY index.html /usr/share/nginx/html/index.html
