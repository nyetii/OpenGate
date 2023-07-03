# OpenGate

### How to run docker-compose.yml
```console
docker-compose -p "opengate-access-manager" -f docker-compose.yml up
```

### How to rebuild and deploy in dettached mode
```console
docker-compose.exe -p "opengate-access-manager" -f docker-compose.yml up --build -d
```

### How to rebuild OpenGate changes only
```console
docker-compose.exe -p "opengate-access-manager" -f docker-compose.yml up --build --force-recreate opengate -d
```