
namespace GraphDemo.Models
{
    public class Airport
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string PartitionKey { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public int Runways { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
    }
}
