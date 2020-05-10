using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serializers
{
	public class JsonConfigItemModel
	{
		public JsonConfigItemModel(IConfigItem item, ushort address)
		{
			Address = address;
			RegistryType = item.RegistryType;
			DecimalSeparatorPlace = item.DecimalSeparatorPlace;
			MinValue = item.MinValue;
			MaxValue = item.MaxValue;
			DefaultValue = item.DefaultValue;
			ProcessingType = item.ProcessingType;
			Description = item.Description;
			AcquisitionInterval = item.AcquisitionInterval;
			ScaleFactor = item.ScaleFactor;
			Deviation = item.Deviation;
			EGU_Max = item.EGU_Max;
			EGU_Min = item.EGU_Min;
			AbnormalValue = item.AbnormalValue;
			HighLimit = item.HighLimit;
			LowLimit = item.LowLimit;
			SecondsPassedSinceLastPoll = item.SecondsPassedSinceLastPoll;
		}
		// <summary>
		/// Gets/Sets a value indicating the registry type.
		/// </summary>
		public PointType RegistryType { get; set; }

		/// <summary>
		/// Gets/Sets the start address.
		/// </summary>
		public ushort Address { get; set; }

		/// <summary>
		/// Gets/Sets the decimal separator`s place.
		/// </summary>
		public ushort DecimalSeparatorPlace { get; }

		/// <summary>
		/// Gets/Sets the minimal value.
		/// </summary>
		public ushort MinValue { get; set; }

		/// <summary>
		/// Gets/Sets the maximal value.
		/// </summary>
		public ushort MaxValue { get; set; }

		/// <summary>
		/// Gets/Sets the default value.
		/// </summary>
		public ushort DefaultValue { get; set; }

		/// <summary>
		/// Gets/Sets the processing type.
		/// </summary>
		public string ProcessingType { get; set; }

		/// <summary>
		/// Gets/Sets the description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets/Sets the acquisition interval.
		/// </summary>
		public int AcquisitionInterval { get; set; }

		/// <summary>
		/// Gets/Sets the scale factor.
		/// </summary>
		public double ScaleFactor { get; set; }

		/// <summary>
		/// Gets/Sets the deviation.
		/// </summary>
		public double Deviation { get; set; }

		/// <summary>
		/// Gets/Sets the minimal value in engineering units.
		/// </summary>
		public double EGU_Min { get; set; }

		/// <summary>
		/// Gets/Sets the maximal value in engineering units.
		/// </summary>
		public double EGU_Max { get; set; }

		/// <summary>
		/// Gets/Sets the abnormal value.
		/// </summary>
		public ushort AbnormalValue { get; set; }

		/// <summary>
		/// Gets/Sets high alarm limit.
		/// </summary>
		public double HighLimit { get; set; }

		/// <summary>
		/// Gets/Sets the low alarm limit.
		/// </summary>
		public double LowLimit { get; set; }

		/// <summary>
		/// Gets/Sets or sets the time passed since last poll was issued.
		/// </summary>
		public int SecondsPassedSinceLastPoll { get; set; }
	}
}
