Test task for Kaspelan. Microservices (Gateway, Saga, External config, ServicesDiscovery)

For testing used Docker-Desktop: Current version: 4.33.1 (161083)

For start open docker folder and enter command
docker-compose up

Docker has 4 open ports:
http://localhost:8761/ - for Eureka dashboard.
http://localhost:15672/ - for RabbitMQ dashboard.
http://localhost:8080(81)/ - gateway.

Gateway has 3 entry point:
POST http://localhost:8080/api/auth - authenticate.
POST http://localhost:8080/api/v1/order - create order.
GET http://localhost:8080/api/v1/order/{orderID} - get order.

Workflow:
1) POST http://localhost:8080/api/auth with body { "email": "email@email.email",  "password": "password"} for authenticate.
2) Use result as "Authenticate: Bearer {token} and call POST http://localhost:8080/api/v1/order with body { "details": "test details 123" } and save id from result.
3) Looks in the notification service log and find something like:
  info: TestTaskKaspelan.Notification.Repositories.Repositories.EmailRepository[0]
  Sent with message: OrderID# {Your ID from result} sent.
4) Congrats! Finish!


There are still some things that could be improved and tidied up, but this should be enough for demonstration purposes.



