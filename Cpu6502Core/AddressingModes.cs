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

        // Zero Page, X: адрес в Zero Page со смещением на регистр X (с переполнением в пределах 0-255)
        public static ushort GetZeroPageX(Memory memory, ref ushort pc, byte x)
        {
            byte baseAddr = memory.Read(pc++);
            return (byte)(baseAddr + x);
        }

        // Absolute: полный 16-битный адрес (Little Endian)
        public static ushort GetAbsolute(Memory memory, ref ushort pc)
        {
            ushort low = memory.Read(pc++);
            ushort high = memory.Read(pc++);
            return (ushort)((high << 8) | low);
        }

        // Absolute, X: полный 16-битный адрес со смещением на регистр X
        public static ushort GetAbsoluteX(Memory memory, ref ushort pc, byte x)
        {
            ushort abs = GetAbsolute(memory, ref pc);
            return (ushort)(abs + x);
        }

        // Absolute, Y: полный 16-битный адрес со смещением на регистр Y
        public static ushort GetAbsoluteY(Memory memory, ref ushort pc, byte y)
        {
            ushort abs = GetAbsolute(memory, ref pc);
            return (ushort)(abs + y);
        }

        // Indexed Indirect (Indirect, X): (zp + X) читается из Zero Page как 16-битный указатель
        public static ushort GetIndirectX(Memory memory, ref ushort pc, byte x)
        {
            byte baseAddr = memory.Read(pc++);
            byte ptr = (byte)(baseAddr + x);
            ushort low = memory.Read(ptr);
            ushort high = memory.Read((byte)(ptr + 1));
            return (ushort)((high << 8) | low);
        }

        // Indirect Indexed (Indirect, Y): чтение 16-битного указателя из Zero Page и добавление регистра Y
        public static ushort GetIndirectY(Memory memory, ref ushort pc, byte y)
        {
            byte ptr = memory.Read(pc++);
            ushort low = memory.Read(ptr);
            ushort high = memory.Read((byte)(ptr + 1));
            ushort baseAddr = (ushort)((high << 8) | low);
            return (ushort)(baseAddr + y);
        }
    }
}