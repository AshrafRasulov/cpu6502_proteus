using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class ClcInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P &= 0xFE; } // инверсия 0x01
    public class SecInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P |= 0x01; }
    public class CliInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P &= 0xFB; } // инверсия 0x04
    public class SeiInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P |= 0x04; }
    public class ClvInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P &= 0xBF; } // инверсия 0x40
    public class CldInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P &= 0xF7; } // инверсия 0x08
    public class SedInstruction : IInstruction { public void Execute(Cpu6502 cpu, Memory memory, Registers regs) => regs.P |= 0x08; }
}