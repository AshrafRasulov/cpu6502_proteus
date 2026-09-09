using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class LdyImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            regs.Y = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.UpdateZeroAndNegativeFlags(regs.Y);
        }
    }

    public class LdyZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.Y = memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.Y);
        }
    }

    public class LdyAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort absAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.Y = memory.Read(absAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.Y);
        }
    }

    public class StyZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            memory.Write(zeroPageAddr, regs.Y);
        }
    }

    public class StyAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort absAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            memory.Write(absAddr, regs.Y);
        }
    }
}