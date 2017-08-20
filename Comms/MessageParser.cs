using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comms
{
    /// <summary>
    /// Class holding methods for parsing packets
    /// </summary>
    public static class MessageParser
    {
        public static Data[] ParseMessage(byte[] message)
        {
            List<Data> data = new List<Data>();
            int index = 8;

            try
            {
                data.Add(new Data(Data.INT, Data.IT_TimeSeconds, BitConverter.ToInt32(message, 0)));
                data.Add(new Data(Data.INT, Data.IT_TimeMilliseconds, BitConverter.ToInt32(message, 4)));

                while (index < message.Length)
                {
                    byte type = message[index];
                    index += 1;
                    ushort infoType = BitConverter.ToUInt16(message, index);
                    index += 2;

                    switch (type)
                    {
                        case Data.INT:
                            int dataInt = BitConverter.ToInt32(message, index);
                            data.Add(new Data(type, infoType, dataInt));
                            index += 4;
                            break;
                        case Data.UINT:
                            uint dataUInt = BitConverter.ToUInt32(message, index);
                            data.Add(new Data(type, infoType, dataUInt));
                            index += 4;
                            break;
                        case Data.SHORT:
                            short dataShort = BitConverter.ToInt16(message, index);
                            data.Add(new Data(type, infoType, dataShort));
                            index += 2;
                            break;
                        case Data.USHORT:
                            ushort dataUShort = BitConverter.ToUInt16(message, index);
                            data.Add(new Data(type, infoType, dataUShort));
                            index += 2;
                            break;
                        case Data.FLOAT:
                            float dataFloat = BitConverter.ToSingle(message, index);
                            data.Add(new Data(type, infoType, dataFloat));
                            index += 4;
                            break;
                        case Data.DOUBLE:
                            double dataDouble = BitConverter.ToDouble(message, index);
                            data.Add(new Data(type, infoType, dataDouble));
                            index += 8;
                            break;
                        case Data.LONG:
                            long dataLong = BitConverter.ToInt64(message, index);
                            data.Add(new Data(type, infoType, dataLong));
                            index += 8;
                            break;
                        case Data.ULONG:
                            ulong dataULong = BitConverter.ToUInt64(message, index);
                            data.Add(new Data(type, infoType, dataULong));
                            index += 8;
                            break;
                        case Data.BOOL:
                            bool dataBool = BitConverter.ToBoolean(message, index);
                            data.Add(new Data(type, infoType, dataBool));
                            index += 1;
                            break;
                        case Data.BYTE:
                            byte dataByte = message[index];
                            data.Add(new Data(type, infoType, dataByte));
                            index += 1;
                            break;
                        case Data.SBYTE:
                            sbyte dataSByte = (sbyte) message[index];
                            data.Add(new Data(type, infoType, dataSByte));
                            index += 1;
                            break;
                        case Data.CHAR:
                            char dataChar = BitConverter.ToChar(message, index);
                            data.Add(new Data(type, infoType, dataChar));
                            index += 1;
                            break;
                        case Data.STRING:
                            int len = BitConverter.ToInt32(message, index);
                            string dataString = Encoding.Default.GetString(message, index, len);
                            index += len;
                            data.Add(new Data(Data.STRING, infoType, dataString));
                            break;
                        default:
                            //Throw unknown data exception
                            break;
                    }

                }
            }
            catch(Exception e) { }

            return data.ToArray();
        }
    }
}
