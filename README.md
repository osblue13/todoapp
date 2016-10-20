# TodoApp

A Todo MVC app created in .Net Core with a docker file to host it in a docker container. 
It uses Auth0 to authenticate users and call the web api (https://github.com/osblue13/todoapicore) to get the list of items in the todo list. (which requires authentication)


# Getting started

    cd src/TodoApp

## Easy way

    docker-compose up
This will build the images and bring up the containers.
