namespace Cpu6502Core
{
    public static class AddressingModes
    {
        // Immediate: значение находится прямо по адресу PC
        public static byte GetImmediate(Memory memory, ref ushort pc)
        {
            return memory.Read(pc++);
        }

        // Zero Page: адрес в пределах первых 256 байт (0x00xx)
        public static ushort GetZeroPage(Memory memory, ref ushort pc)
        {
            return memory.Read(pc++);
        }

        // Absolute: полный 16-битный адрес (Little Endian)
        public static ushort GetAbsolute(Memory memory, ref ushort pc)
        {
            ushort low = memory.Read(pc++);
            ushort high = memory.Read(pc++);
            return (ushort)((high << 8) | low);
        }
    }
}