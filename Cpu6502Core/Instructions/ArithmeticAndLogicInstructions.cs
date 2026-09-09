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

    public class AdcIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = memory.Read(addr);
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class AdcIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
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

    // --- SBC (Subtract with Carry) ---

    public class SbcImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = (byte)(~AddressingModes.GetImmediate(memory, ref pc));
            regs.PC = pc;
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
        }
    }

    public class SbcIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
            regs.PC = pc;
            byte operand = (byte)(~memory.Read(addr));
            AdcHelper.ExecuteAdc(cpu, regs, operand);
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

    public class AndZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
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

    public class AndAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A &= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AndAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            regs.A &= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AndIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A &= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AndIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
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

    public class OraZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
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

    public class OraAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A |= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class OraAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            regs.A |= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class OraIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A |= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class OraIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
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

    public class EorZeroPageXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPageX(memory, ref pc, regs.X);
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

    public class EorAbsoluteXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class EorAbsoluteYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetAbsoluteY(memory, ref pc, regs.Y);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class EorIndirectXInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectX(memory, ref pc, regs.X);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class EorIndirectYInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetIndirectY(memory, ref pc, regs.Y);
            regs.PC = pc;
            regs.A ^= memory.Read(addr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    internal static class CmpHelper
    {
        public static void ExecuteCmp(Cpu6502 cpu, Registers regs, byte regVal, byte operand)
        {
            int result = regVal - operand;
            // Установка Carry, если regVal >= operand
            if (regVal >= operand)
                regs.P |= 0x01;
            else
                regs.P &= unchecked((byte)~0x01);

            cpu.UpdateZeroAndNegativeFlags((byte)(result & 0xFF));
        }
    }
}