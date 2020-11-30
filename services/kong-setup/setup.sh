#!/bin/sh

kong migrations bootstrap --vv;
kong config db_import /etc/kong/config.yml