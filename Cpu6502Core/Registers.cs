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


        public void SetZeroAndNegativeFlags(byte value)
        {
            // Бит 1 (0x02) - Zero flag: устанавливается, если значение равно 0
            if (value == 0)
            {
                P |= 0x02;
            }
            else
            {
                P &= 0xFD;
            }

            // Бит 7 (0x80) - Negative flag: устанавливается, если старший бит равен 1
            if ((value & 0x80) != 0)
            {
                P |= 0x80;
            }
            else
            {
                P &= 0x7F;
            }
        }
    }
}