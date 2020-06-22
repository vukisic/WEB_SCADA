namespace WebApi.Providers
{
    /// <summary>
    /// Singleton pattern for DComCore
    /// </summary>
    public class DComCoreSingleton
    {
        #region Fields
        private static DComCore main;
        private static object lockObj = new object();
        #endregion
        private DComCoreSingleton() { }

        /// <summary>
        /// Method to get singleton instance
        /// </summary>
        /// <returns>Singleton Instance</returns>
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






