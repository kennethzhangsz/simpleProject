using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lmi3d.GoSdk;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.Zen.Threads;
using Lmi3d.GoSdk.Messages;
using Lmi3d.GoSdk.Tools;
using System.IO;



namespace Demo_Test
{
    class sensorClass
    {
        public  string SENSOR_MAIN_IP = "169.254.1.1";
        public  string SENSOR_BUDDY_IP = "192.168.1.2";

        #region " defineName"
        GoSensor sensor_main;       //sensorMain
        GoSystem system_main;       //sensorSystem
        GoSetup system_main_setup;  //sensorSetup
        GoTools system_setup;     //sensor tools handle

        GoAccelerator accelerator;
        public bool calibrationSuccessfulBl = false; 
        double currentExposre, newExposure;
        short mean = 0;
        GoDataSet dataSet = new GoDataSet();
        Data_ReceivedFlag dataReceivedFlag = new Data_ReceivedFlag();
        Get_Stamp getStamp = new Get_Stamp();
        Get_measurementMsg getMeasurement= new Get_measurementMsg();
        GoDataSet healthData = new GoDataSet();

        public List<string> errorInformation = new List<string>();  //ERROR Information 

        public List<ulong> get_stamp_FrameIndex = new List<ulong>();
        public List<ulong> get_stamp_TimeStamp = new List<ulong>();
        public List<long> get_stamp_EncoderValue = new List<long>();

        public List<ushort> get_measurementMsg_Id = new List<ushort>();
        public List<double> get_measurementMsg_Value = new List<double>();
        public List<double> get_measurementMsg_Decison = new List<double>();

        public List<ulong> syncget_stamp_FrameIndex = new List<ulong>();
        public List<ulong> syncget_stamp_TimeStamp = new List<ulong>();
        public List<long> syncget_stamp_EncoderValue = new List<long>();

        public List<ushort> syncget_measurementMsg_Id = new List<ushort>();
        public List<double> syncget_measurementMsg_Value = new List<double>();
        public List<double> syncget_measurementMsg_Decison = new List<double>();

        public List<int> healthIdicator_id =new List<int>();
        public List<uint> healthIdicator_instance = new List<uint>();
        public List<long> healthIdicator_value = new List<long>();


        public struct Data_ReceivedFlag            //ReceivedFlag/接收数据标志
        {
            public short Stamp;
            public short Measurement;
            public short Alignment;
            public short EdgeMatch;
            public short Health;
            public short Profile;
            public short Range;
            public short RangeIntensity;
            public short Section;
            public short SectionIntensity;
            public short Surface;
            public short Tracheid;
            public short Video;

        }
    public  struct Get_Stamp
        {
            public long Encoder;
            public long EncoderAtZ;
            public uint Reserved32U;
            public ulong Reserved64u;
            public ulong status;
            public ulong Timestamp;
            public ulong FrameIndex;
        }

    public struct Get_measurementMsg
        {
            public ushort Id;
            public double value;
            public double decison;

        }

        enum kStatus_0
        {
            kERROR_STATE = -1000,                                                // Invalid state.
            kERROR_NOT_FOUND = -999,                                             // Item is not found.
            kERROR_COMMAND = -998,                                               // Command not recognized.
            kERROR_PARAMETER = -997,                                             // Parameter is invalid.
            kERROR_UNIMPLEMENTED = -996,                                         // Feature not implemented.
            kERROR_HANDLE = -995,                                                // Handle is invalid.
            kERROR_MEMORY = -994,                                                // Out of memory.
            kERROR_TIMEOUT = -993,                                               // Action timed out.
            kERROR_INCOMPLETE = -992,                                            // Buffer not large enough for data.
            kERROR_STREAM = -991,                                                // Error in stream.
            kERROR_CLOSED = -990,                                                // Resource is no longer avaiable. 
            kERROR_VERSION = -989,                                               // Invalid version number.
            kERROR_ABORT = -988,                                                 // Operation aborted.
            kERROR_ALREADY_EXISTS = -987,                                        // Conflicts with existing item.
            kERROR_NETWORK = -986,                                               // Network setup/resource error.
            kERROR_HEAP = -985,                                                  // Heap error (leak/double-free).
            kERROR_FORMAT = -984,                                                // Data parsing/formatting error. 
            kERROR_READ_ONLY = -983,                                             // Object is read-only (cannot be written).
            kERROR_WRITE_ONLY = -982,                                            // Object is write-only (cannot be read). 
            kERROR_BUSY = -981,                                                  // Agent is busy (cannot service request).
            kERROR_CONFLICT = -980,                                              // State conflicts with another object.
            kERROR_OS = -979,                                                    // Generic error reported by underlying OS.
            kERROR_DEVICE = -978,                                                // Hardware device error.
            kERROR_FULL = -977,                                                  // Resource is already fully utilized.
            kERROR_IN_PROGRESS = -976,                                           // Operation is in progress, but not yet complete.
            kERROR = 0,                                                          // General error. 
            kOK = 1                                                              // Operation successful. 
        }





