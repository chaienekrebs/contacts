
namespace Contacts.Domain.DTO
{
    public class IpInfo
    {
        public string Ip { get; set; }
        public string ContinentCode { get; set; }
        public string ContinentName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryNameNative { get; set; }
        public string OfficialCountryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public int CityGeoNameId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Capital { get; set; }
        public string PhoneCode { get; set; }
        public string CountryFlagEmoj { get; set; }
        public string CountryFlagEmojUnicode { get; set; }
        public List<string> Borders { get; set; }
    }
}
