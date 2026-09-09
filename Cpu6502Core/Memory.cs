namespace Cpu6502Core
{
    public class Memory
    {
        private readonly byte[] _mem = new byte[65536];
        private readonly object _lock = new object();

        public byte Read(ushort address)
        {
            lock (_lock)
            {
                return _mem[address];
            }
        }

        public void Write(ushort address, byte value)
        {
            lock (_lock)
            {
                _mem[address] = value;
            }
        }

        public void LoadProgram(byte[] program, ushort startAddress)
        {
            lock (_lock)
            {
                Array.Copy(program, 0, _mem, startAddress, program.Length);
            }
        }

        public ushort RWORD(ushort address)
        {
            byte lowByte = Read(address);
            byte highByte = Read((ushort)(address + 1));
            return (ushort)((highByte << 8) | lowByte);
        }
    }
}