## Instalacion de Makefile, primero instale el chocolate 
## https://chocolatey.org/install
## https://community.chocolatey.org/packages/make
## https://makefiletutorial.com/
##
## Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
## choco install make

## Para correr solo se ubica en la carpeta donde esta el archivo y da Make mas el comando a realizar 

tag = latest
name_container = app-security
network_docker = micro
port_out_host = 9004
name_image = dach092/img-security

delete: rm rmi
docker: build start

run:
		dotnet run
restore:
		dotnet restore
compile:
		dotnet build

build:
		docker build -t $(name_image):$(tag) .		
start:
		docker run --name=$(name_container) --network=$(network_docker) -d -p $(port_out_host):80 $(name_image):$(tag)
rm: 
		docker rm -f $(name_container)
rmi: 
		docker rmi -f $(name_image):$(tag)
push: 
		docker push $(name_image):$(tag)
