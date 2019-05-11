using Microsoft.Data.DataView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.ML
{
  public static class DataViewExtension
  {
    public static string[] GetFeatureColumnNames(this IDataView dw) => dw.Schema
                                                                      .Where(feature => feature.Name != "Label")
                                                                      .Select(f => f.Name).ToArray();
  }
}
