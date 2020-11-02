module hello

go 1.13

replace "hello" => ./hello

require (
	github.com/golang/protobuf v1.4.3
	github.com/grpc-ecosystem/grpc-gateway/v2 v2.0.1
	google.golang.org/genproto v0.0.0-20201029200359-8ce4113da6f7
	google.golang.org/grpc v1.33.1
	google.golang.org/protobuf v1.25.0
)
