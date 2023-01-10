#!/bin/sh

mkdir ~/.ssh
echo "$CI_DEPLOY_SSH_KEY" > ~/.ssh/id_rsa
chmod 0600 ~/.ssh/id_rsa

ansible-playbook -i ansible/hosts.txt \
--extra-vars "GITHUB_WORKSPACE=$GITHUB_WORKSPACE DOPPLER_TOKEN=$DOPPLER_TOKEN DOCKER_USERNAME=$DOCKER_USERNAME IMAGE_NAME=$IMAGE_NAME IMAGE_VERSION=$IMAGE_VERSION" \
--private-key ~/.ssh/id_rsa $INPUT_PLAYBOOKPATH