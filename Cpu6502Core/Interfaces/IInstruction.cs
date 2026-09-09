namespace Cpu6502Core.Interfaces
{
    public interface IInstruction
    {
        void Execute(Cpu6502 cpu, Memory memory, Registers regs);
    }
}