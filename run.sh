#! /bin/bash
set -e
./build.sh
docker run -p 80:80 hjerpbakk/image-gallery