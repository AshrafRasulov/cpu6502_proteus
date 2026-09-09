namespace Cpu6502Core
{
    public class Registers
    {
        public byte A { get; set; } // Аккумулятор
        public byte X { get; set; } // Регистр X
        public byte Y { get; set; } // Регистр Y
        public ushort PC { get; set; } // Стек/указатель инструкций
        public byte S { get; set; } = 0xFD; // Указатель стека
        public byte P { get; set; } // Регистр флагов (Processor Status)
    }
}