# SimpleWebshop

A demonstration of [NServiceBus](https://particular.net/nservicebus) and the [Particular Service Platform](https://particular.net/service-platform) showing several capabilities all at once:

* NServiceBus running on .NET 8
* Ease of local development without dependencies using the [Learning Transport](https://docs.particular.net/transports/learning/)
* Running on the [RabbitMQ Transport](https://docs.particular.net/transports/rabbitmq/) hosted in docker
* Load simulation
* [Real-time monitoring](https://particular.net/real-time-monitoring) of queue length, throughput, scheduled retry rate, processing time, and critical time for each NServiceBus endpoint 

## Prerequisites

* Visual Studio/Rider
* Docker

## Running the solution locally

By default, the solution uses the [learning transport](https://docs.particular.net/transports/learning/), which is useful for demonstration and experimentation purposes. It is not meant to be used in production scenarios.

1. Open the solution in Visual Studio
1. Set the following projects as Startup projects
   * EShop.UI
   * Billing.Api
   * Marketing.Api
   * Sales.Api
   * Shipping.Api
1. Start the application

![EShop](images/HomePage.png)

Purchase one of the products and note the log messages that appear in the various endpoint consoles.

## Monitoring the endpoints

###  Set up RabbitMQ and the platform

Execute the following in the `/platform-rabbitmq` folder:

```
docker compose up -d
```

### Set the connection

Update the ITOps.Shared project to use RabbitMQ with connection string `host=localhost`

### Simulate load and monitor the results

1. In Visual Studio, run the solution. The endpoint console windows will appear as well as the web interface.
2. With the endpoints running, launch the LoadGenerator project from Visual Studio. This will launch a console application that continuously sends a PlaceOrder message once per second.
3. In a browser, navigate to ServicePulse at http://localhost:9090
4. Select _Monitoring_ in the menu bar to see statistics for the four endpoints

At this point, you can increase or decrease the load in the LoadGenerator console application with the <kbd>&uarr;</kbd> and <kbd>&darr;</kbd> keys. You can also press <kbd>S</kbd> to send a spike of 25 messages or press <kbd>P</kbd> to pause/unpause the load generator. It's useful to have this running side-by-side with ServicePulse to see the effects this has on the graphs.
