help:
	@echo "make help  -- show this message"
	@echo "make build -- build the project"
	@echo "make run   -- build and run the project"
	@echo "make clean -- remove the build files"

.PHONY: run
run: build frontend
	dotnet run

.PHONY: build
build:
	dotnet build

.PHONY: frontend
frontend:
	cd frontend && npm run build

.PHONY: clean
clean:
	dotnet clean
	rm -rf obj bin