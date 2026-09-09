using System;
using Cpu6502Core.Interfaces;

namespace Cpu6502Core
{
    public static class Opcodes
    {
        // NOP
        public const byte NOP = 0xEA;

        // LDA
        public const byte LDA_Immediate = 0xA9;
        public const byte LDA_ZeroPage  = 0xA5;
        public const byte LDA_ZeroPageX = 0xB5;
        public const byte LDA_Absolute  = 0xAD;
        public const byte LDA_AbsoluteX = 0xBD;
        public const byte LDA_AbsoluteY = 0xB9;
        public const byte LDA_IndirectX = 0xA1;
        public const byte LDA_IndirectY = 0xB1;

        // STA
        public const byte STA_ZeroPage  = 0x85;
        public const byte STA_ZeroPageX = 0x95;
        public const byte STA_Absolute  = 0x8D;
        public const byte STA_AbsoluteX = 0x9D;
        public const byte STA_AbsoluteY = 0x99;
        public const byte STA_IndirectX = 0x81;
        public const byte STA_IndirectY = 0x91;

        // LDX / STX
        public const byte LDX_Immediate = 0xA2;
        public const byte LDX_ZeroPage  = 0xA6;
        public const byte LDX_ZeroPageY = 0xB6;
        public const byte LDX_Absolute  = 0xAE;
        public const byte LDX_AbsoluteY = 0xBE;
        public const byte STX_ZeroPage  = 0x86;
        public const byte STX_ZeroPageY = 0x96;
        public const byte STX_Absolute  = 0x8E;

        // LDY / STY
        public const byte LDY_Immediate = 0xA0;
        public const byte LDY_ZeroPage  = 0xA4;
        public const byte LDY_ZeroPageX = 0xB4;
        public const byte LDY_Absolute  = 0xAC;
        public const byte LDY_AbsoluteX = 0xBC;
        public const byte STY_ZeroPage  = 0x84;
        public const byte STY_ZeroPageX = 0x94;
        public const byte STY_Absolute  = 0x8C;

        // ADC
        public const byte ADC_Immediate = 0x69;
        public const byte ADC_ZeroPage  = 0x65;
        public const byte ADC_ZeroPageX = 0x75;
        public const byte ADC_Absolute  = 0x6D;
        public const byte ADC_AbsoluteX = 0x7D;
        public const byte ADC_AbsoluteY = 0x79;
        public const byte ADC_IndirectX = 0x61;
        public const byte ADC_IndirectY = 0x71;

        // SBC
        public const byte SBC_Immediate = 0xE9;
        public const byte SBC_ZeroPage  = 0xE5;
        public const byte SBC_ZeroPageX = 0xF5;
        public const byte SBC_Absolute  = 0xED;
        public const byte SBC_AbsoluteX = 0xFD;
        public const byte SBC_AbsoluteY = 0xF9;
        public const byte SBC_IndirectX = 0xE1;
        public const byte SBC_IndirectY = 0xF1;

        // AND
        public const byte AND_Immediate = 0x29;
        public const byte AND_ZeroPage  = 0x25;
        public const byte AND_ZeroPageX = 0x35;
        public const byte AND_Absolute  = 0x2D;
        public const byte AND_AbsoluteX = 0x3D;
        public const byte AND_AbsoluteY = 0x39;
        public const byte AND_IndirectX = 0x21;
        public const byte AND_IndirectY = 0x31;

        // ORA
        public const byte ORA_Immediate = 0x09;
        public const byte ORA_ZeroPage  = 0x05;
        public const byte ORA_ZeroPageX = 0x15;
        public const byte ORA_Absolute  = 0x0D;
        public const byte ORA_AbsoluteX = 0x1D;
        public const byte ORA_AbsoluteY = 0x19;
        public const byte ORA_IndirectX = 0x01;
        public const byte ORA_IndirectY = 0x11;

        // EOR
        public const byte EOR_Immediate = 0x49;
        public const byte EOR_ZeroPage  = 0x45;
        public const byte EOR_ZeroPageX = 0x55;
        public const byte EOR_Absolute  = 0x4D;
        public const byte EOR_AbsoluteX = 0x5D;
        public const byte EOR_AbsoluteY = 0x59;
        public const byte EOR_IndirectX = 0x41;
        public const byte EOR_IndirectY = 0x51;

