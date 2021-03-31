using LiteDB;
using Microsoft.ML.Data;

namespace Booli.ML
{
  public class ListingPrediction
  {
    [ColumnName("Score")]
    public float SoldPrice { get; set; }
  }
}
