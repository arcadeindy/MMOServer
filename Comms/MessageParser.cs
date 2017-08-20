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
                    ushort infoType = BitConverter.ToUInt16(message, index);
                    index += 2;

                    switch (infoType)
                    {
                        case Data.IT_PlayerPositionX:
                            float x = BitConverter.ToSingle(message, index);
                            index += 4;
                            data.Add(new Data(Data.FLOAT, Data.IT_PlayerPositionX, x));
                            break;
                        case Data.IT_PlayerPositionY:
                            float y = BitConverter.ToSingle(message, index);
                            index += 4;
                            data.Add(new Data(Data.FLOAT, Data.IT_PlayerPositionY, y));
                            break;
                        case Data.IT_PlayerPositionZ:
                            float z = BitConverter.ToSingle(message, index);
                            index += 4;
                            data.Add(new Data(Data.FLOAT, Data.IT_PlayerPositionZ, z));
                            break;
                        case Data.IT_PlayerID:
                            ushort id = (ushort) BitConverter.ToInt16(message, index);
                            index += 4;
                            break;
                        default:
                            //Throw unknown data exception
                            break;
                    }

                }
            }
            catch(Exception e) { }
        }
    }
}
