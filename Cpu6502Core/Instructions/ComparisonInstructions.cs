using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class CmpImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.Compare(regs.A, operand);
        }
    }

    public class CmpZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }

        public class CmpAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }
}