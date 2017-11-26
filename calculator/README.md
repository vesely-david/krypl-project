# Calculator

Swagger server side for calculation of technical indicators. The API is documented in
swagger definition calculatorAPI.yaml.

## How To Run

The server can be run by using any WSGI server, such as uWSGI or Gunicorn.
The only thing need is to add ../../krypl-project to PYTHONPATH.

### Unix

```
guicorn server:api
```

### Windows

```
waitress-serve --port=8000 server:api
```