docker build -t {{imagename}} -f Adventure.Api/Dockerfile
docker run -d -p 8080:80 --name aspnetcorewebapicontainer {{imagename}}