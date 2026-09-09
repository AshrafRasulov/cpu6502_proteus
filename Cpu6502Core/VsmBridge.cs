using System.Runtime.InteropServices;

namespace Cpu6502Core
{
    public static class VsmBridge
    {
        private static Cpu6502? _cpu;
        private static Memory? _memory;

        private static readonly string[] PinNames = new string[]
        {
            "GND", "RDY", "OUT1", "IRQ", "NC", "NMI", "SYNC", "VCC",    // 1-8
            "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7",             // 9-16
            "A8", "A9", "A10", "A11", "A12", "A13", "A14", "A15",       // 17-24
            "D7", "D6", "D5", "D4", "D3", "D2", "D1", "D0",             // 25-32
            "RW", "NC", "NC", "IN", "SO", "OUT2", "RES"                 // 33-40 
        };

        // Точка входа, которую Proteus вызывает самой первой при старте симуляции
        [UnmanagedCallersOnly(EntryPoint = "isismain")]
        public static IntPtr IsisMain(IntPtr device, IntPtr name)
        {
            _memory = new Memory();
            _cpu = new Cpu6502();
            return GCHandle.ToIntPtr(GCHandle.Alloc(_cpu));
        }

        [UnmanagedCallersOnly(EntryPoint = "vsm_init")]
        public static IntPtr Init()
        {
            _memory = new Memory();
            _cpu = new Cpu6502();
            return GCHandle.ToIntPtr(GCHandle.Alloc(_cpu));
        }

        [UnmanagedCallersOnly(EntryPoint = "vsm_step")]
        public static unsafe void Step(IntPtr handle, ushort addressBus, byte* dataBus, int rwSignal)
        {
            if (_cpu == null || _memory == null || dataBus == null) return;

            if (rwSignal == 1) // Чтение
            {
                *dataBus = _memory.Read(addressBus);
            }
            else // Запись
            {
                _memory.Write(addressBus, *dataBus);
            }

            _cpu.Step();
        }

        [UnmanagedCallersOnly(EntryPoint = "vsm_free")]
        public static void Free(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                GCHandle.FromIntPtr(handle).Free();
            }
        }
    }
}