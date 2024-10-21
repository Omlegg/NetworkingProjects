

IPAdress ipAdress = IPAdress.Parse("127.0.0.1")
var port = 7070

var newTcpListener = new TcpListener(ip, port)