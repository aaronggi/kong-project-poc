module main

go 1.13

replace hello => ./hello

require (
	github.com/grpc-ecosystem/grpc-gateway/v2 v2.0.1
	google.golang.org/api v0.30.0
	google.golang.org/grpc v1.33.1
	google.golang.org/grpc/cmd/protoc-gen-go-grpc v1.0.1
	google.golang.org/protobuf v1.25.0
	hello v0.0.0-00010101000000-000000000000
)
