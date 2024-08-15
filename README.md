After checking out the code, please run the below commands from the cmd prompt.

docker-compose build

docker-compose up

After executing the above commands, two docker containers will be created.
![image](https://github.com/user-attachments/assets/00abeab3-e153-4093-9daa-d1da42358a59)

**Below are the endpoints to access the api.**

[Post] http://localhost:8080/api/auth/login

[Get] http://localhost:8080/api/Product

[Put] http://localhost:8080/api/Product

[Get] http://localhost:8080/api/Product/GetProductsByColour?colour=Blue

[Del] http://localhost:8080/api/Product?id=4

Anonymous Endpoint
[Get] http://localhost:8080/api/health

**Current Architecture**

![image](https://github.com/user-attachments/assets/fc2668f8-62f2-4e77-a47c-c019be64f706)

**TOBE - Improved Architecture using Azure**
![image](https://github.com/user-attachments/assets/0dd248e3-ae04-4d5d-b6c3-a5c223aac591)

