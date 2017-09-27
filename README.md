# oj_mallocfree
C언어 소스코드와 STDIN를 웹페이지에서 입력하고 메모리 동적할당(malloc, free)의 호출을 체크합니다. 

## 의존성
### Native
bash gcc .NET core
### Docker
docker

## 사용법
### Native
```bash
git clone https://github.com/cupsos/oj_mallocfree
cd oj_mallocfree
dotnet run
```
### Docker
* 이미지 빌드
```bash
docker build --tag oj_mallocfree https://raw.githubusercontent.com/cupsos/oj_mallocfree/master/Dockerfile
```
* 실행
```bash
docker run --name oj_mallocfree -p 127.0.0.1:5000:5000 --rm=true -i -t oj_mallocfree
```

## 주의사항
C 소스코드를 인증없이 웹으로 받아서 컴파일 후 실행하므로, Native로 구동할 시에는 더욱 주의가 필요합니다.  
Native로 구동 시에는 Program.cs를 수정하여 외부IP 접근 불가능하게 하거나, 방화벽에서 TCP:5000 인바운드를 필히 차단하시기 바랍니다. 
