using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- ASL (Arithmetic Shift Left) ---

    public class AslAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte carry = (byte)((regs.A & 0x80) != 0 ? 1 : 0);
            regs.A = (byte)(regs.A << 1);
            if (carry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class AslZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            
            byte value = memory.Read(addr);
            byte carry = (byte)((value & 0x80) != 0 ? 1 : 0);
            value = (byte)(value << 1);
            
            memory.Write(addr, value);
            if (carry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(value);
        }
    }

    // --- LSR (Logical Shift Right) ---

    public class LsrAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte carry = (byte)(regs.A & 0x01);
            regs.A = (byte)(regs.A >> 1);
            if (carry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    public class LsrZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;
            
            byte value = memory.Read(addr);
            byte carry = (byte)(value & 0x01);
            value = (byte)(value >> 1);
            
            memory.Write(addr, value);
            if (carry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(value);
        }
    }

    // --- ROL (Rotate Left) ---

    public class RolAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte oldCarry = (byte)((regs.P & 0x01) != 0 ? 1 : 0);
            byte newCarry = (byte)((regs.A & 0x80) != 0 ? 1 : 0);
            regs.A = (byte)((regs.A << 1) | oldCarry);
            if (newCarry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }

    // --- ROR (Rotate Right) ---

    public class RorAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            byte oldCarry = (byte)((regs.P & 0x01) != 0 ? 1 : 0);
            byte newCarry = (byte)(regs.A & 0x01);
            regs.A = (byte)((regs.A >> 1) | (oldCarry << 7));
            if (newCarry == 1) regs.P |= 0x01; else regs.P &= 0xFE;
            cpu.UpdateZeroAndNegativeFlags(regs.A);
        }
    }
}