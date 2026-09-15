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
            // NOP
            _instructions[Opcodes.NOP] = new NopInstruction();

            // --- LDA (Load Accumulator) ---
            _instructions[Opcodes.LDA_Immediate] = new LdaImmediateInstruction();
            _instructions[Opcodes.LDA_ZeroPage]  = new LdaZeroPageInstruction();
            _instructions[Opcodes.LDA_ZeroPageX] = new LdaZeroPageXInstruction();
            _instructions[Opcodes.LDA_Absolute]  = new LdaAbsoluteInstruction();
            _instructions[Opcodes.LDA_AbsoluteX] = new LdaAbsoluteXInstruction();
            _instructions[Opcodes.LDA_AbsoluteY] = new LdaAbsoluteYInstruction();
            _instructions[Opcodes.LDA_IndirectX] = new LdaIndirectXInstruction();
            _instructions[Opcodes.LDA_IndirectY] = new LdaIndirectYInstruction();

            // --- STA (Store Accumulator) ---
            _instructions[Opcodes.STA_ZeroPage]  = new StaZeroPageInstruction();
            _instructions[Opcodes.STA_Absolute]  = new StaAbsoluteInstruction();

            // --- LDX / STX (X Register) ---
            _instructions[Opcodes.LDX_Immediate] = new LdxImmediateInstruction();
            _instructions[Opcodes.LDX_ZeroPage]  = new LdxZeroPageInstruction();
            _instructions[Opcodes.LDX_ZeroPageY] = new LdxZeroPageYInstruction();
            _instructions[Opcodes.LDX_Absolute]  = new LdxAbsoluteInstruction();
            _instructions[Opcodes.LDX_AbsoluteY] = new LdxAbsoluteYInstruction();
            _instructions[Opcodes.STX_ZeroPage]  = new StxZeroPageInstruction();
            _instructions[Opcodes.STX_Absolute]  = new StxAbsoluteInstruction();

            // --- LDY / STY (Y Register) ---
            _instructions[Opcodes.LDY_Immediate] = new LdyImmediateInstruction();
            _instructions[Opcodes.LDY_ZeroPage]  = new LdyZeroPageInstruction();
            _instructions[Opcodes.LDY_ZeroPageX] = new LdyZeroPageXInstruction();
            _instructions[Opcodes.LDY_Absolute]  = new LdyAbsoluteInstruction();
            _instructions[Opcodes.LDY_AbsoluteX] = new LdyAbsoluteXInstruction();
            _instructions[Opcodes.STY_ZeroPage]  = new StyZeroPageInstruction();
            _instructions[Opcodes.STY_Absolute]  = new StyAbsoluteInstruction();

            // --- ADC (Add with Carry) ---
            _instructions[Opcodes.ADC_Immediate] = new AdcImmediateInstruction();
            _instructions[Opcodes.ADC_ZeroPage]  = new AdcZeroPageInstruction();
            _instructions[Opcodes.ADC_ZeroPageX] = new AdcZeroPageXInstruction();
            _instructions[Opcodes.ADC_Absolute]  = new AdcAbsoluteInstruction();
            _instructions[Opcodes.ADC_AbsoluteX] = new AdcAbsoluteXInstruction();
            _instructions[Opcodes.ADC_AbsoluteY] = new AdcAbsoluteYInstruction();
            _instructions[Opcodes.ADC_IndirectX] = new AdcIndirectXInstruction();
            _instructions[Opcodes.ADC_IndirectY] = new AdcIndirectYInstruction();

            // --- SBC (Subtract with Carry) ---
            _instructions[Opcodes.SBC_Immediate] = new SbcImmediateInstruction();
            _instructions[Opcodes.SBC_ZeroPage]  = new SbcZeroPageInstruction();
            _instructions[Opcodes.SBC_ZeroPageX] = new SbcZeroPageXInstruction();
            _instructions[Opcodes.SBC_Absolute]  = new SbcAbsoluteInstruction();
            _instructions[Opcodes.SBC_AbsoluteX] = new SbcAbsoluteXInstruction();
            _instructions[Opcodes.SBC_AbsoluteY] = new SbcAbsoluteYInstruction();
            _instructions[Opcodes.SBC_IndirectX] = new SbcIndirectXInstruction();
            _instructions[Opcodes.SBC_IndirectY] = new SbcIndirectYInstruction();

            // --- AND ---
            _instructions[Opcodes.AND_Immediate] = new AndImmediateInstruction();
            _instructions[Opcodes.AND_ZeroPage]  = new AndZeroPageInstruction();
            _instructions[Opcodes.AND_ZeroPageX] = new AndZeroPageXInstruction();
            _instructions[Opcodes.AND_Absolute]  = new AndAbsoluteInstruction();
            _instructions[Opcodes.AND_AbsoluteX] = new AndAbsoluteXInstruction();
            _instructions[Opcodes.AND_AbsoluteY] = new AndAbsoluteYInstruction();
            _instructions[Opcodes.AND_IndirectX] = new AndIndirectXInstruction();
            _instructions[Opcodes.AND_IndirectY] = new AndIndirectYInstruction();

            // --- ORA ---
            _instructions[Opcodes.ORA_Immediate] = new OraImmediateInstruction();
            _instructions[Opcodes.ORA_ZeroPage]  = new OraZeroPageInstruction();
            _instructions[Opcodes.ORA_ZeroPageX] = new OraZeroPageXInstruction();
            _instructions[Opcodes.ORA_Absolute]  = new OraAbsoluteInstruction();
            _instructions[Opcodes.ORA_AbsoluteX] = new OraAbsoluteXInstruction();
            _instructions[Opcodes.ORA_AbsoluteY] = new OraAbsoluteYInstruction();
            _instructions[Opcodes.ORA_IndirectX] = new OraIndirectXInstruction();
            _instructions[Opcodes.ORA_IndirectY] = new OraIndirectYInstruction();

            // --- EOR ---
            _instructions[Opcodes.EOR_Immediate] = new EorImmediateInstruction();
            _instructions[Opcodes.EOR_ZeroPage]  = new EorZeroPageInstruction();
            _instructions[Opcodes.EOR_ZeroPageX] = new EorZeroPageXInstruction();
            _instructions[Opcodes.EOR_Absolute]  = new EorAbsoluteInstruction();
            _instructions[Opcodes.EOR_AbsoluteX] = new EorAbsoluteXInstruction();
            _instructions[Opcodes.EOR_AbsoluteY] = new EorAbsoluteYInstruction();
            _instructions[Opcodes.EOR_IndirectX] = new EorIndirectXInstruction();
            _instructions[Opcodes.EOR_IndirectY] = new EorIndirectYInstruction();

            // --- CMP (Compare Accumulator) ---
            _instructions[Opcodes.CMP_Immediate] = new CmpImmediateInstruction();
            _instructions[Opcodes.CMP_ZeroPage]  = new CmpZeroPageInstruction();
            _instructions[Opcodes.CMP_ZeroPageX] = new CmpZeroPageXInstruction();
            _instructions[Opcodes.CMP_Absolute]  = new CmpAbsoluteInstruction();
            _instructions[Opcodes.CMP_AbsoluteX] = new CmpAbsoluteXInstruction();
            _instructions[Opcodes.CMP_AbsoluteY] = new CmpAbsoluteYInstruction();
            _instructions[Opcodes.CMP_IndirectX] = new CmpIndirectXInstruction();
            _instructions[Opcodes.CMP_IndirectY] = new CmpIndirectYInstruction();

            // --- CPX (Compare X) ---
            _instructions[Opcodes.CPX_Immediate] = new CpxImmediateInstruction();
            _instructions[Opcodes.CPX_ZeroPage]  = new CpxZeroPageInstruction();
            _instructions[Opcodes.CPX_Absolute]  = new CpxAbsoluteInstruction();

            // --- CPY (Compare Y) ---
            _instructions[Opcodes.CPY_Immediate] = new CpyImmediateInstruction();
            _instructions[Opcodes.CPY_ZeroPage]  = new CpyZeroPageInstruction();
            _instructions[Opcodes.CPY_Absolute]  = new CpyAbsoluteInstruction();

            // --- Регистровые пересылки (Register Transfers) ---
            _instructions[Opcodes.TAX] = new TaxInstruction();
            _instructions[Opcodes.TAY] = new TayInstruction();
            _instructions[Opcodes.TXA] = new TxaInstruction();
            _instructions[Opcodes.TYA] = new TyaInstruction();
            _instructions[Opcodes.TSX] = new TsxInstruction();
            _instructions[Opcodes.TXS] = new TxsInstruction();

            // --- Инкременты и декременты (INX, INY, DEX, DEY) ---
            _instructions[Opcodes.INX] = new InxInstruction();
            _instructions[Opcodes.INY] = new InyInstruction();
            _instructions[Opcodes.DEX] = new DexInstruction();
            _instructions[Opcodes.DEY] = new DeyInstruction();

            // --- Инкременты и декременты памяти ---
            _instructions[Opcodes.INC_ZeroPage] = new IncZeroPageInstruction();
            _instructions[Opcodes.DEC_ZeroPage] = new DecZeroPageInstruction();

            // --- Флаги процессора (Processor Status Flags) ---
            _instructions[Opcodes.CLC] = new ClcInstruction();
            _instructions[Opcodes.SEC] = new SecInstruction();
            _instructions[Opcodes.CLI] = new CliInstruction();
            _instructions[Opcodes.SEI] = new SeiInstruction();
            _instructions[Opcodes.CLV] = new ClvInstruction();
            _instructions[Opcodes.CLD] = new CldInstruction();
            _instructions[Opcodes.SED] = new SedInstruction();

            // --- Стек (Stack Operations) ---
            _instructions[Opcodes.PHA] = new PhaInstruction();
            _instructions[Opcodes.PLA] = new PlaInstruction();
            _instructions[Opcodes.PHP] = new PhpInstruction();
            _instructions[Opcodes.PLP] = new PlpInstruction();

            // --- Битовые тесты и сдвиги / вращения ---
            _instructions[Opcodes.BIT_ZeroPage]    = new BitZeroPageInstruction();
            _instructions[Opcodes.ASL_Accumulator] = new AslAccumulatorInstruction();
            _instructions[Opcodes.ASL_ZeroPage]    = new AslZeroPageInstruction();
            _instructions[Opcodes.LSR_Accumulator] = new LsrAccumulatorInstruction();
            _instructions[Opcodes.LSR_ZeroPage]    = new LsrZeroPageInstruction();
            _instructions[Opcodes.ROL_Accumulator] = new RolAccumulatorInstruction();
            _instructions[Opcodes.ROR_Accumulator] = new RorAccumulatorInstruction();

            // --- Управление потоком (JMP, JSR, RTS) ---
            _instructions[Opcodes.JMP_Absolute] = new JmpAbsoluteInstruction();
            _instructions[Opcodes.JMP_Indirect] = new JmpIndirectInstruction();
            _instructions[Opcodes.JSR_Absolute] = new JsrInstruction();
            _instructions[Opcodes.RTS]          = new RtsInstruction();

            // --- Условные переходы (Branches) ---
            _instructions[Opcodes.BEQ] = new BeqInstruction();
            _instructions[Opcodes.BNE] = new BneInstruction();
            _instructions[Opcodes.BMI] = new BmiInstruction();
            _instructions[Opcodes.BPL] = new BplInstruction();
            _instructions[Opcodes.BCC] = new BccInstruction();
            _instructions[Opcodes.BCS] = new BcsInstruction();
            _instructions[Opcodes.BVC] = new BvcInstruction();
            _instructions[Opcodes.BVS] = new BvsInstruction();
        }

        public void Reset()
        {
            _regs.A = 0;
            _regs.X = 0;
            _regs.Y = 0;
            _regs.S = 0xFD;
            _regs.P = 0x24;

            byte low = _memory.Read(0xFFFC);
            byte high = _memory.Read(0xFFFD);
            ushort resetVector = (ushort)((high << 8) | low);

            // Если вектор прописан в ROM — прыгаем по нему, иначе стартуем с 0x8000
            _regs.PC = (resetVector != 0) ? resetVector : (ushort)0x8000;
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

        // Методы выборки операндов (Fetch)
        public byte FetchImmediate(Memory memory, Registers regs)
        {
            return memory.Read(regs.PC++);
        }

        public byte FetchZeroPage(Memory memory, Registers regs)
        {
            return memory.Read(regs.PC++);
        }

        public ushort FetchAbsolute(Memory memory, Registers regs)
        {
            ushort pc = regs.PC;
            byte low = memory.Read(pc++);
            byte high = memory.Read(pc++);
            regs.PC = pc;
            return (ushort)((high << 8) | low);
        }

        // Методы установки флагов с поддержкой Registers regs

        public void SetZeroFlag(Registers regs, byte value)
        {
            if (value == 0) regs.P |= 0x02;
            else regs.P = (byte)(regs.P & ~0x02);
        }


        public void SetNegativeFlag(Registers regs, byte value)
        {
            if ((value & 0x80) != 0) regs.P |= 0x80;
            else regs.P = (byte)(regs.P & ~0x80);
            
        }


        public void SetCarryFlag(Registers regs, bool condition)
        {
            if (condition)
                regs.P |= 0x01; // Бит Carry (C)
            else
                regs.P = (byte)(regs.P & ~0x01);
        }

        public void SetOverflowFlag(Registers regs, bool condition)
        {
            if (condition)
                regs.P |= 0x40; // Бит Overflow (V)
            else
                regs.P = (byte)(regs.P & ~0x40);
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
