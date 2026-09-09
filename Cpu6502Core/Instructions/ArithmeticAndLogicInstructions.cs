using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- ADC (Add with Carry) ---

    public class AdcImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    internal static class AdcHelper
    {
        public static void ExecuteAdc(Cpu6502 cpu, Registers regs, byte operand)
        {
            bool carry = (regs.P & 0x01) != 0;
            int sum = regs.A + operand + (carry ? 1 : 0);
            cpu.UpdateAddFlags(sum, regs.A, operand, carry);
            regs.A = (byte)(sum & 0xFF);
        }
    }

    // --- AND ---

    public class AndImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            regs.A &= operand;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AndZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A &= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AndAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.A &= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    // --- ORA ---

    public class OraImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            regs.A |= operand;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class OraZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A |= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class OraAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.A |= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    // --- EOR ---

    public class EorImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            regs.A ^= operand;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class EorZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class EorAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

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