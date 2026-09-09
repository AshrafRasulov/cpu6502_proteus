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

        [Fact]
        public void TestJmpAndBranchInstructions()
        {
            var cpu = new Cpu6502();

            // Загружаем инструкцию JMP Absolute по адресу 0x8000, прыгаем на 0x9000
            // Опкод JMP (0x4C), младший байт (0x00), старший байт (0x90)
            byte[] program = new byte[] { 0x4C, 0x00, 0x90 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // Выполняем JMP

            Assert.Equal(0x9000, cpu.PC);
        }
        
        [Fact]
        public void TestBeqInstruction()
        {
            var cpu = new Cpu6502();

            // Загружаем программу начиная с адреса 0x8000 (LoadProgram сама установит PC)
            byte[] program = new byte[] { 0xA9, 0x00, 0xF0, 0x05 };
            cpu.LoadProgram(program, 0x8000);

            // Шаг 1: Выполняем LDA #$00 (PC сместится на 2 байта, активируется Zero flag)
            cpu.Step();

            // Шаг 2: Выполняем BEQ +5
            cpu.Step();

            Assert.Equal(0x8009, cpu.PC);
        }

        [Fact]
        public void TestJsrAndRtsInstructions()
        {
            var cpu = new Cpu6502();

            // Программа:
            // 0x8000: JSR 0x8005 (вызываем подпрограмму)
            // 0x8003: NOP (сюда должны вернуться после RTS)
            // ...
            // 0x8005: RTS (возврат из подпрограммы)
            
            byte[] program = new byte[] {
                0x20, 0x05, 0x80, // 0x8000: JSR $8005
                0xEA,             // 0x8003: NOP
                0xEA,             // 0x8004: NOP (заполнитель для смещения)
                0x60              // 0x8005: RTS
            };
            
            cpu.LoadProgram(program, 0x8000);

            // Выполняем JSR (PC должен перейти на 0x8005, а адрес возврата (0x8002) записаться в стек)
            cpu.Step();
            Assert.Equal(0x8005, cpu.PC);

            // Выполняем RTS (процессор должен считать адрес из стека, прибавить 1 и вернуться на 0x8003)
            cpu.Step();
            Assert.Equal(0x8003, cpu.PC);
        }

        [Fact]
        public void TestPhaAndPlaInstructions()
        {
            var cpu = new Cpu6502();

            // Программа:
            // LDA #$77
            // PHA (сохраняем в стек)
            // LDA #$00 (обнуляем аккумулятор)
            // PLA (достаем из стека обратно в A)
            byte[] program = new byte[] { 0xA9, 0x77, 0x48, 0xA9, 0x00, 0x68 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$77
            Assert.Equal(0x77, cpu.A);

            cpu.Step(); // PHA
            cpu.Step(); // LDA #$00
            Assert.Equal(0x00, cpu.A);

            cpu.Step(); // PLA
            Assert.Equal(0x77, cpu.A);
        }

        [Fact]
        public void TestPhpAndPlpInstructions()
        {
            var cpu = new Cpu6502();
            // Программа:
            // 1. LDA #$00 -> устанавливает Zero флаг (P содержит флаг Zero)
            // 2. PHP      -> сохраняет статус в стек
            // 3. LDA #$01 -> сбрасывает Zero флаг в процессоре
            // 4. PLP      -> восстанавливает статус из стека (возвращает Zero флаг)
            byte[] program = new byte[] { 0xA9, 0x00, 0x08, 0xA9, 0x01, 0x28 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$00 (ставит Zero флаг)
            cpu.Step(); // PHP (пушим статус со взведенным Zero)
            
            cpu.Step(); // LDA #$01 (сбрасывает Zero флаг в регистре P)
            // Проверяем, что флаг действительно сбросился
            Assert.Equal(0, cpu.P & 0x02);

            cpu.Step(); // PLP (достает статус обратно из стека)
            // Проверяем, что флаг Zero снова вернулся из стека
            Assert.NotEqual(0, cpu.P & 0x02);
        }

        [Fact]
        public void TestBneNegativeBranch()
        {
            var cpu = new Cpu6502();

            // Программа:
            // 0x8000: LDA #$01 (2 байта, переносит PC на 0x8002)
            // 0x8002: BNE 0xFA (-6 в дополнительном коде) (2 байта, переносит PC на 0x8004)
            // При выполнении ветвления PC станет: 0x8004 + (sbyte)0xFA (-6) = 0x80FE
            byte[] program = new byte[] { 0xA9, 0x01, 0xD0, 0xFA };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // Выполнился LDA #$01, PC = 0x8002
            Assert.Equal(0x8002, cpu.PC);

            cpu.Step(); // Выполнился BNE с отрицательным смещением
            // Assert.Equal(0x80FE, cpu.PC);
            Assert.Equal(0x7FFE, cpu.PC);
        }

        [Fact]
        public void TestStxAbsolute()
        {
            var cpu = new Cpu6502();
            // Программа: LDX #$55, STX $1234
            // Опкоды: LDX Immediate (0xA2, 0x55), STX Absolute (0x8E, 0x34, 0x12)
            byte[] program = new byte[] { 0xA2, 0x55, 0x8E, 0x34, 0x12 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDX #$55
            Assert.Equal(0x55, cpu.X);

            cpu.Step(); // STX $1234
            Assert.Equal(0x55, cpu.Read(0x1234));
        }

        [Fact]
        public void TestStyZeroPage()
        {
            var cpu = new Cpu6502();
            // Программа: LDY #$66, STY $30
            // Опкоды: LDY Immediate (0xA0, 0x66), STY ZeroPage (0x84, 0x30)
            byte[] program = new byte[] { 0xA0, 0x66, 0x84, 0x30 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDY #$66
            Assert.Equal(0x66, cpu.Y);

            cpu.Step(); // STY $30
            Assert.Equal(0x66, cpu.Read(0x0030));
        }

        [Fact]
        public void TestStyAbsolute()
        {
            var cpu = new Cpu6502();
            // Программа: LDY #$77, STY $4321
            // Опкоды: LDY Immediate (0xA0, 0x77), STY Absolute (0x8C, 0x21, 0x43)
            byte[] program = new byte[] { 0xA0, 0x77, 0x8C, 0x21, 0x43 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDY #$77
            Assert.Equal(0x77, cpu.Y);

            cpu.Step(); // STY $4321
            Assert.Equal(0x77, cpu.Read(0x4321));
        }

        [Fact]
        public void TestAslAccumulator()
        {
            var cpu = new Cpu6502();
            // Загружаем 0x40 (01000000), ASL сдвигает влево, старший бит уходит в Carry
            byte[] program = new byte[] { 0xA9, 0x40, 0x0A };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$40
            cpu.Step(); // ASL A (A станет 0x80, Carry = 0)

            Assert.Equal(0x80, cpu.A);
            Assert.False((cpu.P & 0x01) != 0); // Carry флаг 0
            Assert.True((cpu.P & 0x80) != 0);  // Negative флаг 1
        }

        [Fact]
        public void TestLsrAccumulator()
        {
            var cpu = new Cpu6502();
            // Загружаем 0x01, LSR сдвигает вправо, младший бит уходит в Carry
            byte[] program = new byte[] { 0xA9, 0x01, 0x4A };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$01
            cpu.Step(); // LSR A (A станет 0x00, Carry = 1, Zero = 1)

            Assert.Equal(0x00, cpu.A);
            Assert.True((cpu.P & 0x01) != 0);  // Carry флаг 1
            Assert.True((cpu.P & 0x02) != 0);  // Zero флаг 1
        }

        [Fact]
        public void TestCpxImmediate()
        {
            var cpu = new Cpu6502();
            // LDX #$30, CPX #$20 (X > значение, должен установиться Carry)
            byte[] program = new byte[] { 0xA2, 0x30, 0xE0, 0x20 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDX #$30
            cpu.Step(); // CPX #$20

            Assert.Equal(0x30, cpu.X);
            Assert.True((cpu.P & 0x01) != 0);  // Carry флаг активен (30 >= 20)
            Assert.False((cpu.P & 0x02) != 0); // Zero неактивен
        }

        [Fact]
        public void TestCpyImmediate()
        {
            var cpu = new Cpu6502();
            // 0xA0 — это LDY Immediate
            // 0x15 — аргумент для LDY
            // 0xC0 — это опкод для CPY Immediate

  
            byte[] program = new byte[] { 0xA0, 0x15, 0xC0, 0x15 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDY #$15
            cpu.Step(); // CPY #$15

            Assert.Equal(0x15, cpu.Y);
            Assert.True((cpu.P & 0x01) != 0);  // Carry активен
            Assert.True((cpu.P & 0x02) != 0);  // Zero активен
        }

        [Fact]
        public void TestBitInstruction()
        {
            var cpu = new Cpu6502();
            // Записываем в ячейку 0x10 значение 0xC0 (биты 7 и 6 взведены: N=1, V=1)
            cpu.Write(0x0010, 0xC0);
            
            // Загружаем в A значение 0x40 (бит 6), затем проверяем BIT $10
            byte[] program = new byte[] { 0xA9, 0x40, 0x24, 0x10 };
            cpu.LoadProgram(program, 0x8000);

            cpu.Step(); // LDA #$40
            cpu.Step(); // BIT $10

            // Аккумулятор не изменился
            Assert.Equal(0x40, cpu.A);
            // Флаг Negative (бит 7) берется из бит 7 памяти (0xC0 -> N=1)
            Assert.True((cpu.P & 0x80) != 0);
            // Флаг Overflow (бит 6) берется из бит 6 памяти (0xC0 -> V=1)
            Assert.True((cpu.P & 0x40) != 0);
            // Флаг Zero (A & память = 0x40 & 0xC0 = 0x40 != 0, значит Zero = 0)
            Assert.False((cpu.P & 0x02) != 0);
        }


    }
}