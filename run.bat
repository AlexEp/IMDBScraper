docker build -f IMDBScraper.API/Dockerfile -t imdb-scraper-API
docker run -d -p 5000:80 —-name imdb-scraper-API-container imdb-scraper-API