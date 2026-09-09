using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class InxInstruction : IInstruction 
    { 
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) 
        { 
            regs.X++; 
            cpu.UpdateZeroAndNegativeFlags(regs.X); 
        } 
    }

    public class InyInstruction : IInstruction 
    { 
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) 
        { 
            regs.Y++; 
            cpu.UpdateZeroAndNegativeFlags(regs.Y); 
        } 
    }

    public class DexInstruction : IInstruction 
    { 
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) 
        { 
            regs.X--; 
            cpu.UpdateZeroAndNegativeFlags(regs.X); 
        } 
    }

    public class DeyInstruction : IInstruction 
    { 
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) 
        { 
            regs.Y--; 
            cpu.UpdateZeroAndNegativeFlags(regs.Y); 
        } 
    }

    public class IncZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;

            byte val = (byte)(memory.Read(addr) + 1);
            memory.Write(addr, val);
            cpu.UpdateZeroAndNegativeFlags(val);
        }
    }

    public class DecZeroPageInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort addr = AddressingModes.GetZeroPage(memory, ref pc);
            regs.PC = pc;

            byte val = (byte)(memory.Read(addr) - 1);
            memory.Write(addr, val);
            cpu.UpdateZeroAndNegativeFlags(val);
        }
    }
}