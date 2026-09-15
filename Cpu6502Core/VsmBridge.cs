using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cpu6502Core
{
    public static class VsmBridge
    {
        private static Cpu6502? _cpu;
        private static Memory? _memory;

        // Таблица указателей функций C++ интерфейса IDSIMMODEL для Proteus
        private static IntPtr _vtablePtr = IntPtr.Zero;
        private static readonly object _lock = new object();

        private static readonly string[] PinNames = new string[]
        {
            // Левая сторона (выводы 1 - 20)
            "GND",    // 1
            "RDY",    // 2
            "OUT1",   // 3 (PHI1 out)
            "$IRQ$",  // 4 (Инверсный IRQ)
            "NC",     // 5
            "$NMI$",  // 6 (Инверсный NMI)
            "SYNC",   // 7
            "VCC",    // 8
            "A0",     // 9
            "A1",     // 10
            "A2",     // 11
            "A3",     // 12
            "A4",     // 13
            "A5",     // 14
            "A6",     // 15
            "A7",     // 16
            "A8",     // 17
            "A9",     // 18
            "A10",    // 19
            "A11",    // 20

            // Правая сторона (выводы 21 - 40, снизу вверх)
            "GND",    // 21
            "A12",    // 22
            "A13",    // 23
            "A14",    // 24
            "A15",    // 25
            "D7",     // 26
            "D6",     // 27
            "D5",     // 28
            "D4",     // 29
            "D3",     // 30
            "D2",     // 31
            "D1",     // 32
            "D0",     // 33
            "R/$W$",  // 34 (Read / Write)
            "NC",     // 35
            "NC",     // 36
            "IN",     // 37 (PHI0 in)
            "SO",     // 38
            "OUT2",   // 39 (PHI2 out)
            "$RES$"   // 40 (Инверсный Reset)
        };

        // =========================================================================
        // СЕКЦИЯ 1: Официальный интерфейс Proteus VSM (C++ IDSIMMODEL vtable на C#)
        // =========================================================================

        [UnmanagedCallersOnly(EntryPoint = "createdsimmodel", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe IntPtr CreateDsimModel(IntPtr device, IntPtr ils)
        {
            // Обязательная авторизация в сервере лицензий Proteus:
            if (ils != IntPtr.Zero)
            {
                try
                {
                    IntPtr* ilsVTable = *(IntPtr**)ils;
                    // Метод authorize находится под индексом 0 в vtable ILICENCESERVER
                    var authorizeFn = (delegate* unmanaged[Cdecl]<IntPtr, uint, int>)ilsVTable[0];
                    // 0x80808081 — стандартный ключ Proteus VSM для пользовательских моделей
                    authorizeFn(ils, 0x80808081);
                }
                catch
                {
                    // Игнорируем возможные исключения
                }
            }

            EnsureVTableInitialized();

            _memory = new Memory();
            _cpu = new Cpu6502();
            _cpu.Reset();

            // Выделяем память под объект C++ класса (первый указатель — ссылка на vtable)
            IntPtr* modelInstance = (IntPtr*)Marshal.AllocHGlobal(IntPtr.Size * 2);
            *modelInstance = _vtablePtr;

            return (IntPtr)modelInstance;
        }

        [UnmanagedCallersOnly(EntryPoint = "deletedsimmodel", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void DeleteDsimModel(IntPtr model)
        {
            if (model != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(model);
            }
        }

        private static unsafe void EnsureVTableInitialized()
        {
            lock (_lock)
            {
                if (_vtablePtr != IntPtr.Zero) return;

                // Создаём vtable из 7 виртуальных методов интерфейса IDSIMMODEL
                _vtablePtr = Marshal.AllocHGlobal(IntPtr.Size * 7);
                IntPtr* vt = (IntPtr*)_vtablePtr;

                vt[0] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, byte*, int>)&DsimIsDigital;
                vt[1] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)&DsimSetup;
                vt[2] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, int, void>)&DsimRunCtrl;
                vt[3] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, double, int, void>)&DsimActuate;
                vt[4] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, double, IntPtr, int>)&DsimIndicate;
                vt[5] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, long, int, void>)&DsimSimulate;
                vt[6] = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, long, int, void>)&DsimCallback;
            }
        }

        // Методы C++ интерфейса IDSIMMODEL:
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static unsafe int DsimIsDigital(IntPtr thisPtr, byte* pinname) => 1; // Все пины цифровые

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void DsimSetup(IntPtr thisPtr, IntPtr instance, IntPtr dsim)
        {
            if (_cpu == null)
            {
                _memory = new Memory();
                _cpu = new Cpu6502();
                _cpu.Reset();
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void DsimRunCtrl(IntPtr thisPtr, int mode)
        {
            // Режим старта симулятора (RM_START = 0)
            if (mode == 0 && _cpu != null)
            {
                _cpu.Reset();
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void DsimActuate(IntPtr thisPtr, double time, int newstate) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static int DsimIndicate(IntPtr thisPtr, double time, IntPtr newstate) => 0;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void DsimSimulate(IntPtr thisPtr, long time, int mode)
        {
            if (_cpu != null)
            {
                try
                {
                    _cpu.Step();
                }
                catch
                {
                    // Защита от аварийного завершения симулятора
                }
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void DsimCallback(IntPtr thisPtr, long time, int eventid) { }


        // =========================================================================
        // СЕКЦИЯ 2: Вспомогательные C-экспорты и тесты
        // =========================================================================

        [UnmanagedCallersOnly(EntryPoint = "vsm_getpinicount")]
        public static int GetPinCount() => PinNames.Length;

        [UnmanagedCallersOnly(EntryPoint = "vsm_getpiname")]
        public static IntPtr GetPinName(int index)
        {
            if (index < 0 || index >= PinNames.Length) return IntPtr.Zero;
            return Marshal.StringToHGlobalAnsi(PinNames[index]);
        }

        private static IntPtr InitializeCore()
        {
            _memory = new Memory();
            _cpu = new Cpu6502();
            _cpu.Reset();
            return GCHandle.ToIntPtr(GCHandle.Alloc(_cpu));
        }

        [UnmanagedCallersOnly(EntryPoint = "isismain")]
        public static IntPtr IsisMain(IntPtr device, IntPtr name) => InitializeCore();

        [UnmanagedCallersOnly(EntryPoint = "vsm_init")]
        public static IntPtr Init() => InitializeCore();


        [UnmanagedCallersOnly(EntryPoint = "vsm_step")]
        public static unsafe void Step(IntPtr handle, ushort addressBus, byte* dataBus, int rwSignal)
        {
            if (_cpu == null || _memory == null || dataBus == null) return;

            if (rwSignal == 1)
            {
                *dataBus = _memory.Read(addressBus);
            }
            else
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