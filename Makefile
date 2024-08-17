help:
	@echo "$ make help     -- show this message"
	@echo "$ make build    -- build the project"
	@echo "$ make run      -- build and run the project"
	@echo "$ make frontend -- build the frontend"
	@echo "$ make clean    -- remove the build files"

CS_SOURCES       := $(shell find src -type f -name '*.cs') # TODO: change for windows
FRONTEND_SOURCES := $(shell find frontend/src -type f -name '*.js' -o -name '*.css') # TODO: change for windows
EXECUTABLE       := bin/Debug/net8.0/CDNApp # TODO: change for windows

.PHONY: run
run: build frontend
	dotnet run --no-build

.PHONY: build
build: $(EXECUTABLE)

$(EXECUTABLE): $(CS_SOURCES)
	dotnet build

.PHONY: frontend
frontend: frontend/build

frontend/build: $(FRONTEND_SOURCES)
	cd frontend && npm run build
	touch $@

.PHONY: clean
clean:
	dotnet clean
	rm -rf obj bin
	rm -rf frontend/build