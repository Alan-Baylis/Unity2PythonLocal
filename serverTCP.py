import socket
import numpy as np


HOST = 'localhost' 
PORT = 8000           
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)
c, addr = s.accept()


while True:
    print 'Got connection from', addr
    c.send('Thank you for your connecting: ')
    data = c.recv(196608)
    if not data: break
    print(np.fromstring(data, dtype=np.uint8))
