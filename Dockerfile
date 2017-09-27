FROM microsoft/aspnetcore:2.0.0

RUN apt-get update \ 
	&& apt-get install -y gcc \
	&& cd /root \
	&& curl https://codeload.github.com/cupsos/oj_mallocfree/tar.gz/master \
	| tar -xz --strip=1

WORKDIR /root/build

CMD ["dotnet","oj_mallocfree.dll"]
