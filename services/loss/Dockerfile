FROM golang:1.13

# install protobuf
ENV PB_VER 3.10.1
ENV PB_URL https://github.com/google/protobuf/releases/download/v${PB_VER}/protoc-${PB_VER}-linux-x86_64.zip

RUN apt-get -qq update && apt-get -qqy install curl git make unzip gettext rsync

RUN mkdir -p /tmp/protoc && \
    curl -L ${PB_URL} > /tmp/protoc/protoc.zip && \
    cd /tmp/protoc && \
    unzip protoc.zip && \
    cp /tmp/protoc/bin/protoc /usr/local/bin && \
    cp -R /tmp/protoc/include/* /usr/local/include && \
    chmod go+rx /usr/local/bin/protoc && \
    cd /tmp && \
    rm -r /tmp/protoc

# Get the source from GitHub
RUN go get -u google.golang.org/grpc

# Install protoc-gen-go
RUN go get -u github.com/golang/protobuf/protoc-gen-go

ENV GO111MODULE=on

USER root
RUN mkdir /src/proto -p


#COPY ./external/googleapis/google ./proto/google
COPY ./services/loss/src $GOPATH/src

RUN cd $GOPATH/src && \
    go install \
    github.com/grpc-ecosystem/grpc-gateway/v2/protoc-gen-grpc-gateway \
    github.com/grpc-ecosystem/grpc-gateway/v2/protoc-gen-openapiv2 \
    github.com/grpc-ecosystem/grpc-gateway/v2/utilities \
    github.com/grpc-ecosystem/grpc-gateway/v2/runtime \ 
    google.golang.org/protobuf/cmd/protoc-gen-go \
    google.golang.org/grpc/cmd/protoc-gen-go-grpc && \
    go get google.golang.org/genproto/googleapis/api/annotations
#RUN git clone https://github.com/googleapis/googleapis
EXPOSE 12001

#RUN protoc -I googleapis -I . ./*.proto
RUN cd $GOPATH/src/hello && go mod tidy && go install .
RUN cd $GOPATH/src/ && go mod tidy && go mod download && go mod graph
RUN cd $GOPATH/src/ && go build $GOPATH/src/main.go
CMD go run $GOPATH/src/main.go
#& && go run /proxyserver/proxyserver.go


