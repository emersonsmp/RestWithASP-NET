using RestWithASPNET.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestWithASPNET.Data.VO
{
    public class PersonVO
    {
        [JsonPropertyName("Code")]
        public long Id { get; set; }
        public string Name { get; set; }


        [JsonPropertyName("Last_Name")]
        public string LastName { get; set; }

        [JsonIgnore]
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
