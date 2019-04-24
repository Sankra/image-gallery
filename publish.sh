#! /bin/bash
set -e
./build.sh
# az login
az acr login --name hjerpbakk
docker tag hjerpbakk/image-gallery:latest hjerpbakk.azurecr.io/hjerpbakk/image-gallery
docker push hjerpbakk.azurecr.io/hjerpbakk/image-gallery
