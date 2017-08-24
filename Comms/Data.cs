namespace Comms
{
    /// <summary>
    /// Container for primitive types with identifiers for use in packets
    /// </summary>
    public class Data
    {
        #region Info Types
        public const ushort IT_PlayerPositionX   = 0;  // float       : x coordinate of a player
        public const ushort IT_PlayerPositionY   = 1;  // float       : y coordinate of a player
        public const ushort IT_PlayerPositionZ   = 2;  // float       : z coordinate of a player
        public const ushort IT_PlayerID          = 3;  // ushort      : Player ID unique to world server
        public const ushort IT_TimeSeconds       = 4;  // int         : Seconds when packet was sent
        public const ushort IT_TimeMilliseconds  = 5;  // int         : Milliseconds when packet was sent
        public const ushort IT_Username          = 6;  // string      : Username for login auth
        public const ushort IT_Password          = 7;  // string      : Hashed password for login auth
        public const ushort IT_TextMessage       = 8;
        public const ushort IT_ConnectServiceIP  = 9;  // string      : IP Address for Connect service provided by Auth server
        #endregion
        #region Primitive Types
        public const byte INT     = 0;
        public const byte UINT    = 1;
        public const byte SHORT   = 2;
        public const byte USHORT  = 3;
        public const byte FLOAT   = 4;
        public const byte DOUBLE  = 5;
        public const byte LONG    = 6;
        public const byte ULONG   = 7;
        public const byte BOOL    = 8;
        public const byte BYTE    = 9;
        public const byte SBYTE   = 10;
        public const byte CHAR    = 11;
        public const byte STRING  = 12;
        #endregion
        #region Data Variables
        //Only one of these in use per instance of this class
        public int data_int;
        public uint data_uint;
        public short data_short;
        public ushort data_ushort;
        public float data_float;
        public double data_double;
        public long data_long;
        public ulong data_ulong;
        public bool data_bool;
        public byte data_byte;
        public sbyte data_sbyte;
        public char data_char;
        public string data_string;
        #endregion

        public byte type;
        public ushort infoType;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, int data)
        {
            this.type = type;
            this.infoType = infoType;
            data_int = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, uint data)
        {
            this.type = type;
            this.infoType = infoType;
            data_uint = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, short data)
        {
            this.type = type;
            this.infoType = infoType;
            data_short = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, ushort data)
        {
            this.type = type;
            this.infoType = infoType;
            data_ushort = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, float data)
        {
            this.type = type;
            this.infoType = infoType;
            data_float = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, double data)
        {
            this.type = type;
            this.infoType = infoType;
            data_double = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, long data)
        {
            this.type = type;
            this.infoType = infoType;
            data_long = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, ulong data)
        {
            this.type = type;
            this.infoType = infoType;
            data_ulong = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, bool data)
        {
            this.type = type;
            this.infoType = infoType;
            data_bool = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, byte data)
        {
            this.type = type;
            this.infoType = infoType;
            data_byte = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, sbyte data)
        {
            this.type = type;
            this.infoType = infoType;
            data_sbyte = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, char data)
        {
            this.type = type;
            this.infoType = infoType;
            data_char = data;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Primitive type of the data, select from constants in this class</param>
        /// <param name="infoType">Type of information, select from IT_x variables in this class</param>
        /// <param name="data">The data to be sent</param>
        public Data(byte type, ushort infoType, string data)
        {
            this.type = type;
            this.infoType = infoType;
            data_string = data;
        }
    }
}
