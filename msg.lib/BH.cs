using System;
using System.Net.Sockets;

namespace msg.lib {
    public class BlockHelper {
        public BlockHelper(Socket client) {
            this.client = client;
        }

        public Socket client { get; }

        public X RecieveBlock<X>(int size, X lBlock) where X : IBlock {
            lBlock.SetSize(size);
            var buffer = new byte[size];
            var l = client.Receive(buffer);
            if (l <= 0)
                throw new Exception($"expected fileSizePiece ({size} bytes)");
            lBlock.SetBody(buffer);
            return lBlock;
        }
        public (byte type, int size) RecieveHead() {
            var idBuffer = new byte[1];
            var idL = client.Receive(idBuffer);
            if (idL <= 0)
                throw new Exception("expected id (1 byte)");
            var sizeBuffer = new byte[4];
            var sizeL = client.Receive(sizeBuffer);
            if (sizeL <= 0)
                throw new Exception("expected piece size (8 bytes)");
            return (idBuffer[0], BitConverter.ToInt32(sizeBuffer, 0));
        }

        public void Send(IBlock filePiece, bool wait = true) {
            client.Send(filePiece.GetBytes());
        }

        public static bool SocketConnected(Socket s) {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }
}