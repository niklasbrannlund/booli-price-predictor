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
    public static string[] GetNumericalFeatureColumnNames(this IDataView dw) => dw.Schema
                                                                      .Where(feature => feature.Name != "Label" && 
                                                                             feature.Type.ToString() == "R4")
                                                                      .Select(f => f.Name).ToArray();

    public static string[] GetCategoricalFeatureColumnNames(this IDataView dw) => dw.Schema
                                                                  .Where(feature => feature.Name != "Label" &&
                                                                         feature.Type.ToString() == "Text")
                                                                  .Select(f => f.Name).ToArray();
  }
}
