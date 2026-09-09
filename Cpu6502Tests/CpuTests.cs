using Xunit;
using Cpu6502Core;

namespace Cpu6502Tests
{
    public class CpuTests
    {
        [Fact]
        public void TestLdaImmediate()
        {
            var cpu = new Cpu6502();
            // Программа: LDA #$42, NOP
            byte[] program = new byte[] { 0xA9, 0x42, 0xEA };
            
            cpu.LoadProgram(program, 0x8000);
            cpu.Step();
            
            Assert.Equal(0x42, cpu.A);
            Assert.False((cpu.P & 0x02) != 0); // Флаг Zero должен быть 0
            Assert.False((cpu.P & 0x80) != 0); // Флаг Negative должен быть 0
            Assert.Equal(0x8002, cpu.PC);     // PC сместился на 2 байта
        }

        [Fact]
        public void TestLdaAbsolute()
        {
            var cpu = new Cpu6502();
            
            // Записываем тестовое значение 0x55 в ячейку памяти 0x1234
            cpu.Write(0x1234, 0x55);

            // Программа: опкод 0xAD (LDA Absolute) и адрес 0x1234 (Little Endian)
            byte[] program = new byte[] { 0xAD, 0x34, 0x12 };
            cpu.LoadProgram(program, 0x8000);
            
            cpu.Step();

            Assert.Equal(0x55, cpu.A);
            Assert.False((cpu.P & 0x02) != 0); // Флаг Zero должен быть 0
            Assert.False((cpu.P & 0x80) != 0); // Флаг Negative должен быть 0
            Assert.Equal(0x8003, cpu.PC);     // PC сместился на 3 байта
        }

        [Fact]
        public void TestStaZeroPage()
        {
            var cpu = new Cpu6502();
            // Сначала загрузим в аккумулятор значение 0x77 с помощью LDA Immediate (0xA9)
            // Затем сохраним его в нулевую страницу по адресу 0x20 с помощью STA Zero Page (0x85 0x20)
            byte[] program = new byte[] { 0xA9, 0x77, 0x85, 0x20 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // Выполнился LDA #$77 (в A теперь 0x77)
            cpu.Step(); // Выполнился STA $20 (в памяти по адресу 0x0020 записалось 0x77)

            Assert.Equal(0x77, cpu.Read(0x0020));
        }

        [Fact]
        public void TestStaAbsolute()
        {
            var cpu = new Cpu6502();
            // Загружаем 0x88 в аккумулятор, затем сохраняем в абсолютный адрес 0x1234
            byte[] program = new byte[] { 0xA9, 0x88, 0x8D, 0x34, 0x12 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$88
            cpu.Step(); // STA $1234

            Assert.Equal(0x88, cpu.Read(0x1234));
        }

        [Fact]
        public void TestLdxAndStx()
        {
            var cpu = new Cpu6502();
            // Программа: LDX #$33, STX $40
            byte[] program = new byte[] { 0xA2, 0x33, 0x86, 0x40 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // Выполняется LDX #$33
            Assert.Equal(0x33, cpu.X);

            cpu.Step(); // Выполняется STX $40
            Assert.Equal(0x33, cpu.Read(0x0040));
        }

        [Fact]
        public void TestLdyAndSty()
        {
            var cpu = new Cpu6502();
            // Программа: LDY #$44, STY $50
            byte[] program = new byte[] { 0xA0, 0x44, 0x84, 0x50 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // Выполняется LDY #$44
            Assert.Equal(0x44, cpu.Y);

            cpu.Step(); // Выполняется STY $50
            Assert.Equal(0x44, cpu.Read(0x0050));
        }

        [Fact]
        public void TestAdcImmediate()
        {
            var cpu = new Cpu6502();
            // Загружаем 10 в аккумулятор, затем прибавляем #$20 (32 в десятичной)
            byte[] program = new byte[] { 0xA9, 0x0A, 0x69, 0x20 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$0A (A = 10)
            cpu.Step(); // ADC #$20 (A = 10 + 32 = 42 или 0x2A)

            Assert.Equal(0x2A, cpu.A);
        }

        [Fact]
        public void TestParallelCpuExecution()
        {
            var cpu1 = new Cpu6502();
            cpu1.LoadProgram(new byte[] { 0xA9, 0x11 }, 0x8000); // LDA #$11

            var cpu2 = new Cpu6502();
            cpu2.LoadProgram(new byte[] { 0xA9, 0x22 }, 0x8000); // LDA #$22

            Parallel.Invoke(
                () => cpu1.Step(),
                () => cpu2.Step()
            );

            Assert.Equal(0x11, cpu1.A);
            Assert.Equal(0x22, cpu2.A);
        }

        [Fact]
        public void TestAndImmediate()
        {
            var cpu = new Cpu6502();
            // Загружаем 0xCF (11001111 в двоичной), затем делаем AND с 0xF0 (11110000)
            byte[] program = new byte[] { 0xA9, 0xCF, 0x29, 0xF0 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$CF
            cpu.Step(); // AND #$F0 (0xCF & 0xF0 = 0xC0)

            Assert.Equal(0xC0, cpu.A);
        }

        [Fact]
        public void TestOraImmediate()
        {
            var cpu = new Cpu6502();
            // Загружаем 0x0F (00001111), затем делаем ORA с 0xF0 (11110000)
            byte[] program = new byte[] { 0xA9, 0x0F, 0x09, 0xF0 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$0F
            cpu.Step(); // ORA #$F0 (0x0F | 0xF0 = 0xFF)

            Assert.Equal(0xFF, cpu.A);
        }

        [Fact]
        public void TestEorImmediate()
        {
            var cpu = new Cpu6502();
            // Загружаем 0xFF (11111111), затем делаем EOR с 0x0F (00001111)
            byte[] program = new byte[] { 0xA9, 0xFF, 0x49, 0x0F };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$FF
            cpu.Step(); // EOR #$0F (0xFF ^ 0x0F = 0xF0)

            Assert.Equal(0xF0, cpu.A);
        }

        [Fact]
        public void TestCmpImmediate()
        {
            var cpu = new Cpu6502();
            // Загружаем 0x50, затем сравниваем с 0x50 (должен установиться флаг Zero)
            byte[] program = new byte[] { 0xA9, 0x50, 0xC9, 0x50 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$50
            cpu.Step(); // CMP #$50

            // Проверяем флаг Zero (бит 1 в регистре P) и что аккумулятор не изменился
            Assert.Equal(0x50, cpu.A);
            Assert.True((cpu.P & 0x02) != 0); // Zero флаг активен
            Assert.True((cpu.P & 0x01) != 0); // Carry флаг активен (50 >= 50)
        }

    }
}