        #endregion

        public sensorClass(string SENSOR_MAIN_IP)
        {
            if (string.IsNullOrEmpty(SENSOR_MAIN_IP)==false)
            {
                this.SENSOR_MAIN_IP = SENSOR_MAIN_IP;
            }

        }
        /// <summary>
        /// initialize /Connect sensor 
        /// </summary>
        /// <returns></returns>
        public bool  Go_Init_Sensor()
        {
            bool rtn = false;
            try
            {
                KApiLib.Construct();
                GoSdkLib.Construct();
                system_main = new GoSystem();
                accelerator = new GoAccelerator();
                accelerator.Start();
                KIpAddress ipAddress = KIpAddress.Parse(SENSOR_MAIN_IP);
                sensor_main = system_main.FindSensorByIpAddress(ipAddress);
                accelerator.Attach(sensor_main);
                sensor_main.Connect();
                system_main.EnableData(true);
                system_main.SetDataHandler(AsyncReceiveData);
                system_main.Start();
                 

            }
            catch (Exception ex)
            {
                rtn = true;
            }
                    
             return rtn;
        }

        void Init_Data_ReceivedFlag()
        {
            dataReceivedFlag.Alignment = 0;
            dataReceivedFlag.EdgeMatch = 0;
            dataReceivedFlag.Health = 0;
            dataReceivedFlag.Measurement = 0;
            dataReceivedFlag.Profile = 0;
            dataReceivedFlag.Range = 0;
            dataReceivedFlag.RangeIntensity = 0;
            dataReceivedFlag.Section = 0;
            dataReceivedFlag.SectionIntensity = 0;
            dataReceivedFlag.Stamp = 0;
            dataReceivedFlag.Surface = 0;
            dataReceivedFlag.Tracheid = 0;
            dataReceivedFlag.Video = 0;

        }


        void Init_getStamp()
        {
            getStamp.Encoder = 0;
            getStamp.EncoderAtZ = 0;
            getStamp.Reserved32U = 0;
            getStamp.Reserved64u= 0;
            getStamp.Timestamp = 0;
            getStamp.status = 0;
            getStamp.FrameIndex = 0;
          
        }

        void Init_getMeasurement()
        {
            getMeasurement.Id = 0;
            getMeasurement.value = 0;
            getMeasurement.decison = 0;
        }


        void Clear_measurementData()
        {
            get_measurementMsg_Id.Clear();
            get_measurementMsg_Value.Clear();
            get_measurementMsg_Decison.Clear();
        }

        void Clear_getStamp()
        {
            get_stamp_FrameIndex.Clear();
            get_stamp_TimeStamp.Clear();
            get_stamp_EncoderValue.Clear();
        }

        void Clear_healthIdicator()
        {
            healthIdicator_id.Clear();
            healthIdicator_instance.Clear();
            healthIdicator_value.Clear();
        }


        public int SetExposure(double currentExposureAdd)
        {
            int  rtn=9;
             try
            {
               
                system_main_setup = sensor_main.Setup;
                currentExposre = system_main_setup.GetExposure(GoRole.Main);  //set exposure/获取曝光  
                system_main_setup.SetExposure(GoRole.Main, currentExposre + currentExposureAdd);
                sensor_main.Flush();
                rtn = 0;
            }
            catch (KException  ex)
            {
               
                rtn =(int)ex.Status;
            }

            return rtn;
        }
        public int  GetExposure(ref double exposureValue)
        {
            int  rtn = 9;
            try
            {
                system_main_setup = sensor_main.Setup;
                exposureValue = system_main_setup.GetExposure(GoRole.Main);  //get exposure/获取曝光
                rtn = 0;
            }
            catch (KException ex)
            {
                rtn = (int)ex.Status;
            }
            return rtn;
        }

