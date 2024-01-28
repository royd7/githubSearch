
# How to Build GitSearch

    cd projects
    git clone https://github.com/royd7/githubSearch.git

# How to Run GitSearch

run the server

    cd server
    cd githubSearch
    dotnet restore
    dotnet run

open browser and run http://localhost:44867/github

run the client

    cd client
    cd githubSearch
    npm i
    npm start

open browser and run http://localhost:4200/


# Or run on docker

    docker build -t githubsearch .
    docker run -p 44867:80 -p 4200:4200 githubsearch

open browser and run http://localhost:44867/github
open browser and run http://localhost:4200/



#Thanks roy dan


