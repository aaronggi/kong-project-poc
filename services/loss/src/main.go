
// Package main implements a server for Greeter service.
package main

import (
	
	"context"
	"log"
	"net"
	"time"

	"google.golang.org/grpc"
	pb "hello"

	"google.golang.org/grpc/reflection"
//      "google.golang.org/genproto/googleapis/api/annotations"
)

const (
        port = ":8090"
)

// server is used to implement helloworld.GreeterServer.
type server struct {
        pb.UnimplementedHelloServiceServer
}

// SayHello implements helloworld.GreeterServer
func (s *server) SayHello(ctx context.Context, in *pb.HelloRequest) (*pb.HelloResponse, error) {
        log.Printf("Received: %v", in.GetGreeting())
        return &pb.HelloResponse{Reply: "Hello!! " + in.GetGreeting()}, nil
}

func main() {
        lis, err := net.Listen("tcp", port)
        if err != nil {
                log.Fatalf("failed to listen: %v", err)
        }
        s := grpc.NewServer()
        reflection.Register(s)
        pb.RegisterHelloServiceServer(s, &server{})
        if err := s.Serve(lis); err != nil {
                log.Fatalf("failed to serve: %v", err)
		}
		
		for(true){
			log.Printf("ok");
			time.Sleep(10000);
		}
}