using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooliAPI.Models
{
  public class Address
  {
    [JsonProperty("streetAddress")]
    public string StreetAddress { get; set; }
  }
}
