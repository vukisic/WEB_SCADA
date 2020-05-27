using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class Singleton
    {
        private static Main main;
        private static Singleton inst;
        private Singleton()
        {
            
        }

        public static Main GetSingleton()
        {
            if(main == null)
            {
                main = new Main();
            }
            return main;
        }
    }
}
