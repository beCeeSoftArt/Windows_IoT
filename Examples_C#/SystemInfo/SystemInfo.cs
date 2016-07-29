/* Copyright André Spitzner 2003 - 2016 */
using bcplanet.IOT.Base.Enums;
using System;

namespace bcplanet.IOT.Base.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// The processor architecture
        /// </summary>
        public EProcessorArchitecture ProcessorArchitecture { get; set; }

        /// <summary>
        /// The processor architecture identifier
        /// </summary>
        public ushort ProcessorArchitectureId { get; set; }

        /// <summary>
        /// The processor type
        /// </summary>
        public EProcessorType ProcessorType { get; set; }

        /// <summary>
        /// The processor type identifier
        /// </summary>
        public uint ProcessorTypeId { get; set; }

        /// <summary>
        /// The number of processors
        /// </summary>
        public uint NumberOfProcessors { get; set; }

        /// <summary>
        /// The processor level
        /// </summary>
        public ushort ProcessorLevel { get; set; }

        /// <summary>
        /// The processor revision
        /// </summary>
        public ushort ProcessorRevision { get; set; }

        /// <summary>
        /// The allocation granularity
        /// </summary>
        public uint AllocationGranularity { get; set; }
    };
}
