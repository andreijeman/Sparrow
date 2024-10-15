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
            _buffer = new byte[bufferSize];
        }

        public async Task<T> ReceivePacketAsync(Socket socket)
        {
             int count = await socket.ReceiveAsync(_buffer);
             return _codec.Unpack(_buffer, 0, count);
        }

        public async Task SendPacketAsync(T data, Socket socket)
        {
            await socket.SendAsync(_codec.Pack(data));
        }

        public async Task SendPacketsAsync(T data, List<Socket> sockets)
        {
            List<Task> tasks = new List<Task>();

            foreach (Socket socket in sockets)
            {
                tasks.Add(SendPacketAsync(data, socket));
            }

            await Task.WhenAll(tasks);
        }
    }
}
