package main

import (
	"github.com/gin-gonic/gin"
	"os"
)

func getPort() string {
	port := ":8080"
	if val, ok := os.LookupEnv("FUNCTIONS_CUSTOMHANDLER_PORT"); ok {
		port = ":" + val
	}
	return port
}

func main() {
	r := gin.Default()
	r.GET("/api/ping", func(c *gin.Context) {
		c.JSON(200, gin.H{
			"message": "pong",
		})
	})

	// Gin will listen on 0.0.0.0:8080 unless a PORT environment variable specifies a port to
	// use. See the readme for details on FUNCTIONS_CUSTOMHANDLER_PORT
	r.Run(getPort())
}
