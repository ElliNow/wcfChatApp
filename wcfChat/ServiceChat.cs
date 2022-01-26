using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcfChat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            SendMessage($"{user.Name} Подключился к чату!",0);
            users.Add(user);
            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if(user != null)
            {
                users.Remove(user);
                SendMessage($"{user.Name} Покинул чат!",0);
            }
        }

        public void SendMessage(string message,int id)
        {
            foreach(var i in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    answer += $" : {user.Name} ";
                }
                answer += message;

                i.operationContext.GetCallbackChannel<IServiceChatCallBack>().MessageCallBack(answer);
            }
        }
    }
}
