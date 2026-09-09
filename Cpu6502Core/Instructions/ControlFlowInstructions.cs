using Cpu6502Core.Interfaces;

namespace Cpu6502Core.Instructions
{
    // --- JMP (Jump) ---

    public class JmpAbsoluteInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort absAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = absAddr;
        }
    }

    public class JmpIndirectInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort ptrAddr = AddressingModes.GetAbsolute(memory, ref pc);
            regs.PC = pc;
            
            // Эмуляция бага оригинального чипа 6502 для Indirect JMP на границе страницы
            ushort targetAddr;
            if ((ptrAddr & 0x00FF) == 0x00FF)
            {
                byte low = memory.Read(ptrAddr);
                byte high = memory.Read((ushort)(ptrAddr & 0xFF00));
                targetAddr = (ushort)((high << 8) | low);
            }
            else
            {
                byte low = memory.Read(ptrAddr);
                byte high = memory.Read((ushort)(ptrAddr + 1));
                targetAddr = (ushort)((high << 8) | low);
            }
            
            regs.PC = targetAddr;
        }
    }

    // --- Subroutines (JSR / RTS) ---

    public class JsrInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            ushort targetAddr = AddressingModes.GetAbsolute(memory, ref pc);
            
            ushort returnAddr = (ushort)(pc - 1);
            
            memory.Write((ushort)(0x0100 + regs.S), (byte)(returnAddr >> 8)); // Старший
            regs.S--;
            memory.Write((ushort)(0x0100 + regs.S), (byte)(returnAddr & 0xFF));  // Младший
            regs.S--;
            
            regs.PC = targetAddr;
        }
    }

    public class RtsInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            regs.S++;
            byte low = memory.Read((ushort)(0x0100 + regs.S));   // Младший
            regs.S++;
            byte high = memory.Read((ushort)(0x0100 + regs.S));  // Старший
            
            ushort returnAddr = (ushort)((high << 8) | low);
            regs.PC = (ushort)(returnAddr + 1);
        }
    }

    // --- Conditional Branching ---

    public class BeqInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x02) != 0) // Zero flag set
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }

    public class BneInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x02) == 0) // Zero flag clear
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }

    public class BmiInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x80) != 0) // Negative flag set
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }

    public class BplInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x80) == 0) // Negative flag clear
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }

    public class BccInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x01) == 0) // Carry clear
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }

    public class BcsInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            sbyte offset = (sbyte)memory.Read(pc++);
            regs.PC = pc;
            
            if ((regs.P & 0x01) != 0) // Carry set
            {
                regs.PC = (ushort)(regs.PC + offset);
            }
        }
    }
}