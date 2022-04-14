tag = latest
name_container = app-deposit
network_docker = micro
port_out_host = 9003
name_image = icuadrosaforo255/img-deposit

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