        // CMP
        public const byte CMP_Immediate = 0xC9;
        public const byte CMP_ZeroPage  = 0xC5;
        public const byte CMP_ZeroPageX = 0xD5;
        public const byte CMP_Absolute  = 0xCD;
        public const byte CMP_AbsoluteX = 0xDD;
        public const byte CMP_AbsoluteY = 0xD9;
        public const byte CMP_IndirectX = 0xC1;
        public const byte CMP_IndirectY = 0xD1;

        // CPX / CPY
        public const byte CPX_Immediate = 0xE0;
        public const byte CPX_ZeroPage  = 0xE4;
        public const byte CPX_Absolute  = 0xEC;
        public const byte CPY_Immediate = 0xC0;
        public const byte CPY_ZeroPage  = 0xC4;
        public const byte CPY_Absolute  = 0xCC;

        // Bitwise / Shifts & Rotates
        public const byte BIT_ZeroPage  = 0x24;
        public const byte BIT_Absolute  = 0x2C;
        
        public const byte ASL_Accumulator = 0x0A;
        public const byte ASL_ZeroPage    = 0x06;
        public const byte ASL_ZeroPageX   = 0x16;
        public const byte ASL_Absolute    = 0x0E;
        public const byte ASL_AbsoluteX   = 0x1E;

        public const byte LSR_Accumulator = 0x4A;
        public const byte LSR_ZeroPage    = 0x46;
        public const byte LSR_ZeroPageX   = 0x56;
        public const byte LSR_Absolute    = 0x4E;
        public const byte LSR_AbsoluteX   = 0x5E;

        public const byte ROL_Accumulator = 0x2A;
        public const byte ROL_ZeroPage    = 0x26;
        public const byte ROL_ZeroPageX   = 0x36;
        public const byte ROL_Absolute    = 0x2E;
        public const byte ROL_AbsoluteX   = 0x3E;

        public const byte ROR_Accumulator = 0x6A;
        public const byte ROR_ZeroPage    = 0x66;
        public const byte ROR_ZeroPageX   = 0x76;
        public const byte ROR_Absolute    = 0x6E;
        public const byte ROR_AbsoluteX   = 0x7E;

        // Increments & Decrements
        public const byte INC_ZeroPage  = 0xE6;
        public const byte INC_ZeroPageX = 0xF6;
        public const byte INC_Absolute  = 0xEE;
        public const byte INC_AbsoluteX = 0xFE;

        public const byte DEC_ZeroPage  = 0xC6;
        public const byte DEC_ZeroPageX = 0xD6;
        public const byte DEC_Absolute  = 0xCE;
        public const byte DEC_AbsoluteX = 0xDE;

        public const byte INX = 0xE8;
        public const byte INY = 0xC8;
        public const byte DEX = 0xCA;
        public const byte DEY = 0x88;

        // Register Transfers
        public const byte TAX = 0xAA;
        public const byte TAY = 0xA8;
        public const byte TXA = 0x8A;
        public const byte TYA = 0x98;
        public const byte TSX = 0xBA;
        public const byte TXS = 0x9A;

        // Control Flow & Branching
        public const byte JMP_Absolute  = 0x4C;
        public const byte JMP_Indirect  = 0x6C;
        public const byte JSR_Absolute  = 0x20;
        public const byte RTS           = 0x60;
        public const byte RTI           = 0x40;
        
        public const byte BEQ           = 0xF0;
        public const byte BNE           = 0xD0;
        public const byte BMI           = 0x30;
        public const byte BPL           = 0x10;
        public const byte BCS           = 0xB0;
        public const byte BCC           = 0x90;
        public const byte BVS           = 0x70;
        public const byte BVC           = 0x50;

        // Stack Operations
        public const byte PHA = 0x48;
        public const byte PLA = 0x68;
        public const byte PHP = 0x08;
        public const byte PLP = 0x28;

        // Status Flag Changes (Implied)
        public const byte CLC = 0x18;
        public const byte SEC = 0x38;
        public const byte CLI = 0x58;
        public const byte SEI = 0x78;
        public const byte CLV = 0xB8;
        public const byte CLD = 0xD8;
        public const byte SED = 0xF8;

        // System / Interrupts
        public const byte BRK = 0x00;


        // Хелпер для массовой регистрации
        public static void RegisterGroup(IInstruction[] instructionsArray, byte[] opcodes, Func<IInstruction> factory)
        {
            foreach (var op in opcodes)
            {
                instructionsArray[op] = factory();
            }
        }
    }
}