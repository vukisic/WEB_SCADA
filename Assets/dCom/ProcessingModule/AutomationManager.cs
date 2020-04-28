using Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProcessingModule
{
	/// <summary>
	/// Class containing logic for automated work.
	/// </summary>
	public class AutomationManager : IAutomationManager, IDisposable
	{
		private Thread automationWorker;
		private AutoResetEvent automationTrigger;
		private IStorage storage;
		private IProcessingManager processingManager;
		private int delayBetweenCommands;
		private IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutomationManager"/> class.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="processingManager">The processing manager.</param>
		/// <param name="automationTrigger">The automation trigger.</param>
		/// <param name="configuration">The configuration.</param>
		public AutomationManager(IStorage storage, IProcessingManager processingManager, AutoResetEvent automationTrigger, IConfiguration configuration)
		{
			this.storage = storage;
			this.processingManager = processingManager;
			this.configuration = configuration;
			this.automationTrigger = automationTrigger;
			this.delayBetweenCommands = configuration.DelayBetweenCommands;
		}

		/// <summary>
		/// Initializes and starts the threads.
		/// </summary>
		private void InitializeAndStartThreads()
		{
			InitializeAutomationWorkerThread();
			StartAutomationWorkerThread();
		}

		/// <summary>
		/// Initializes the automation worker thread.
		/// </summary>
		private void InitializeAutomationWorkerThread()
		{
			automationWorker = new Thread(AutomationWorker_DoWork);
			automationWorker.Name = "Aumation Thread";
		}

		/// <summary>
		/// Starts the automation worker thread.
		/// </summary>
		private void StartAutomationWorkerThread()
		{
			automationWorker.Start();
		}


		private void AutomationWorker_DoWork()
		{
            /*
                    0--SP1 - Potrosnja PP1+PP2 - 1000
                    1--SP2 - Potrosnja PP3+PP4 - 1001
                    2--TM1 - TM1 - 2000
                    3--TM2 - TM2 - 2001
                    4--PP1 - Prekidac PP1 - 3000
                    5--PP2 - Prekidac PP2 - 3001
                    6--PP3 - Prekidac PP3 - 3002
                    7--PP4 - Prekidac PP4 - 3003
                    8--DG1 - Dis.Gen za PP1 - 4000
                    9--DG2 - Dis.Gen za PP2 - 4001
                    10-DG3 - Dis.Gen za PP3 - 4002
                    11-DG4 - Dis.Gen za PP4 - 4003
             
             */
			int counter = 0;
			int index = -1;
            double SP1, SP2, TM1, TM2;
            int PP1, PP2, PP3, PP4, DG1, DG2, DG3, DG4;
			List<IPoint> points = new List<IPoint>();
			List<PointIdentifier> pointIdentifiers = new List<PointIdentifier>()
			{
				new PointIdentifier(PointType.ANALOG_OUTPUT,1000),
				new PointIdentifier(PointType.ANALOG_OUTPUT,1001),
				new PointIdentifier(PointType.ANALOG_OUTPUT,2000),
				new PointIdentifier(PointType.ANALOG_OUTPUT,2001),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,3000),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,3001),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,3002),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,3003),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,4000),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,4001),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,4002),
				new PointIdentifier(PointType.DIGITAL_OUTPUT,4003),
			};

            List<double> L1 = new List<double>() { 20, 12, 15, 12, 14, 15, 14, 16, 15, 16, 19, 20 };
            List<double> L2 = new List<double>() { 30, 15, 18, 16, 16, 17, 19, 20, 19, 18, 20, 24 };
            List<double> L3 = new List<double>() { 20, 15, 17, 18, 18, 15, 18, 16, 18, 18, 21, 24 };
            List<double> L4 = new List<double>() { 30, 15, 18, 16, 16, 17, 19, 20, 19, 18, 20, 24 };
            List<double> G1 = new List<double>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            List<double> G2 = new List<double>() { -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2 };
            List<double> G3 = new List<double>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            List<double> G4 = new List<double>() { -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2 };

            while (!disposedValue)
			{
                if( counter % delayBetweenCommands == 0 )
                {
                    points = storage.GetPoints(pointIdentifiers);

                    if (counter % 15 == 0)
                    {
                        index = IncIndex(index);
                        ChangeStates(index, points, pointIdentifiers, L1, L2, L3, L4);
                    }

                    points = storage.GetPoints(pointIdentifiers);

                    SP1 = ((IAnalogPoint)points[0]).EguValue;
                    SP2 = ((IAnalogPoint)points[1]).EguValue;
                    TM1 = ((IAnalogPoint)points[2]).EguValue;
                    TM2 = ((IAnalogPoint)points[3]).EguValue;

                    PP1 = ((IDigitalPoint)points[4]).RawValue;
                    PP2 = ((IDigitalPoint)points[5]).RawValue;
                    PP3 = ((IDigitalPoint)points[6]).RawValue;
                    PP4 = ((IDigitalPoint)points[7]).RawValue;

                    DG1 = ((IDigitalPoint)points[8]).RawValue;
                    DG2 = ((IDigitalPoint)points[9]).RawValue;
                    DG3 = ((IDigitalPoint)points[10]).RawValue;
                    DG4 = ((IDigitalPoint)points[11]).RawValue;

                    if (SP1 > 30)
                    {
                        if (SP1 > 30 && DG1 == 0 && DG2 == 0)
                        {
                           SP1 = HandleDistributedGenerators(points,pointIdentifiers,SP1, G1[index], G2[index], 0, 8, 9);
                        }

                        if(SP1 > 30 && TM1 < 4 && DG1==1 && DG2==1)
                        {
                            Tuple<double, double> tuple = HandleTapChanger(points, pointIdentifiers, SP1,TM1,0, 2);
                            SP1 = tuple.Item1;
                            TM1 = tuple.Item2;
                        }

                        if(SP1>30 && TM1==4 && DG1==1 && DG2 == 1)
                        {
                            SP1 = HandleFiderDisabling(points,pointIdentifiers,SP1,L1[index],L2[index],0,4,5);
                        }
                    }
                    else
                    {
                        if (SP1 < 30)
                        {
                            if (DG1 == 1)
                            {
                                if(SP1 - G1[index] <= 30)
                                {
                                    SP1 -= G1[index];
                                    processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(SP1));
                                    processingManager.ExecuteWriteCommand(points[8].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[8].Address, 0);
                                }
                            }

                            if (DG2 == 1)
                            {
                                if (SP1 - G2[index] <= 30)
                                {
                                    SP1 -= G2[index];
                                    processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(SP1));
                                    processingManager.ExecuteWriteCommand(points[9].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[9].Address, 0);
                                }
                            }

                            if (TM1 != 1 && TM1 <= 4)
                            {
                                if(TM1-1>=1 && SP1 + (0.1 *30) <= 30)
                                {
                                    TM1--;
                                    SP1 += (0.1 * 30);
                                    processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(SP1));
                                    processingManager.ExecuteWriteCommand(points[2].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[2].Address, (int)TM1);
                                }
                            }

                            if (PP1 == 0)
                            {
                                if (SP1 + L1[index] <= 30)
                                {
                                    SP1 += L1[index];
                                    processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(SP1));
                                    processingManager.ExecuteWriteCommand(points[4].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[4].Address, 1);
                                }
                            }

                            if(PP2 == 0)
                            {
                                if (SP1 + L2[index] <= 30)
                                {
                                    SP1 += L2[index];
                                    processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(SP1));
                                    processingManager.ExecuteWriteCommand(points[5].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[5].Address, 1);
                                }
                            }

                           

                        }
                    }


                    if (SP2 > 30)
                    {
                        if (SP2 > 30 && DG3 == 0 && DG4 == 0)
                        {
                            SP2 = HandleDistributedGenerators(points, pointIdentifiers, SP2, G3[index], G4[index], 1, 10, 11);
                        }

                        if (SP2 > 30 && TM2 < 4 && DG3 == 1 && DG4 == 1)
                        {
                            Tuple<double, double> tuple = HandleTapChanger(points, pointIdentifiers, SP2, TM2, 1, 3);
                            SP2 = tuple.Item1;
                            TM2 = tuple.Item2;
                        }

                        if (SP2 > 30 && TM2 == 4 && DG3 == 1 && DG4 == 1)
                        {
                            SP2 = HandleFiderDisabling(points, pointIdentifiers, SP2, L3[index], L4[index], 1, 6, 7);
                        }
                    }
                    else
                    {
                        if (SP2 < 30)
                        {
                            if (DG3 == 1)
                            {
                                if (SP2 - G3[index] <= 30)
                                {
                                    SP2 -= G3[index];
                                    processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(SP2));
                                    processingManager.ExecuteWriteCommand(points[10].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[10].Address, 0);
                                }
                            }

                            if (DG4 == 1)
                            {
                                if (SP2 - G4[index] <= 30)
                                {
                                    SP2 -= G4[index];
                                    processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(SP2));
                                    processingManager.ExecuteWriteCommand(points[11].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[11].Address, 0);
                                }
                            }

                            if (TM2 != 1 && TM2 <= 4)
                            {
                                if (TM2 - 1 >= 1 && SP2 + (0.1 * 30) <= 30)
                                {
                                    TM2--;
                                    SP2 += (0.1 * 30);
                                    processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(SP2));
                                    processingManager.ExecuteWriteCommand(points[3].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[3].Address, (int)TM2);
                                }
                            }

                            if (PP3 == 0)
                            {
                                if (SP2 + L3[index] <= 30)
                                {
                                    SP2 += L3[index];
                                    processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(SP2));
                                    processingManager.ExecuteWriteCommand(points[6].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[6].Address, 1);
                                }
                            }

                            if (PP4 == 0)
                            {
                                if (SP2 + L4[index] <= 30)
                                {
                                    SP2 += L4[index];
                                    processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(SP2));
                                    processingManager.ExecuteWriteCommand(points[7].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[7].Address, 1);
                                }
                            }



                        }
                    }


                }
                ++counter;
                automationTrigger.WaitOne();
			}
		}

        private double HandleFiderDisabling(List<IPoint> points, List<PointIdentifier> pointIdentifiers, double currentPower,double L1Power,double L2Power,int mainIndex,int L1Index,int L2Index)
        {
            double NEW_SP = currentPower;
            if (currentPower > 30)
            {
                if (L1Power > L2Power)
                {
                    NEW_SP = currentPower - L2Power;
                    processingManager.ExecuteWriteCommand(points[mainIndex].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[mainIndex].Address, (int)(NEW_SP));
                    processingManager.ExecuteWriteCommand(points[L2Index].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[L2Index].Address, 0);

                }
                else
                {
                    NEW_SP = currentPower - L1Power;
                    processingManager.ExecuteWriteCommand(points[mainIndex].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[mainIndex].Address, (int)(NEW_SP));
                    processingManager.ExecuteWriteCommand(points[L1Index].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[L1Index].Address, 0);
                }
            }
            return NEW_SP;
        }

        private Tuple<double,double> HandleTapChanger(List<IPoint> points, List<PointIdentifier> pointIdentifiers, double currentPower,double currentTM,int mainIndex,int TMIndex)
        {
            double NEW_SP = currentPower;
            double NEW_TM = currentTM;
            if(currentPower > 30)
            {
                NEW_TM++;
                NEW_SP = currentPower - (0.1 * 30);
                processingManager.ExecuteWriteCommand(points[mainIndex].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[mainIndex].Address, (int)(NEW_SP));
                processingManager.ExecuteWriteCommand(points[TMIndex].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[TMIndex].Address, (int)NEW_TM);
            }
            return new Tuple<double, double>(NEW_SP, NEW_TM);
        }


        private double HandleDistributedGenerators(List<IPoint> points,List<PointIdentifier> pointIdentifiers,double currentPower,double firstDisGen,double secondDisGen,int mainIndex,int DG1Index,int DG2Index)
        {
            double NEW_SP = currentPower;
            if (currentPower > 30)
            {
                NEW_SP = currentPower + firstDisGen + secondDisGen;
                processingManager.ExecuteWriteCommand(points[mainIndex].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[mainIndex].Address, (int)(NEW_SP));
                processingManager.ExecuteWriteCommand(points[DG1Index].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[DG1Index].Address, 1);
                processingManager.ExecuteWriteCommand(points[DG2Index].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[DG2Index].Address, 1);
            }
            return NEW_SP;
        }

        private void ChangeStates(int index,List<IPoint> points,List<PointIdentifier> pointIdentifiers,List<double> L1,List<double> L2, List<double> L3,List<double> L4)
        {
            processingManager.ExecuteWriteCommand(points[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[0].Address, (int)(L1[index] + L2[index]));
            processingManager.ExecuteWriteCommand(points[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[1].Address, (int)(L3[index] + L4[index]));
            processingManager.ExecuteWriteCommand(points[2].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[2].Address, 1);
            processingManager.ExecuteWriteCommand(points[3].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[3].Address, 1);
            processingManager.ExecuteWriteCommand(points[4].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[4].Address, 1);
            processingManager.ExecuteWriteCommand(points[5].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[5].Address, 1);
            processingManager.ExecuteWriteCommand(points[6].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[6].Address, 1);
            processingManager.ExecuteWriteCommand(points[7].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[7].Address, 1);
            processingManager.ExecuteWriteCommand(points[8].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[8].Address, 0);
            processingManager.ExecuteWriteCommand(points[9].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[9].Address, 0);
            processingManager.ExecuteWriteCommand(points[10].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[10].Address, 0);
            processingManager.ExecuteWriteCommand(points[11].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, pointIdentifiers[11].Address, 0);
        }
        private int IncIndex(int index)
        {
            if (index == 11)
                index = 0;
            else
                index++;
            return index;
        }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls


		/// <summary>
		/// Disposes the object.
		/// </summary>
		/// <param name="disposing">Indication if managed objects should be disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				}
				disposedValue = true;
			}
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// GC.SuppressFinalize(this);
		}

		/// <inheritdoc />
		public void Start(int delayBetweenCommands)
		{
			InitializeAndStartThreads();
		}

		/// <inheritdoc />
		public void Stop()
		{
			Dispose();
		}
		#endregion
	}
}
