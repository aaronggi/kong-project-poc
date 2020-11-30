FROM kong:2.1-centos

USER root

RUN mkdir /etc/kong/ -p
RUN mkdir /setup/

COPY services/kong-setup/config.yml /etc/kong/
COPY services/kong-setup/setup.sh /setup/

USER kong

CMD sh -c /setup/setup.sh

