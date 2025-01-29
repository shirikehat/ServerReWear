using System.ComponentModel.DataAnnotations;

namespace ServerReWear.DTO
{
    public class Status
    {
        public int StatusCode { get; set; }

        public string? Name { get; set; }
        public Status(Models.Status status)
        {
            this.StatusCode = status.StatusCode;
            this.Name = status.Name;
        }
    }

    public class PrType
    {
        public int TypeCode { get; set; }

        public string? Name { get; set; }
        public PrType(Models.Type t)
        {
            this.TypeCode = t.TypeCode;
            this.Name = t.Name;
        }

    }
    public class BasicData
    {
        public BasicData() { }

        public BasicData(List<Models.Status> statuses, List<Models.Type> types)
        {
            this.Statuses = new List<Status>();
            foreach(Models.Status s in statuses)
                this.Statuses.Add(new Status(s));

            this.PrTypes = new List<PrType>();
            foreach (Models.Type s in types)
                this.PrTypes.Add(new PrType(s));

        }
        public List<Status> Statuses { get; set; }
        public List<PrType> PrTypes { get; set; }
    }
}
