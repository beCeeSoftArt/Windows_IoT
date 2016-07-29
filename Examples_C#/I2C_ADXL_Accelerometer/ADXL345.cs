/* Copyright André Spitzner 2003 - 2016 */
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using bcplanet.IOT.Base.Base;
using bcplanet.IOT.Base.Enums;

namespace bcplanet.IOT.Base.Sensors
{
    /// <summary>
    /// ADXL345 Device
    /// </summary>
    public class ADXL345
    {
        /// <summary>
        /// Address of the Power Control register
        /// </summary>
        private const byte AccelRegPowerControl = 0x2D;

        /// <summary>
        /// Address of the Data Format register
        /// </summary>
        private const byte AccelRegDataFormat = 0x31;

        /// <summary>
        /// Address of the X Axis data register
        /// </summary>
        private const byte AccelStartRegisterX = 0x32;

        /// <summary>
        /// The Address of the Y Axis data register
        /// </summary>
        private const byte AccelStartRegisterY = 0x34;

        /// <summary>
        /// Address of the Z Axis data register 
        /// </summary>
        private const byte AccelStartRegisterZ = 0x36;

        /// <summary>
        /// The ADXL345 has 10 bit resolution giving 1024 unique values
        /// </summary>
        private const int AcceleratorResolution = 1024;

        /// <summary>
        /// The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G
        /// </summary>
        private int _AccelerometerDynamicRangeG = 8;

        /// <summary>
        /// Gets or sets the i2c address.
        /// </summary>
        /// <value>
        /// The I2C address.
        /// </value>
        public int I2CDeviceAddress { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized => _IsInitialized;

        /// <summary>
        /// The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G
        /// </summary>
        public int AccelerometerDynamicRangeG
        {
            get { return _AccelerometerDynamicRangeG; }
            set { _AccelerometerDynamicRangeG = value; }
        }

        /// <summary>
        /// The i2c device
        /// </summary>
        private I2cDevice _I2CDevice;

        /// <summary>
        /// The is initialized
        /// </summary>
        private bool _IsInitialized;

        /// <summary>
        /// Syncronisation object
        /// </summary>
        private static readonly object _SyncRoot = new object();

        /// <summary>
        /// Own instance
        /// </summary>
        private static volatile ADXL345 _Instance;

        /// <summary>
        /// Instance accessor
        /// </summary>
        public static ADXL345 Instance
        {
            get
            {
                if (_Instance != null)
                    return _Instance;

                lock (_SyncRoot)
                    if (_Instance == null)
                        _Instance = new ADXL345();
                return _Instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ADXL345" /> class.
        /// </summary>
        /// <param name="deviceAdress">The device adress.</param>
        public ADXL345(byte deviceAdress = 0x53)
        {
            I2CDeviceAddress = deviceAdress;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            if (!_IsInitialized)
                return DeviceInitialized();
            return _IsInitialized;
        }

        /// <summary>
        /// Devices the initialized.
        /// </summary>
        /// <returns></returns>
        private bool DeviceInitialized()
        {
            if (_IsInitialized)
                return true;
            
            var result = false;
            try
            {
                var mre = new System.Threading.ManualResetEvent(false);
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        var settings = new I2cConnectionSettings(I2CDeviceAddress)
                        {
                            BusSpeed = I2cBusSpeed.FastMode
                        };

                        string aqs = I2cDevice.GetDeviceSelector();
                        var dis = await DeviceInformation.FindAllAsync(aqs);
                        if (dis.Count == 0)
                        {
                            LogManager.WriteLog(ELogTypes.Error, text: "No I2C controllers were found on the system.");
                        }
                        else
                        {
                            _I2CDevice = await I2cDevice.FromIdAsync(dis[0].Id, settings);

                            if (_I2CDevice == null)
                            {
                                LogManager.WriteLog(ELogTypes.Error, text: string.Format(
                                    "Slave address {0} on I2C Controller {1} is currently in use by " +
                                    "another application. Please ensure that no other applications are using I2C.",
                                    settings.SlaveAddress,
                                    dis[0].Id));
                            }
                            else
                            {
                                //Initialize
                                try
                                {
                                    // 0x01 sets range to +- 4Gs
                                    WriteCommand(AccelRegDataFormat, 0x01);
                                    // 0x08 puts the accelerometer into measurement mode 
                                    WriteCommand(AccelRegPowerControl, 0x08);

                                    _IsInitialized = true;
                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                    LogManager.WriteLog(ELogTypes.Error, ex,
                                        "Failed to communicate with device: " + ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(ELogTypes.Error, ex, "I2C Initialization Failed");
                    }
                    finally
                    {
                        mre.Set();
                    }
                });
                mre.WaitOne();

            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex, "I2C Initialization Failed");
            }
            return result;
        }

        /// <summary>
        /// Writes the command.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <param name="value">The value.</param>
        private void WriteCommand(byte register, byte value)
        {
            _I2CDevice.Write(new[] { register, value });
        }

        /// <summary>
        /// Gets the acceleration.
        /// </summary>
        /// <returns></returns>
        public Acceleration GetAcceleration()
        {
            if (_IsInitialized)
            {
                // Ratio of raw int values to G units
                int unitsPerG = AcceleratorResolution / _AccelerometerDynamicRangeG;
                // Register address we want to read from 
                byte[] addressBuf = { AccelStartRegisterX };
                // We read 6 bytes sequentially to get all 3 two-byte axes registers in one read
                var readBuffer = new byte[6];

                // Read all 3 axes
                _I2CDevice.WriteRead(addressBuf, readBuffer);

                // In order to get the raw 16-bit data values, we need to concatenate two 8-bit bytes from the I2C read for each axis.
                // We accomplish this by using the BitConverter class.
                var accelerationRawX = BitConverter.ToInt16(readBuffer, 0);
                var accelerationRawY = BitConverter.ToInt16(readBuffer, 2);
                var accelerationRawZ = BitConverter.ToInt16(readBuffer, 4);

                // Convert raw values to G's
                Acceleration accel = new Acceleration
                {
                    X = (double)accelerationRawX / unitsPerG,
                    Y = (double)accelerationRawY / unitsPerG,
                    Z = (double)accelerationRawZ / unitsPerG
                };

                return accel;
            }
            return null;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _I2CDevice.Dispose();
            _IsInitialized = false;
        }
    }
}