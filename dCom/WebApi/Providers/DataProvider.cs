using Common;
using Common.Serializers;
using Common.WebCommunication;
using dCom.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Configuration;

namespace WebApi.Providers
{
	public class DataProvider : IDataProvider
	{
		private IConfiguration configuration;
		private IJsonSerializer<JsonConfigItemModel> serializer;
		public DataProvider()
		{
			configuration = new ConfigReader();
			serializer = new JsonSerializer<JsonConfigItemModel>();
		}
		public List<IConfigItem> GetConfig()
		{
			return configuration.GetConfigurationItems();
		}

		public string GetJsonConfig()
		{
			List<IConfigItem> list = configuration.GetConfigurationItems();
			List<JsonConfigItemModel> models = new List<JsonConfigItemModel>();
			foreach (IConfigItem configItem in list)
			{
				for (ushort i = 0; i < configItem.NumberOfRegisters; i++)
				{
					JsonConfigItemModel model = new JsonConfigItemModel(configItem, (ushort)(configItem.StartAddress + i));
					models.Add(model);
				}
			}
			return serializer.Serialize(models);
		}
	}
}
