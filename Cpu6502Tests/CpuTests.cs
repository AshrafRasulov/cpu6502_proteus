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
            
            // Выполняем LDA #$42
            cpu.Step();
            
            Assert.Equal(0x42, cpu.A);
            Assert.False((cpu.P & 0x02) != 0); // Флаг Zero должен быть 0
            Assert.False((cpu.P & 0x80) != 0); // Флаг Negative должен быть 0
            Assert.Equal(0x8002, cpu.PC);     // PC сместился на 2 байта
        }
    }
}