using Sharp_231.Attributes;
using System;

namespace Sharp_231.Data.Dto
{
    [TableName("Accesses")]
    public class Access
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public string Login { get; set; }
        public string Salt { get; set; }
        public string Dk { get; set; }
    }
}