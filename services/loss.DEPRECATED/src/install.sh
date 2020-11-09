 #!/bin/bash
mkdir proto

cp ../../../proto/*.proto
cp ../../../external/google 
go mod tidy
 
go install \
    github.com/grpc-ecosystem/grpc-gateway/v2/protoc-gen-grpc-gateway \
    github.com/grpc-ecosystem/grpc-gateway/v2/protoc-gen-openapiv2 \
    google.golang.org/protobuf/cmd/protoc-gen-go \
    google.golang.org/grpc/cmd/protoc-gen-go-grpc


protoc -I proto --go_out ./hello --go_opt paths=source_relative \
    --go-grpc_out ./hello --go-grpc_opt paths=source_relative \
    hello.proto

protoc -I proto --grpc-gateway_out ./hello \
    --grpc-gateway_opt logtostderr=true \
    --grpc-gateway_opt paths=source_relative \
    --grpc-gateway_opt generate_unbound_methods=true \
    proto/hello.proto

cd ./hello 

go mod init hello
go mod tidy

cd ../proxyserver
go mod init proxyserver.go
go mod tidy

go install *.go

cd ..