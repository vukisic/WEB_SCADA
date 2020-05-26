using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.PoinViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		internal void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