        public int  CopyFile(string sourceFileName)
        {
            int  rtn = 9;
            try
            {
                if (sensor_main.IsConnected())
                {
                    sensor_main.CopyFile(sourceFileName, "newExposure.job");
                    sensor_main.DefaultJob = "newEposure.job";
                }
                else
                {
                    rtn = 91;
                }
            }
            catch (KException ex)
            {
                rtn = (int)ex.Status;
            }
            return rtn;
        }

        public void  DisconnectSensor()            //disconnectSensor and ReadDataStop
        {
            try
            {
                if (sensor_main.IsConnected())
                {
                    sensor_main.Disconnect();
                    sensor_main.Dispose();
                }
                system_main.Stop();
            }
            catch (Exception ex)
            {

            }            
        }
        
        public void AsyncReceiveData(KObject data)
        {
            GoDataSet dataset = (GoDataSet)data;
            for(UInt32 i =0;i<dataset.Count;i++)
            {
                GoDataMsg dataObj = (GoDataMsg)dataset.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.Stamp:
                        {
                            GoStampMsg stampMsg = (GoStampMsg)dataObj;
                            for (UInt32 j = 0; j < stampMsg.Count; j++)
                            {
                                GoStamp stamp = stampMsg.Get(j);
                                getStamp.FrameIndex = stamp.FrameIndex;
                                getStamp.Timestamp = stamp.Timestamp;
                                getStamp.Encoder = stamp.Encoder;
                                get_stamp_FrameIndex.Add(stamp.FrameIndex);
                                get_stamp_TimeStamp.Add(stamp.Timestamp);
                                get_stamp_EncoderValue.Add(stamp.Encoder);
                            }
                            dataReceivedFlag.Stamp = 1;
                 
                        }
                        break;
                    case GoDataMessageType.Measurement:
                        {
                            
                            GoMeasurementMsg measurementMsg = (GoMeasurementMsg)dataObj;
                            for (UInt32 k=0;k<measurementMsg .Count;k++)
                            {
                                GoMeasurementData measurementData = measurementMsg.Get(k);
                                getMeasurement.Id = measurementMsg.Id;
                                getMeasurement.value = measurementData.Value;
                                getMeasurement.decison = measurementData.Decision;
                                get_measurementMsg_Id.Add(measurementMsg.Id);
                                get_measurementMsg_Value.Add(measurementData.Value);
                                get_measurementMsg_Decison.Add(measurementData.Decision);
                            }
                            dataReceivedFlag.Measurement = 1;

                        }
                        break;

                    case GoDataMessageType.Alignment:
                        {
                            GoAlignMsg dataItem = (GoAlignMsg)dataSet.Get(i);
                            if (dataItem.Status == KStatus.Ok)
                            {
                                calibrationSuccessfulBl = true;
                            }
                            else
                            {
                                calibrationSuccessfulBl = false;
                            }
                        }
                        break;
                }
            }

        }

        public void SetupMeasurement()     //Connect to Gocator system.setup measurement data under profile mode
        {
            system_main_setup = sensor_main.Setup;   //retrieve setup handle 



        }


