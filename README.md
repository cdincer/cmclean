# cmclean
A sample application with all the features,you would want in a production project.
It's made in Clean Architecture with a DDD approach.
Use Visual Studio 2022.You don't need to anything in CLI for this one or through Desktop Docker.
Just start debugging through docker-compose option in VS, everything will be setup.
There is a sample DB creator script(InitializeDBExtension.cs) in MinimalApi for setting up records and tables.
Integration testing is done through actually starting up the whole system through xunit.
It will build every docker image in the docker-compose file you see.That's why it takes about 
a minute to start testing.

# Filter Query Testing
???

# Random testing scenarios
???