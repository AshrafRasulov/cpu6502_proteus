using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- LDY (Load Y Register) ---

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

    public class LdyZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.Y = memory.Read(addr);
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

    public class LdyAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.Y = memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.Y);
        }
    }

    // --- CPY (Compare Y Register) ---

    public class CpyImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte value = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.Compare(regs.Y, value);
        }
    }

    public class CpyZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte value = memory.Read(addr);
            cpu.Compare(regs.Y, value);
        }
    }

    public class CpyAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            byte value = memory.Read(addr);
            cpu.Compare(regs.Y, value);
        }
    }
}