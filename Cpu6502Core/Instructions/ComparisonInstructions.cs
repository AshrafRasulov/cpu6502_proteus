using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- CMP (Compare Accumulator) ---

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

    public class CmpZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
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

    public class CmpAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }

    public class CmpAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }

    public class CmpIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }

    public class CmpIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            cpu.Compare(regs.A, operand);
        }
    }
}