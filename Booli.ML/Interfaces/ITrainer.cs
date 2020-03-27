using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.ML.Interfaces
{
  public interface ITrainer
  {
    void TrainModel();
    void SaveModel(string modelPath);
  }
}
