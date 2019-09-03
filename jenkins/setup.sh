#!/bin/bash
docker image build -t jenkins-docker $(dirname $0)
docker container run -d --name jenkins -p 8080:8080 -v jenkins_home:/var/jenkins_home -v /var/run/docker.sock:/var/run/docker.sock jenkins-docker
