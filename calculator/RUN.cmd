ECHO ON

SET PYTHONPATH=%PYTHONPATH%;..\..\krypl-project

waitress-serve --port=8000 server:api

PAUSE