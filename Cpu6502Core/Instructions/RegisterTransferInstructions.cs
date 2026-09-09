using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class TaxInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.X = regs.A; regs.SetZeroAndNegativeFlags(regs.X); }
    }
    public class TayInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.Y = regs.A; regs.SetZeroAndNegativeFlags(regs.Y); }
    }
    public class TxaInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.A = regs.X; regs.SetZeroAndNegativeFlags(regs.A); }
    }
    public class TyaInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.A = regs.Y; regs.SetZeroAndNegativeFlags(regs.A); }
    }
    public class TsxInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.X = regs.S; regs.SetZeroAndNegativeFlags(regs.X); }
    }
    public class TxsInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs) { regs.S = regs.X; }
    }
}