        public void syncReceiveData()
        {
            dataSet = system_main.ReceiveData(30000000);
            for (UInt32 i = 0; i < dataSet.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)dataSet.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.Stamp:
                        {
                            GoStampMsg stampMsg = (GoStampMsg)dataObj;
                            for (UInt32 j = 0; j < stampMsg.Count; j++)
                            {
                                GoStamp stamp = stampMsg.Get(j);
                                getStamp.FrameIndex = stamp.FrameIndex;
                                getStamp.Timestamp = stamp.Timestamp;
                                getStamp.Encoder = stamp.Encoder;
                                get_stamp_FrameIndex.Add(stamp.FrameIndex);
                                get_stamp_TimeStamp.Add(stamp.Timestamp);
                                get_stamp_EncoderValue.Add(stamp.Encoder);
                            }
                            dataReceivedFlag.Stamp = 1;

                        }
                        break;
                    case GoDataMessageType.Measurement:
                        {

                            GoMeasurementMsg measurementMsg = (GoMeasurementMsg)dataObj;
                            for (UInt32 k = 0; k < measurementMsg.Count; k++)
                            {
                                GoMeasurementData measurementData = measurementMsg.Get(k);
                                getMeasurement.Id = measurementMsg.Id;
                                getMeasurement.value = measurementData.Value;
                                getMeasurement.decison = measurementData.Decision;
                                get_measurementMsg_Id.Add(measurementMsg.Id);
                                get_measurementMsg_Value.Add(measurementData.Value);
                                get_measurementMsg_Decison.Add(measurementData.Decision);
                            }
                            dataReceivedFlag.Measurement = 1;

                        }
                        break;
                    case GoDataMessageType.Alignment:
                        {
                            GoAlignMsg dataItem = (GoAlignMsg)dataSet.Get(i);
                            if (dataItem.Status == KStatus.Ok)
                            {
                                calibrationSuccessfulBl = true;
                            }
                            else
                            {
                                calibrationSuccessfulBl = false;
                            }

                        }
                        break;

                }
            }

        }

        #region Calibration disk

        int CalibrationSensor(int goalignmentType,int alignmentmovingTarget,double diskDiameter,double diskHeight)
        {
            try
            {
                GoSetup setup = sensor_main.Setup;
                setup.AlignmentType = GoAlignmentType.Moving;
                setup.AlignmentMovingTarget = GoAlignmentTarget.Disk;
                setup.DiskDiameter = diskDiameter;
                setup.DiskHeight = diskHeight;
                setup.AlignmentEncoderCalibrateEnabled = true;
                sensor_main.Align();           //wait for calibration disk to be scanned

                dataSet = system_main.ReceiveData(300000000);
                for (UInt32 i = 0; i < dataSet.Count; i++)
                {
                    GoAlignMsg dataItem = (GoAlignMsg)dataSet.Get(i);
                    if (dataItem.MessageType == GoDataMessageType.Alignment)
                    {
                        if (dataItem.Status == KStatus.Ok)
                        {

                            calibrationSuccessfulBl = true;
                        }
                        else
                        {
                            calibrationSuccessfulBl = false;
                        }
                    }
                }
            }
            catch (KException ex)
            {
                if (ex.Status == KStatus.ErrorTimeout)
                {
                    errorInformation.Add(DateTime.Now.ToString("yyyy:MM:dd HHmmss")+"calibration disk time out");
                }
                else if (ex.Status != KStatus.Ok)
                {
                    errorInformation.Add(DateTime.Now.ToString("yyyy:MM:dd HHmmss")+"calibration error code" +ex.Status);
                }
            }

           
            return 0;
        }
        #endregion

        #region  backupGocator
        string backupfile = "SystemBackup.bin";
        int BackupSenorParamter(string backupfileName)
        {
            int rtn = 9;
            try
            {
                if (sensor_main.IsConnected())
                {
                    sensor_main.Backup(backupfileName);
                }
            }
            catch (KException ex)
            {
                rtn = (int)ex.Status;
            }
            return rtn;
        }

        int RestoreSensorParamter(string restoreFileName)
        {
            int rtn = 9;
            try
            {
                if (sensor_main.IsConnected())
                {
                    sensor_main.Restore(restoreFileName);
                }
            }
            catch (KException ex)
            {
                rtn = (int)ex.Status;
            }
            return rtn;
        }
        #endregion

        int ReceiveHealthData()
        {
            int rtn = 9;
            try
            {
                healthData = system_main.ReceiveHealth(3000000);
                for(UInt32 j =0;j<healthData .Count;j++)
                {
                    GoHealthMsg healthmsg = (GoHealthMsg)healthData.Get(j);
                    for (UInt32 k=0; k < healthmsg.Count; k++)
                    {
                        GoIndicator healthIndicator = healthmsg.Get(k);
                        healthIdicator_id.Add(healthIndicator.id);
                        healthIdicator_instance.Add(healthIndicator.instance);
                        healthIdicator_value.Add(healthIndicator.value);
                    }
                    
                }
            }
            catch(KException ex)
            {
                rtn = (int)ex.Status;
            }
            return rtn;
        }
























    }
}
