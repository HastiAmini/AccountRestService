using System.Runtime.Serialization;

namespace AccountSearch.Models
{
    [DataContract]
    public class Account
    {
        public bool MaskAccountNumber { get; set; }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Status { get; private set; }

        private string _number;

        [DataMember]
        public string Number
        {
            get
            {
                if (MaskAccountNumber)
                {
                    if (_number.Length > 4)
                    {
                        return _number.Substring(_number.Length - 4);
                    }
                }

                return _number;
            }
            private set
            {
                _number = value;                
            }
        }

        public Account(int id, string number, string type, string name, string status)
        {
            MaskAccountNumber = false;

            Id = id;
            Number = number;
            Type = type;
            Name = name;
            Status = status;
        }
    }
}
