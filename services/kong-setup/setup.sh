#!/bin/sh

kong migrations bootstrap &&\
kong config db_import /etc/kong/config.yml