FROM kong:2.1-centos

USER root

RUN mkdir /etc/kong/ -p
RUN mkdir /setup/

COPY services/kong-setup/config.yml /etc/kong/
COPY services/kong-setup/setup.sh /setup/


USER kong

CMD sh -c /setup/setup.sh


#CMD [ "kong", "restart"]

#CMD [ "cd", "/home/kong/proto"]
#CMD [ "go", "get", "google.golang.org/genproto/googleapis/api/annotations"]
#CMD [ "git", "clone", "https://github.com/googleapis/googleapis"]
#CMD [ "protoc", "-I", "googleapis", "-I", ".", "./*.proto"]


