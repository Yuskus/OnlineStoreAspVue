@echo

docker-compose down
docker-compose build --no-cache
docker-compose up -d

cd .\onlinestore.client

docker-compose down
docker-compose build --no-cache
docker-compose up -d