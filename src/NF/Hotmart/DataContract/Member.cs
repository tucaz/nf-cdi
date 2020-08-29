using System;
using System.Collections.Generic;

namespace NF.Hotmart.DataContract
{
    public class Member : IEquatable<Member>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ucode { get; set; }
        public string DDDPhone { get; set; }
        public string Phone { get; set; }
        public MemberAddress Address { get; set; }
        public string Locale { get; set; }
        public List<Document> Documents { get; set; }
        public bool IsSubscription { get; set; }

        private static string GetCountryCode(string country)
        {
            switch (country)
            {
                case "United States":
                case "Canada":
                    return "+1";
                case "South Africa":
                    return "+27";
                case "Argentina":
                    return "+54";
                case "Brasil":
                    return "+55";
                case "Colombia":
                    return "+57";
                case "Australia":
                    return "+61";
                case "Panama":
                    return "+507";
                case "Portugal":
                    return "+351";
                case "Ireland":
                    return "+353";
                case "Japan":
                    return "+81";
                case "Netherlands":
                    return "+31";

                default:
                    throw new ArgumentOutOfRangeException($"Invalid country: {country}", nameof(country));
            }
        }

        public string PhoneNumber
        {
            get
            {
                return $"{GetCountryCode(Address.Country)}{DDDPhone}{Phone}";
            }
        }

        public bool Equals(Member other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Member) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}