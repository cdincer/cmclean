# cmclean
A sample application with all the features, you would want in a production project.
It's made in Clean Architecture with a DDD approach. You can deploy this to cloud 
or to a kubernetes cluster just make sure that it has more than 4GB's of RAM per node.
Use Visual Studio 2022, you don't need to do anything in CLI for this one or through Desktop Docker.
Just start debugging through docker-compose option in VS, everything will be setup.
There is a sample DB creator script(InitializeDBExtension.cs) in MinimalApi for setting up records and tables.
Integration testing is done through actually starting up the whole system through xunit.
It will build every docker image in the docker-compose file you see.That's why it takes about 
a minute to start testing. It will compose-down after it's done testing.
