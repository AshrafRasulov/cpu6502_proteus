using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
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

}