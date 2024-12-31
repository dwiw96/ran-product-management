pg-exec:
	docker exec -it ran-product-management-pg psql -h localhost -p 5432 -U admin -d products
pg-start:
	docker start ran-product-management-pg
pg-stop:
	docker stop ran-product-management-pg

mongodb-exec:
	docker exec -it ran-product-management-mongodb mongosh -u admin -p product123
mongodb-start:
	docker container start ran-product-management-mongodb
mongodb-stop:
	docker container stop ran-product-management-mongodb
