using dCom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Utils
{
    public class UpdateDataEventArgs : EventArgs
    {
        public List<BasePointItem> Points { get; set; }

        public UpdateDataEventArgs(List<BasePointItem> points)
        {
            this.Points = points;
        }
    }
}
