using System.Net.Sockets;

namespace NetworkSocket
{
    public class Postman<T>
    {
        private ICodec<T> _codec;
        private byte[] _buffer;

        public Postman(ICodec<T> codec, int bufferSize)
        {
            _codec = codec;
            _buffer = new byte[1024];
        }

        public async Task<T> ReceivePacketAsync(Socket socket)
        {
             int count = await socket.ReceiveAsync(_buffer);
             return _codec.Unpack(_buffer, 0, count);
        }

        private async Task SendPacketAsync(T data, Socket socket)
        {
            await socket.SendAsync(_codec.Pack(data));
        }
    }
}
