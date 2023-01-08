FROM alpine 

ENV ANSIBLE_HOST_KEY_CHECKING=False
ENV PIP_ROOT_USER_ACTION=ignore

RUN apk add ansible gcc python3-dev py3-pip libc-dev libffi-dev openssl-dev
RUN pip3 install --upgrade paramiko
COPY entrypoint.sh /entrypoint.sh
RUN chmod 777 /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]