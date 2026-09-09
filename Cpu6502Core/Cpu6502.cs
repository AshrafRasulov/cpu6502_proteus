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
            _instructions[0xEA] = new NopInstruction();

            // LDA
            _instructions[0xA9] = new LdaImmediateInstruction();
            _instructions[0xA5] = new LdaZeroPageInstruction();
            _instructions[0xAD] = new LdaAbsoluteInstruction();

            // STA
            _instructions[0x85] = new StaZeroPageInstruction();
            _instructions[0x8D] = new StaAbsoluteInstruction();

            // LDX / STX
            _instructions[0xA2] = new LdxImmediateInstruction();
            _instructions[0xA6] = new LdxZeroPageInstruction();
            _instructions[0xAE] = new LdxAbsoluteInstruction();
            _instructions[0x86] = new StxZeroPageInstruction();
            _instructions[0x8E] = new StxAbsoluteInstruction();

            // LDY / STY
            _instructions[0xA0] = new LdyImmediateInstruction();
            _instructions[0xA4] = new LdyZeroPageInstruction();
            _instructions[0xAC] = new LdyAbsoluteInstruction();
            _instructions[0x84] = new StyZeroPageInstruction();
            _instructions[0x8C] = new StyAbsoluteInstruction();

            // ADC
            _instructions[0x69] = new AdcImmediateInstruction();
            _instructions[0x65] = new AdcZeroPageInstruction();

            // AND
            _instructions[0x29] = new AndImmediateInstruction();
            _instructions[0x25] = new AndZeroPageInstruction();

            // ORA
            _instructions[0x09] = new OraImmediateInstruction();
            _instructions[0x05] = new OraZeroPageInstruction();

            // EOR
            _instructions[0x49] = new EorImmediateInstruction();
            _instructions[0x45] = new EorZeroPageInstruction();

            // CMP
            _instructions[0xC9] = new CmpImmediateInstruction();
            _instructions[0xC5] = new CmpZeroPageInstruction();
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
            if (val == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
            if ((val & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
        }

        public void UpdateAddFlags(int result, byte a, byte operand, bool carryIn)
        {
            if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
            if (result > 0xFF) _regs.P |= 0x01; else _regs.P &= (byte)(0x01 ^ 0xFF);
            if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
            
            bool overflow = (~(a ^ operand) & (a ^ result) & 0x80) != 0;
            if (overflow) _regs.P |= 0x40; else _regs.P &= (byte)(0x40 ^ 0xFF);
        }

        public void UpdateCompareFlags(byte reg, byte operand)
        {
            int result = reg - operand;
            if (reg >= operand) _regs.P |= 0x01; else _regs.P &= (byte)(0x01 ^ 0xFF);
            if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
            if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
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




















// namespace Cpu6502Core
// {
//     public class Cpu6502
//     {
//         private readonly Memory _memory = new Memory();
//         private readonly Registers _regs = new Registers();

//         // Словарь инструкций: ключ - HEX-опкод, значение - метод выполнения
//         private readonly Dictionary<byte, Action> _instructions;

//         // Проброс свойств регистров для тестов и внешних модулей
//         public byte     A => _regs.A;
//         public byte     X => _regs.X;
//         public byte     Y => _regs.Y;
//         public byte     SP => _regs.S;
//         public ushort   PC => _regs.PC;
//         public byte     P => _regs.P;

//         public Cpu6502()
//         {
//             Reset();
//         }

//         public void Reset()
//         {
//             _regs.A = 0;
//             _regs.X = 0;
//             _regs.Y = 0;
//             _regs.S = 0xFD;
//             _regs.PC = 0x8000;
//             _regs.P = 0x24;
//         }

//         public byte Read(ushort address) => _memory.Read(address);
        
//         public void Write(ushort address, byte value) => _memory.Write(address, value);

//         public void LoadProgram(byte[] program, ushort startAddress)
//         {
//             _memory.LoadProgram(program, startAddress);
//             _regs.PC = startAddress;
//         }

//         public void Step()
//         {
//             byte opcode = _memory.Read(_regs.PC++);
//             Execute(opcode);
//         }

//         private void Execute(byte opcode)
//         {
//             switch (opcode)
//             {
//                 case 0xEA: // NOP
//                     break;

//                 case 0xA9: // LDA Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         _regs.A = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0xA5: // LDA Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A = _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0xAD: // LDA Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A = _memory.Read(absAddr);
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x85: // STA Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(zeroPageAddr, _regs.A);
//                         break;
//                     }

//                 case 0x8D: // STA Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(absAddr, _regs.A);
//                         break;
//                     }

//                 case 0xA2: // LDX Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         _regs.X = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         UpdateZeroAndNegativeFlags(_regs.X);
//                         break;
//                     }

//                 case 0xA6: // LDX Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.X = _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.X);
//                         break;
//                     }

//                 case 0xAE: // LDX Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.X = _memory.Read(absAddr);
//                         UpdateZeroAndNegativeFlags(_regs.X);
//                         break;
//                     }

//                 case 0x86: // STX Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(zeroPageAddr, _regs.X);
//                         break;
//                     }

//                 case 0x8E: // STX Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(absAddr, _regs.X);
//                         break;
//                     }

//                 case 0xA0: // LDY Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         _regs.Y = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         UpdateZeroAndNegativeFlags(_regs.Y);
//                         break;
//                     }

//                 case 0xA4: // LDY Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.Y = _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.Y);
//                         break;
//                     }

//                 case 0xAC: // LDY Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.Y = _memory.Read(absAddr);
//                         UpdateZeroAndNegativeFlags(_regs.Y);
//                         break;
//                     }

//                 case 0x84: // STY Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(zeroPageAddr, _regs.Y);
//                         break;
//                     }

//                 case 0x8C: // STY Absolute
//                     {
//                         ushort pc = _regs.PC;
//                         ushort absAddr = AddressingModes.GetAbsolute(_memory, ref pc);
//                         _regs.PC = pc;
//                         _memory.Write(absAddr, _regs.Y);
//                         break;
//                     }

//                 case 0x69: // ADC Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         byte operand = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
                        
//                         bool carry = (_regs.P & 0x01) != 0;
//                         int sum = _regs.A + operand + (carry ? 1 : 0);
                        
//                         UpdateAddFlags(sum, _regs.A, operand, carry);
//                         _regs.A = (byte)(sum & 0xFF);
//                         break;
//                     }

//                 case 0x65: // ADC Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         byte operand = _memory.Read(zeroPageAddr);
                        
//                         bool carry = (_regs.P & 0x01) != 0;
//                         int sum = _regs.A + operand + (carry ? 1 : 0);
                        
//                         UpdateAddFlags(sum, _regs.A, operand, carry);
//                         _regs.A = (byte)(sum & 0xFF);
//                         break;
//                     }


//                 case 0x29: // AND Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         byte operand = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A &= operand;
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x25: // AND Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A &= _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x09: // ORA Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         byte operand = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A |= operand;
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x05: // ORA Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A |= _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x49: // EOR Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         byte operand = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A ^= operand;
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0x45: // EOR Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         _regs.A ^= _memory.Read(zeroPageAddr);
//                         UpdateZeroAndNegativeFlags(_regs.A);
//                         break;
//                     }

//                 case 0xC9: // CMP Immediate
//                     {
//                         ushort pc = _regs.PC;
//                         byte operand = AddressingModes.GetImmediate(_memory, ref pc);
//                         _regs.PC = pc;
//                         int result = _regs.A - operand;
//                         UpdateCompareFlags(_regs.A, operand);
//                         break;
//                     }

//                 case 0xC5: // CMP Zero Page
//                     {
//                         ushort pc = _regs.PC;
//                         ushort zeroPageAddr = AddressingModes.GetZeroPage(_memory, ref pc);
//                         _regs.PC = pc;
//                         byte operand = _memory.Read(zeroPageAddr);
//                         UpdateCompareFlags(_regs.A, operand);
//                         break;
//                     }

//                 default:
//                     break;
//             }
            
//         }

//         private void UpdateZeroAndNegativeFlags(byte val)
//         {
//             if (val == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
//             if ((val & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
//         }


//         // Метод для обновления флагов после операции сложения (ADC)
//         private void UpdateAddFlags(int result, byte a, byte operand, bool carryIn)
//         {
//             // Zero flag (бит 1)
//             if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
            
//             // Carry flag (бит 0)
//             if (result > 0xFF) _regs.P |= 0x01; else _regs.P &= (byte)(0x01 ^ 0xFF);
            
//             // Negative flag (бит 7)
//             if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
            
//             // Overflow flag (бит 6) - сигнализирует о переполнении знакового числа
//             bool overflow = (~(a ^ operand) & (a ^ result) & 0x80) != 0;
//             if (overflow) _regs.P |= 0x40; else _regs.P &= (byte)(0x40 ^ 0xFF);
//         }

//         // Метод для обновления флагов после операции сравнения (CMP, CPX, CPY)
//         private void UpdateCompareFlags(byte reg, byte operand)
//         {
//             int result = reg - operand;
//             // Carry (бит 0) устанавливается, если reg >= operand
//             if (reg >= operand) _regs.P |= 0x01; else _regs.P &= (byte)(0x01 ^ 0xFF);
//             // Zero (бит 1)
//             if ((result & 0xFF) == 0) _regs.P |= 0x02; else _regs.P &= (byte)(0x02 ^ 0xFF);
//             // Negative (бит 7)
//             if ((result & 0x80) != 0) _regs.P |= 0x80; else _regs.P &= (byte)(0x80 ^ 0xFF);
//         }

//     }
// }

