#!/bin/sh

mkdir ~/.ssh
echo "$CI_DEPLOY_SSH_KEY" > ~/.ssh/id_rsa
chmod 0600 ~/.ssh/id_rsa

ansible-playbook -i ansible/hosts.txt --private-key ~/.ssh/id_rsa ansible/deploy_configs.yml