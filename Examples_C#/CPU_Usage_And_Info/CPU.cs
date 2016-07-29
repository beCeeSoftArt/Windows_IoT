/* Copyright André Spitzner 2003 - 2016 */
using bcplanet.IOT.Base.Base;
using bcplanet.IOT.Base.Enums;
using System.Runtime.InteropServices;

namespace System
{
    public class CPU
    {
        #region CPU Info

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct _SYSTEM_INFO
        {
            /// <summary>
            /// The processor architecture
            /// </summary>
            public ushort wProcessorArchitecture;

            /// <summary>
            /// The reserved
            /// </summary>
            public ushort wReserved;

            /// <summary>
            /// The page size
            /// </summary>
            public uint dwPageSize;

            /// <summary>
            /// The minimum application address
            /// </summary>
            public IntPtr lpMinimumApplicationAddress;

            /// <summary>
            /// The maximum application address
            /// </summary>
            public IntPtr lpMaximumApplicationAddress;

            /// <summary>
            /// The active processor mask
            /// </summary>
            public UIntPtr dwActiveProcessorMask;

            /// <summary>
            /// The number of processors
            /// </summary>
            public uint dwNumberOfProcessors;

            /// <summary>
            /// The processor type
            /// </summary>
            public uint dwProcessorType;

            /// <summary>
            /// The allocation granularity
            /// </summary>
            public uint dwAllocationGranularity;

            /// <summary>
            /// The processor level
            /// </summary>
            public ushort wProcessorLevel;

            /// <summary>
            /// The processor revision
            /// </summary>
            public ushort wProcessorRevision;
        };

        /// <summary>
        /// Gets the native system information.
        /// </summary>
        /// <param name="lpSystemInfo">The lp system information.</param>
        [DllImport("kernel32.dll")]
        static extern void GetNativeSystemInfo(ref _SYSTEM_INFO lpSystemInfo);

        /// <summary>
        /// Determines whether [is processor feature present] [the specified processor feature].
        /// </summary>
        /// <param name="processorFeature">The processor feature.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        static extern bool IsProcessorFeaturePresent(uint processorFeature);


        /// <summary>
        /// The sys information
        /// </summary>
        private static _SYSTEM_INFO _sysInfo;

        /// <summary>
        /// The information
        /// </summary>
        private static SystemInfo _sInfo;

        /// <summary>
        /// Determines whether [is processor feature present] [the specified feature].
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns></returns>
        public static bool IsProcessorFeaturePresent(EProcessorFeature feature)
        {
            return IsProcessorFeaturePresent((uint)feature);
        }

        /// <summary>
        /// Gets the native information.
        /// </summary>
        /// <value>
        /// The native information.
        /// </value>
        public static SystemInfo NativeInfo
        {
            get
            {
                if (_sInfo == null)
                {
                    _sysInfo = new _SYSTEM_INFO();
                    GetNativeSystemInfo(ref _sysInfo);
                    _sInfo = new SystemInfo()
                    {
                        ProcessorTypeId = _sysInfo.dwProcessorType,
                        ProcessorArchitectureId = _sysInfo.wProcessorArchitecture,
                        ProcessorLevel = _sysInfo.wProcessorLevel,
                        ProcessorRevision = _sysInfo.wProcessorRevision,
                        NumberOfProcessors = _sysInfo.dwNumberOfProcessors,
                        AllocationGranularity = _sysInfo.dwAllocationGranularity,
                        ProcessorArchitecture = Enum.IsDefined(typeof(EProcessorArchitecture), _sysInfo.wProcessorArchitecture) ? (EProcessorArchitecture)_sysInfo.wProcessorArchitecture : EProcessorArchitecture.UNKNOWN,
                        ProcessorType = Enum.IsDefined(typeof(EProcessorType), _sysInfo.dwProcessorType) ? (EProcessorType)_sysInfo.dwProcessorType : EProcessorType.UNKNOWN
                    };
                }
                return _sInfo;
            }
        }

        #endregion

        #region CPU Usage

        /// <summary>
        /// Gets the system times.
        /// </summary>
        /// <param name="idleTime">The idle time.</param>
        /// <param name="kernelTime">The kernel time.</param>
        /// <param name="userTime">The user time.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool GetSystemTimes(out long idleTime, out long kernelTime, out long userTime);

        /// <summary>
        /// The last user time
        /// </summary>
        private static long _lastUserTime;
        /// <summary>
        /// The last kernel time
        /// </summary>
        private static long _lastKernelTime;
        /// <summary>
        /// The last idle time
        /// </summary>
        private static long _lastIdleTime;

        /// <summary>
        /// Gets the system usage.
        /// </summary>
        /// <returns>System usage in percent</returns>
        public static double GetSystemUsage()
        {
            long idleTime;
            long kernelTime;
            long userTime;

            // Get system times
            GetSystemTimes(out idleTime, out kernelTime, out userTime);

            // Get current time diffs
            var usr = userTime - _lastUserTime;
            var ker = kernelTime - _lastKernelTime;
            var idl = idleTime - _lastIdleTime;

            // Remember last times
            _lastUserTime = userTime;
            _lastKernelTime = kernelTime;
            _lastIdleTime = idleTime;

            // Calculate system time
            var sys = ker + usr;
            // Calculate CPU usage
            var cpu = (sys - idl) * 100.0 / sys;
            return cpu;
        }

        #endregion
    }
}