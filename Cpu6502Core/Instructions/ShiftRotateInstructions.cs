using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // ASL Accumulator (Opcode 0x0A)
    public class AslAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            cpu.SetCarryFlag(regs, (regs.A & 0x80) != 0);
            regs.A = (byte)(regs.A << 1);
            cpu.SetZeroFlag(regs, regs.A);
            cpu.SetNegativeFlag(regs, regs.A);
        }
    }
    

    // LSR Accumulator (Opcode 0x4A)
    public class LsrAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            cpu.SetCarryFlag(regs, (regs.A & 0x01) != 0);
            regs.A = (byte)(regs.A >> 1);
            cpu.SetZeroFlag(regs, regs.A);
            cpu.SetNegativeFlag(regs, regs.A);
        }
    }

    // ROL Accumulator (Opcode 0x2A)
    // --- ROL (Rotate Left) ---
    public class RolAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            bool oldCarry = (regs.P & 0x01) != 0;
            cpu.SetCarryFlag(regs, (regs.A & 0x80) != 0);
            regs.A = (byte)((regs.A << 1) | (oldCarry ? 1 : 0));
            cpu.SetZeroFlag(regs, regs.A);
            cpu.SetNegativeFlag(regs, regs.A);
        }
    }

    // ROR Accumulator (Opcode 0x6A)
    public class RorAccumulatorInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            bool oldCarry = (regs.P & 0x01) != 0;
            cpu.SetCarryFlag(regs, (regs.A & 0x01) != 0);
            regs.A = (byte)((regs.A >> 1) | (oldCarry ? 0x80 : 0));
            cpu.SetZeroFlag(regs, regs.A);
            cpu.SetNegativeFlag(regs, regs.A);
        }
    }
}