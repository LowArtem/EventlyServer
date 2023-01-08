FROM alpine 

ENV ANSIBLE_HOST_KEY_CHECKING=False

RUN adduser -D pipuser
USER pipuser
WORKDIR /home/pipuser

RUN apk add ansible gcc python3-dev py3-pip libc-dev libffi-dev openssl-dev
RUN pip3 install --upgrade paramiko
COPY entrypoint.sh /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]