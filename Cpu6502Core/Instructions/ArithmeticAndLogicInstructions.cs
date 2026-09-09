using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class AdcImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            
            bool carry = (regs.P & 0x01) != 0;
            int sum = regs.A + operand + (carry ? 1 : 0);
            
            cpu.UpdateAddFlags(sum, regs.A, operand, carry);
            regs.A = (byte)(sum & 0xFF);
        }
    }

    public class AdcZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(zeroPageAddr);
            
            bool carry = (regs.P & 0x01) != 0;
            int sum = regs.A + operand + (carry ? 1 : 0);
            
            cpu.UpdateAddFlags(sum, regs.A, operand, carry);
            regs.A = (byte)(sum & 0xFF);
        }
    }

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
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A &= memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

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
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A |= memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

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
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            regs.A ^= memory.Read(zeroPageAddr);
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class CmpImmediateInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte operand = AddressingModes.GetImmediate(memory, ref pc);
            regs.PC = pc;
            cpu.UpdateCompareFlags(regs.A, operand);
        }
    }

    public class CmpZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort zeroPageAddr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            byte operand = memory.Read(zeroPageAddr);
            cpu.UpdateCompareFlags(regs.A, operand);
        }
    }
}