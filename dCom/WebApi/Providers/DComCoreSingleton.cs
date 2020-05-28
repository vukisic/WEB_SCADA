namespace WebApi.Providers
{
    public class DComCoreSingleton
    {
        private static DComCore main;
        private static object lockObj = new object();
        private DComCoreSingleton() { }

        public static DComCore GetSingleton()
        {
            if(main == null)
            {
                if(main == null)
                {
                    lock (lockObj)
                    {
                        main = new DComCore();
                    }
                }
            }
            return main;
        }
    }
}
