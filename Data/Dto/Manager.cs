using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data.Dto
{
    internal class Manager
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public String Name { get; set; } = null!;
        public DateTime WorksFrom { get; set; }
        public override string ToString() => $"{Id} - {Name} ({WorksFrom:yyyy-MM-dd}) - Dept: {DepartmentId}";

    }
}