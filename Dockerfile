FROM debian:latest
RUN apt-get update
RUN apt-get install -y wget
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O /root/packages-microsoft-prod.deb
RUN dpkg -i /root/packages-microsoft-prod.deb
RUN rm /root/packages-microsoft-prod.deb
RUN apt-get update
RUN apt-get install -y dotnet-sdk-7.0
RUN mkdir /root/code
ADD . /root/code
EXPOSE 7163 5163
CMD if [ "$ENV" = "test" ]; then (cd /root/code/Stereograph.TechnicalTest.Tests && dotnet test); else (cd /root/code/Stereograph.TechnicalTest.Api && dotnet run); fi
