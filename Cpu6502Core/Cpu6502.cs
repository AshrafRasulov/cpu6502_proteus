using System;
using Cpu6502Core.Interfaces;
using Cpu6502Core.Instructions;

namespace Cpu6502Core
{
    public class Cpu6502
    {
        private readonly Memory _memory = new Memory();
        private readonly Registers _regs = new Registers();
        private readonly IInstruction[] _instructions = new IInstruction[256];

        public byte    A => _regs.A;
        public byte    X => _regs.X;
        public byte    Y => _regs.Y;
        public byte    SP => _regs.S;
        public ushort  PC => _regs.PC;
        public byte    P => _regs.P;

        public Cpu6502()
        {
            RegisterInstructions();
            Reset();
        }

        private void RegisterInstructions()
        {
            _instructions[Opcodes.NOP] = new NopInstruction();

            // LDA
            _instructions[Opcodes.LDA_Immediate] = new LdaImmediateInstruction();
            _instructions[Opcodes.LDA_ZeroPage]  = new LdaZeroPageInstruction();
            _instructions[Opcodes.LDA_Absolute]  = new LdaAbsoluteInstruction();

            // STA
            _instructions[Opcodes.STA_ZeroPage]  = new StaZeroPageInstruction();
            _instructions[Opcodes.STA_Absolute]  = new StaAbsoluteInstruction();

            // LDX / STX
            _instructions[Opcodes.LDX_Immediate] = new LdxImmediateInstruction();
            _instructions[Opcodes.LDX_ZeroPage]  = new LdxZeroPageInstruction();
            _instructions[Opcodes.LDX_Absolute]  = new LdxAbsoluteInstruction();
            _instructions[Opcodes.STX_ZeroPage]  = new StxZeroPageInstruction();
            _instructions[Opcodes.STX_Absolute]  = new StxAbsoluteInstruction();

            // LDY / STY
            _instructions[Opcodes.LDY_Immediate] = new LdyImmediateInstruction();
            _instructions[Opcodes.LDY_ZeroPage]  = new LdyZeroPageInstruction();
            _instructions[Opcodes.LDY_Absolute]  = new LdyAbsoluteInstruction();
            _instructions[Opcodes.STY_ZeroPage]  = new StyZeroPageInstruction();
            _instructions[Opcodes.STY_Absolute]  = new StyAbsoluteInstruction();

            // ADC
            _instructions[Opcodes.ADC_Immediate] = new AdcImmediateInstruction();
            _instructions[Opcodes.ADC_ZeroPage]  = new AdcZeroPageInstruction();

            // AND
            _instructions[Opcodes.AND_Immediate] = new AndImmediateInstruction();
            _instructions[Opcodes.AND_ZeroPage]  = new AndZeroPageInstruction();

            // ORA
            _instructions[Opcodes.ORA_Immediate] = new OraImmediateInstruction();
            _instructions[Opcodes.ORA_ZeroPage]  = new OraZeroPageInstruction();

            // EOR
            _instructions[Opcodes.EOR_Immediate] = new EorImmediateInstruction();
            _instructions[Opcodes.EOR_ZeroPage]  = new EorZeroPageInstruction();

            // CMP
            _instructions[Opcodes.CMP_Immediate] = new CmpImmediateInstruction();
            _instructions[Opcodes.CMP_ZeroPage]  = new CmpZeroPageInstruction();

            // Control Flow & Branching
            _instructions[Opcodes.JMP_Absolute]  = new JmpAbsoluteInstruction();
            _instructions[Opcodes.JSR_Absolute]  = new JsrInstruction();
            _instructions[Opcodes.RTS]           = new RtsInstruction();
            _instructions[Opcodes.BEQ]           = new BeqInstruction();
            _instructions[Opcodes.BNE]           = new BneInstruction();

            // Stack Operations
            _instructions[Opcodes.PHA] = new PhaInstruction();
            _instructions[Opcodes.PLA] = new PlaInstruction();
            _instructions[Opcodes.PHP] = new PhpInstruction();
            _instructions[Opcodes.PLP] = new PlpInstruction();
        }

        public void Reset()
        {
            _regs.A = 0;
            _regs.X = 0;
            _regs.Y = 0;
            _regs.S = 0xFD;
            _regs.PC = 0x8000;
            _regs.P = 0x24;
        }

        public byte Read(ushort address) => _memory.Read(address);
        
        public void Write(ushort address, byte value) => _memory.Write(address, value);

        public void LoadProgram(byte[] program, ushort startAddress)
        {
            _memory.LoadProgram(program, startAddress);
            _regs.PC = startAddress;
        }

        public void Step()
        {
            byte opcode = _memory.Read(_regs.PC++);
            var instruction = _instructions[opcode];
            
            if (instruction != null)
            {
                instruction.Execute(this, _memory, _regs);
            }
            else
            {
                throw new NotSupportedException($"Opcode 0x{opcode:X2} is not implemented.");
            }
        }

        public void UpdateZeroAndNegativeFlags(byte val)
        {
            // Делегируем готовой реализации в классе Registers для избежания дублирования
            _regs.SetZeroAndNegativeFlags(val);
        }

        public void UpdateAddFlags(int result, byte a, byte operand, bool carryIn)
        {
            if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= 0xFD;
            if (result > 0xFF) _regs.P |= 0x01; else _regs.P &= 0xFE;
            if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= 0x7F;
            
            bool overflow = (~(a ^ operand) & (a ^ result) & 0x80) != 0;
            if (overflow) _regs.P |= 0x40; else _regs.P &= 0xBF;
        }

        public void UpdateCompareFlags(byte reg, byte operand)
        {
            int result = reg - operand;
            if (reg >= operand) _regs.P |= 0x01; else _regs.P &= 0xFE;
            if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= 0xFD;
            if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= 0x7F;
        }

        public void Compare(byte reg, byte operand)
        {
            UpdateCompareFlags(reg, operand);
        }
    }

    public class NopInstruction : IInstruction
    {
        public void Execute(Cpu6502 cpu, Memory memory, Registers regs)
        {
            // Ничего не делает
        }
    }
}
