using System;
using System.Collections.Generic;

namespace WebApi.Configuration
{
	/// <summary>
	/// Implementation of IEqualityComparer for ConfigItem
	/// </summary>
	internal class ConfigItemEqualityComparer : IEqualityComparer<ConfigItem>
	{
		/// <summary>
		/// Checks if two config items are equeal
		/// </summary>
		/// <param name="x">First ConfigItem</param>
		/// <param name="y">Second ConfigItem</param>
		/// <returns></returns>
		public bool Equals(ConfigItem x, ConfigItem y)
		{
			if (string.Compare(x.Description, y.Description) == 0)
			{
				throw new ArgumentException("Configuration item description must be unique!");
			}
			if (x.StartAddress == y.StartAddress)
			{
				throw new ArgumentException("Configuration item start address must be unique!");
			}
			if (x.StartAddress != y.StartAddress)
			{
				ConfigItem lessAddress = x.StartAddress < y.StartAddress ? x : y;
				ConfigItem greaterAddress = x.StartAddress > y.StartAddress ? x : y;
				if ((ushort)(lessAddress.StartAddress + lessAddress.NumberOfRegisters) > greaterAddress.StartAddress)
				{
					throw new ArgumentException($"Address ranges are overlapping for point types of {x.RegistryType}({x.Description}) and {y.RegistryType}({y.Description})");
				}
			}
			return false;
		}

		/// <summary>
		/// Return hash code for ConfigItem
		/// </summary>
		/// <param name="obj">ConfigItem</param>
		/// <returns></returns>
		public int GetHashCode(ConfigItem obj)
		{
			return base.GetHashCode();
		}
	}
}