using System.ServiceModel;

namespace wcfChat
{
    class ServerUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OperationContext operationContext { get; set; }       
    }
}
