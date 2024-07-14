# SimpleServiceDiscovery
Examples of using different gRPC resolvers and load balancers.

## Discovery.Service
A simple discovery service with periodic checking of the availability of registered services.

Swagger: https://localhost:7801/swagger/index.html

## Example.Service
A service that allows you to call gRPC methods.
Also provides methods for enabling and disabling availability, without shutting down.

`dotnet run --port 7002 --useDiscovery false`

Command line arguments (optional):<br>
 `--port 7001` - service port (dafault 7001, use 0 for random port)<br>
 `--name Example.Service` - name used in service discovery (default Example.Service)<br>
 `--useDiscovery true` - determines whether the start and end of the service is reported to discovery service (default appsettings UseServiceDiscovery value or true)

Healthcheck: https://localhost:7001/hc<br>
Swagger: https://localhost:7001/swagger/index.html

## Example.Client
A console app calling gRPC methods with various resolver and load balancer configurations.

## Example.Shared
Shared contracts and interfaces