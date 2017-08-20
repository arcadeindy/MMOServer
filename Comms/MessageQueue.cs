using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comms
{
    /// <summary>
    /// Wrapper for a Queue for Message objects in order to provide singleton functionality and limit access
    /// </summary>
    public class MessageQueue
    {
        public static MessageQueue singleton;
        Queue<Message> queue;

        public MessageQueue()
        {
            queue = new Queue<Message>();
            singleton = this;
        }

        public void AddMessage(Message message)
        {
            queue.Enqueue(message);
        }

        public Message GetNext()
        {
            return queue.Dequeue();
        }

        public void Flush()
        {
            queue.Clear();
        }
    }
}
