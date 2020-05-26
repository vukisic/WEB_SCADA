using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class Singleton
    {
        public Main main;
        private static Singleton inst;
        private Singleton()
        {
            
        }

        public static Singleton GetSingleton()
        {
            if(inst == null)
            {
                inst = new Singleton();
                inst.main = new Main();
            }
            return inst;
        }
    }
}
