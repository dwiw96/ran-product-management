pg-exec:
	docker exec -it ran-products-management-net-pg psql -h localhost -p 5432 -U products -d products
pg-stop:
	docker container stop ran-products-management-net-pg

migrate-create:
	migrate create -ext sql -dir internal/migrations -seq init
migrate-up:
	migrate -path internal/migrations -database "postgresql://product:product@localhost:5442/product?sslmode=disable" -verbose up $(v)
migrate-down:
	migrate -path internal/migrations -database "postgresql://product:product@localhost:5442/product?sslmode=disable" -verbose down $(v)
migrate-force:
	migrate -path internal/migrations -database "postgresql://product:product@localhost:5442/product?sslmode=disable" force $(v)
