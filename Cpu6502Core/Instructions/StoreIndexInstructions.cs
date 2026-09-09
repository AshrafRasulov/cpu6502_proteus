using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class StxZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte addr = memory.Read(regs.PC++);
            memory.Write(addr, regs.X);
        }
    }

    public class StxAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort addr = memory.RWORD(regs.PC);
            regs.PC += 2;
            memory.Write(addr, regs.X);
        }
    }

    public class StyZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte addr = memory.Read(regs.PC++);
            memory.Write(addr, regs.Y);
        }
    }

    public class StyAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort addr = memory.RWORD(regs.PC);
            regs.PC += 2;
            memory.Write(addr, regs.Y);
        }
    }
}