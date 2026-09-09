using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- LDX (Load X Register) ---

    public class LdxImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            regs.X = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.UpdateZeroAndNegativeFlags(regs.X);
        }
    }

    public class LdxZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.X = memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.X);
        }
    }

    public class LdxZeroPageYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte baseAddr = memory.Read(pc++);
            ushort addr = (byte)(baseAddr + regs.Y);
            regs.PC = pc;
            regs.X = memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.X);
        }
    }

    public class LdxAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort absAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.X = memory.Read(absAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.X);
        }
    }

    public class LdxAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            regs.X = memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.X);
        }
    }

    // --- CPX (Compare X Register) ---

    public class CpxImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte value = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.Compare(regs.X, value);
        }
    }

    public class CpxZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte value = memory.Read(addr);
            cpu.Compare(regs.X, value);
        }
    }

    public class CpxAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            byte value = memory.Read(addr);
            cpu.Compare(regs.X, value);
        }
    }
}