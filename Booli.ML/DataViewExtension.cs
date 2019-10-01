using Microsoft.Data.DataView;
using System.Linq;

namespace Booli.ML
{
  public static class DataViewExtension
  {
    public static string[] GetFeatureColumnNames(this IDataView dw) => dw.Schema
                                                                      .Where(feature => feature.Name != "Label")
                                                                      .Select(f => f.Name).ToArray();
  }
}
