using CPP_Lab13API.Models;

namespace CPP_Lab13API.Services
{
    public class MessageService : IMessageService
    {
        public Message GetProtectedMessage()
        {
            return new Message { text = "This is a protected message." };
        }

        public Message GetPublicMessage()
        {
            return new Message { text = "This is a public message." };
        }
    }
}
