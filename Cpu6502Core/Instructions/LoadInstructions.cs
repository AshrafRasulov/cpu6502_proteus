using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class LdaImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            regs.A = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class LdaZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A = memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class LdaAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort absAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.A = memory.Read(absAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }
}