#!/bin/sh

mkdir -v -p ~/.local/share/godot/templates
mv /root/.local/share/godot/templates/3.5.2.stable.mono ~/.local/share/godot/templates/3.5.2.stable.mono
mkdir -v -p /build/build/web
godot -v --export "HTML5" /build/build/web/index.html