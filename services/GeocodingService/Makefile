APP_NAME := geocoder
BUILD_DIR := bin

.PHONY: build run test clean

build:
	@mkdir -p $(BUILD_DIR)
	@CGO_ENABLED=0 go build -ldflags="-s -w" -o $(BUILD_DIR)/$(APP_NAME) ./cmd

run:
	@go run ./cmd

clean:
	@rm -rf $(BUILD_DIR)
