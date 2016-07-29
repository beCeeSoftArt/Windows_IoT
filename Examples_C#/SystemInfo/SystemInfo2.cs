/* Copyright André Spitzner 2003 - 2016 */
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace bcplanet.IOT.Base.Base
{
    /// <summary>
    /// System information class
    /// </summary>
    public static class SystemInfo2
    {
        /// <summary>
        /// Gets the system family.
        /// </summary>
        /// <value>
        /// The system family.
        /// </value>
        public static string SystemFamily { get; }

        /// <summary>
        /// Gets the system version.
        /// </summary>
        /// <value>
        /// The system version.
        /// </value>
        public static string SystemVersion { get; }

        /// <summary>
        /// Gets the system architecture.
        /// </summary>
        /// <value>
        /// The system architecture.
        /// </value>
        public static string SystemArchitecture { get; }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public static string ApplicationName { get; }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        /// <value>
        /// The application version.
        /// </value>
        public static string ApplicationVersion { get; }

        /// <summary>
        /// Gets the device manufacturer.
        /// </summary>
        /// <value>
        /// The device manufacturer.
        /// </value>
        public static string DeviceManufacturer { get; }

        /// <summary>
        /// Gets the device model.
        /// </summary>
        /// <value>
        /// The device model.
        /// </value>
        public static string DeviceModel { get; }

        /// <summary>
        /// Gets the system sku.
        /// </summary>
        /// <value>
        /// The system sku.
        /// </value>
        public static string SystemSku { get; }

        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>
        /// The operating system.
        /// </value>
        public static string OperatingSystem { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        public static string DeviceName { get; }

        /// <summary>
        /// Gets the install date.
        /// </summary>
        /// <value>
        /// The install date.
        /// </value>
        public static string InstallDate { get; }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public static string DeviceId { get; }

        /// <summary>
        /// Initializes the <see cref="SystemInfo"/> class.
        /// </summary>
        static SystemInfo2()
        {
            // Get the system family name
            var ai = AnalyticsInfo.VersionInfo;
            SystemFamily = ai.DeviceFamily;

            // Get the system version number
            var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            var v = ulong.Parse(sv);
            var v1 = (v & 0xFFFF000000000000L) >> 48;
            var v2 = (v & 0x0000FFFF00000000L) >> 32;
            var v3 = (v & 0x00000000FFFF0000L) >> 16;
            var v4 = (v & 0x000000000000FFFFL);
            SystemVersion = $"{v1}.{v2}.{v3}.{v4}";

            // Get the package architecure
            var package = Package.Current;
            SystemArchitecture = package.Id.Architecture.ToString();

            // Get the user friendly app name
            ApplicationName = package.DisplayName;

            // Get the app version
            var pv = package.Id.Version;
            ApplicationVersion = $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}";
            InstallDate = package.InstalledDate.ToString();

            // Get the device manufacturer and model name
            var eas = new EasClientDeviceInformation();
            DeviceManufacturer = eas.SystemManufacturer;
            DeviceModel = eas.SystemProductName;
            SystemSku = eas.SystemSku;
            OperatingSystem = eas.OperatingSystem;
            DeviceName = eas.FriendlyName;
            DeviceId = eas.Id.ToString();
        }
    }
}