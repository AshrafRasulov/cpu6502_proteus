using System.Runtime.InteropServices;

namespace Cpu6502Core
{
    public static class VsmBridge
    {
        private static Cpu6502? _cpu;
        private static Memory? _memory;

        private static readonly string[] PinNames = new string[]
        {
            "GND",  // 1
            "RDY",  // 2
            "OUT1", // 3
            "IRQ",  // 4
            "NC",   // 5
            "NMI",  // 6
            "SYNC", // 7
            "VCC",  // 8
            "A0",   // 9
            "A1",   // 10
            "A2",   // 11
            "A3",   // 12
            "A4",   // 13
            "A5",   // 14
            "A6",   // 15
            "A7",   // 16
            "A8",   // 17
            "A9",   // 18
            "A10",  // 19
            "A11",  // 20
            "GND",  // 21
            "A12",  // 22
            "A13",  // 23
            "A14",  // 24
            "A15",  // 25
            "D7",   // 26
            "D6",   // 27
            "D5",   // 28
            "D4",   // 29
            "D3",   // 30
            "D2",   // 31
            "D1",   // 32
            "D0",   // 33
            "R/W",   // 34
            "NC",   // 35
            "NC",   // 36
            "IN",   // 37
            "SO",   // 38
            "OUT2", // 39
            "RES"   // 40
        };

        [UnmanagedCallersOnly(EntryPoint = "vsm_getpinicount")]
        public static int GetPinCount()
        {
            return PinNames.Length;
        }

        [UnmanagedCallersOnly(EntryPoint = "vsm_getpiname")]
        public static IntPtr GetPinName(int index)
        {
            if (index < 0 || index >= PinNames.Length) return IntPtr.Zero;
            return Marshal.StringToHGlobalAnsi(PinNames[index]);
        }

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





// using System.Runtime.InteropServices;

// namespace Cpu6502Core
// {
//     public static class VsmBridge
//     {
//         private static Cpu6502? _cpu;
//         private static Memory? _memory;

//         private static readonly string[] PinNames = new string[]
//         {
//             "GND",  // 1
//             "RDY",  // 2
//             "OUT1", // 3
//             "IRQ",  // 4
//             "NC",   // 5
//             "NMI",  // 6
//             "SYNC", // 7
//             "VCC",  // 8
//             "A0",   // 9
//             "A1",   // 10
//             "A2",   // 11
//             "A3",   // 12
//             "A4",   // 13
//             "A5",   // 14
//             "A6",   // 15
//             "A7",   // 16
//             "A8",   // 17
//             "A9",   // 18
//             "A10",  // 19
//             "A11",  // 20

//             // Правая сторона (снизу вверх, 21 - 40)
//             "GND",  // 21
//             "A12",  // 22
//             "A13",  // 23
//             "A14",  // 24
//             "A15",  // 25
//             "D7",   // 26
//             "D6",   // 27
//             "D5",   // 28
//             "D4",   // 29
//             "D3",   // 30
//             "D2",   // 31
//             "D1",   // 32
//             "D0",   // 33
//             "RW",   // 34
//             "NC",   // 35
//             "NC",   // 36
//             "IN",   // 37
//             "SO",   // 38
//             "OUT2", // 39
//             "RES"   // 40
//         };

        
//         public static IntPtr GetPinName(int index)
//         {
//             if (index < 0 || index >= PinNames.Length) return IntPtr.Zero;
//             return Marshal.StringToHGlobalAnsi(PinNames[index]);
//         }

//         // Точка входа, которую Proteus вызывает самой первой при старте симуляции
//         [UnmanagedCallersOnly(EntryPoint = "isismain")]
//         public static IntPtr IsisMain(IntPtr device, IntPtr name)
//         {
//             for (int i = 0; i < PinNames.Length; i++)
//             {
//                 IntPtr pinNamePtr = GetPinName(i);
//                 if (pinNamePtr != IntPtr.Zero)
//                 {
//                     Marshal.FreeHGlobal(pinNamePtr);
//                 }
//             }
//             _memory = new Memory();
//             _cpu = new Cpu6502();
//             return GCHandle.ToIntPtr(GCHandle.Alloc(_cpu));
//         }

//         [UnmanagedCallersOnly(EntryPoint = "vsm_init")]
//         public static IntPtr Init()
//         {
//             _memory = new Memory();
//             _cpu = new Cpu6502();
//             return GCHandle.ToIntPtr(GCHandle.Alloc(_cpu));
//         }

//         [UnmanagedCallersOnly(EntryPoint = "vsm_step")]
//         public static unsafe void Step(IntPtr handle, ushort addressBus, byte* dataBus, int rwSignal)
//         {
//             if (_cpu == null || _memory == null || dataBus == null) return;

//             if (rwSignal == 1) // Чтение
//             {
//                 *dataBus = _memory.Read(addressBus);
//             }
//             else // Запись
//             {
//                 _memory.Write(addressBus, *dataBus);
//             }

//             _cpu.Step();
//         }

//         [UnmanagedCallersOnly(EntryPoint = "vsm_free")]
//         public static void Free(IntPtr handle)
//         {
//             if (handle != IntPtr.Zero)
//             {
//                 GCHandle.FromIntPtr(handle).Free();
//             }
//         }
//     }
// }