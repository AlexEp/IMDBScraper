#docker-compose up -d
docker build -f IMDBScraper.API/Dockerfile -t imdb-scraper-api .
docker run -d -p 5000:80 --name imdb-scraper-api-container imdb-scraper-api