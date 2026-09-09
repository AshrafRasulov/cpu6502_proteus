namespace Cpu6502Core
{
    public class Cpu6502
    {
        // Регистры процессора 6502
        public byte A { get; set; }     // Аккумулятор
        public byte X { get; set; }     // Индексный регистр X
        public byte Y { get; set; }     // Индексный регистр Y
        public byte SP { get; set; }    // Указатель стека
        public ushort PC { get; set; }  // Указатель команд
        public byte P { get; set; }     // Регистр флагов (Processor Status)

        // Память 64 КБ
        private readonly byte[] _memory = new byte[65536];

        public Cpu6502()
        {
            Reset();
        }

        public void Reset()
        {
            A = 0;
            X = 0;
            Y = 0;
            SP = 0xFD;
            PC = 0x8000; // Стандартный адрес старта для многих систем 6502
            P = 0x24;    // Базовые флаги
        }

        public byte Read(ushort address) => _memory[address];
        
        public void Write(ushort address, byte value) => _memory[address] = value;

        public void LoadProgram(byte[] program, ushort startAddress)
        {
            for (int i = 0; i < program.Length; i++)
            {
                _memory[startAddress + (ushort)i] = program[i];
            }
            PC = startAddress;
        }

        public void Step()
        {
            byte opcode = Read(PC++);
            Execute(opcode);
        }

        private void Execute(byte opcode)
        {
            switch (opcode)
            {
                case 0xEA: // NOP (No Operation)
                    // Ничего не делает, занимает 2 такта
                    break;

                case 0xA9: // LDA Immediate (Загрузка в A значения)
                    A = Read(PC++);
                    UpdateZeroAndNegativeFlags(A);
                    break;

                default:
                    // Неизвестный опкод — пропускаем или останавливаемся
                    break;
            }
        }

        private void UpdateZeroAndNegativeFlags(byte val)
        {
            // Z флаг (бит 1) - сброс через инверсию маски через XOR
            if (val == 0) P |= 0x02; else P &= (byte)(0x02 ^ 0xFF);
            
            // N флаг (бит 7)
            if ((val & 0x80) != 0) P |= 0x80; else P &= (byte)(0x80 ^ 0xFF);
        }
    }
}