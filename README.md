1) start docker

2) open powershell

3) docker build -t mailapi .

3) docker run -it --rm -p 80:80 --name ff_mail_api mailapi

4) now API run on http://localhost:80/api/Mail