FROM kong:2.1-centos

#make the protos directory
RUN mkdir /home/kong/proto/ -p

USER root

RUN mkdir -p /usr/
#install a basic text editor
RUN yum -y install nano

#drop back to kong user
ADD proto /usr/local/kong/

RUN chown kong /usr/local/share/lua/5.1/protoc.lua
RUN chmod 777 /usr/local/share/lua/5.1/protoc.lua

USER kong

COPY services/kong/config.yml /usr/local/kong/
COPY external/googleapis/google /usr/local/kong/google
COPY external/kong-plugin-grpc-gateway/kong /usr/local/custom/kong

#CMD ["kong", "start", "-c", "/usr/local/kong/config.yml"]
#CMD [ "kong", "restart"]

#CMD [ "cd", "/home/kong/proto"]
#CMD [ "go", "get", "google.golang.org/genproto/googleapis/api/annotations"]
#CMD [ "git", "clone", "https://github.com/googleapis/googleapis"]
#CMD [ "protoc", "-I", "googleapis", "-I", ".", "./*.proto"]


