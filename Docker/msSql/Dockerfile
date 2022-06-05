FROM mcr.microsoft.com/mssql/server:2019-latest

ENV SA_PASSWORD Password13579999
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express

# RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY . /usr/src/app

CMD /bin/bash ./entrypoint.sh
