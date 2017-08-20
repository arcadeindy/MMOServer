using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Comms
{
    /// <summary>
    /// Data container for a full packet to send over TCP/IP
    /// </summary>
    public class Message
    {
        public IPAddress destination;
        public Data[] data;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Type of message, select from SC_x, CS_x, or SS_x variables in this class</param>
        /// <param name="destination">Destination IP address for the packet</param>
        /// <param name="data">Array of packet data</param>
        public Message(IPAddress destination, Data[] data)
        {
            this.destination = destination;
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

            for(ushort i = 0; i < data.Length; i++)
            {
                Data dat = data[i];
                messageContents.Add(BitConverter.GetBytes(dat.type));

                //Add to messageContents based on primitive type
                switch (dat.type)
                {
                    case Data.INT:
                        messageContents.Add(BitConverter.GetBytes(dat.data_int));
                        break;
                    case Data.UINT:
                        messageContents.Add(BitConverter.GetBytes(dat.data_uint));
                        break;
                    case Data.SHORT:
                        messageContents.Add(BitConverter.GetBytes(dat.data_short));
                        break;
                    case Data.USHORT:
                        messageContents.Add(BitConverter.GetBytes(dat.data_ushort));
                        break;
                    case Data.FLOAT:
                        messageContents.Add(BitConverter.GetBytes(dat.data_float));
                        break;
                    case Data.DOUBLE:
                        messageContents.Add(BitConverter.GetBytes(dat.data_double));
                        break;
                    case Data.LONG:
                        messageContents.Add(BitConverter.GetBytes(dat.data_long));
                        break;
                    case Data.ULONG:
                        messageContents.Add(BitConverter.GetBytes(dat.data_ulong));
                        break;
                    case Data.BOOL:
                        messageContents.Add(BitConverter.GetBytes(dat.data_bool));
                        break;
                    case Data.BYTE:
                        messageContents.Add(BitConverter.GetBytes(dat.data_byte));
                        break;
                    case Data.SBYTE:
                        messageContents.Add(BitConverter.GetBytes(dat.data_sbyte));
                        break;
                    case Data.CHAR:
                        messageContents.Add(BitConverter.GetBytes(dat.data_char));
                        break;
                    case Data.STRING:
                        messageContents.Add(Encoding.ASCII.GetBytes(dat.data_string));
                        break;
                    default:
                        break;
                }
            }

            return messageContents.SelectMany(a => a).ToArray();
        }
    }
}
