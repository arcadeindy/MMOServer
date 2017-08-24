using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Comms
{
    /// <summary>
    /// Data container for a full packet to send over TCP/IP
    /// </summary>
    public class Message
    {
        public Data[] data;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="data">Array of packet data</param>
        public Message(Data[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Packs this Message instance into a sendable byte[] for a TCP/IP packet
        /// </summary>
        /// <returns>byte[] to be used in a TCP/IP packet</returns>
        public byte[] GetSendableMessage()
        {
            byte[] s = BitConverter.GetBytes(DateTime.Now.Second);
            byte[] ms = BitConverter.GetBytes(DateTime.Now.Millisecond);
            List<byte[]> messageContents = new List<byte[]>();
            messageContents.Add(s);
            messageContents.Add(ms);

            for(int i = 0; i < data.Length; i++)
            {
                Data dat = data[i];
                messageContents.Add(new byte[] { dat.type });

                //Add to messageContents based on primitive type
                switch (dat.type)
                {
                    case Data.INT:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_int));
                        break;
                    case Data.UINT:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_uint));
                        break;
                    case Data.SHORT:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_short));
                        break;
                    case Data.USHORT:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_ushort));
                        break;
                    case Data.FLOAT:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_float));
                        break;
                    case Data.DOUBLE:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_double));
                        break;
                    case Data.LONG:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_long));
                        break;
                    case Data.ULONG:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_ulong));
                        break;
                    case Data.BOOL:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_bool));
                        break;
                    case Data.BYTE:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_byte));
                        break;
                    case Data.SBYTE:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_sbyte));
                        break;
                    case Data.CHAR:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_char));
                        break;
                    case Data.STRING:
                        messageContents.Add(BitConverter.GetBytes(dat.infoType));
                        messageContents.Add(BitConverter.GetBytes(dat.data_string.Length));
                        messageContents.Add(Encoding.ASCII.GetBytes(dat.data_string));
                        break;
                    default:
                        break;
                }
            }

            /*IEnumerable<byte> result = Enumerable.Empty<byte>();

            foreach(byte[] bytes in messageContents)
            {
                result = result.Concat(bytes);
            }

            return result.ToArray();*/
            return messageContents.SelectMany(a => a).ToArray();
        }
    }
}
