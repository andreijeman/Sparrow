namespace Network
{
    public interface ICodec<T>
    {
        public byte[] Pack(T packet);
        public T Unpack(byte[] data, int index, int count);
    }
}
