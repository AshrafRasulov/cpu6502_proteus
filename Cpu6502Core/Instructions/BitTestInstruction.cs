using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class BitZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte addr = cpu.FetchZeroPage(memory, regs);
            byte value = memory.Read(addr);
            
            byte result = (byte)(regs.A & value);
            cpu.SetZeroFlag(regs, result);
            cpu.SetOverflowFlag(regs, (value & 0x40) != 0);
            cpu.SetNegativeFlag(regs, value);
        }
    }
}