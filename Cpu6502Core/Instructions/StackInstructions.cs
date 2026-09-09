using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    public class PhaInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            memory.Write((ushort)(0x0100 + regs.S), regs.A);
            regs.S--;
        }
    }

    public class PlaInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            regs.S++;
            regs.A = memory.Read((ushort)(0x0100 + regs.S));

            // Установка флагов Zero и Negative
            regs.SetZeroAndNegativeFlags(regs.A);
        }
    }

    public class PhpInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            // В 6502 при пуше статуса обычно устанавливаются биты B (Break) в 1
            memory.Write((ushort)(0x0100 + regs.S), (byte)(regs.P | 0x30));
            regs.S--;
        }
    }

    public class PlpInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            regs.S++;
            // Вытягиваем статус из стека (сохраняя биты флагов, кроме фиксированных, если требуется)
            regs.P = memory.Read((ushort)(0x0100 + regs.S));
        }
    }
}