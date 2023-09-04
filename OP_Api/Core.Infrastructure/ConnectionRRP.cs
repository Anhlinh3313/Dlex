using System;
namespace Core.Infrastructure
{
	public sealed class ConnectionRRP
    {
		private static ConnectionRRP mInstance = null;
		private string mConnectionString;
		private static readonly object mPadlock = new object();

		private ConnectionRRP(string connectionString)
        {
			mConnectionString = connectionString;
        }   

		public static ConnectionRRP Instance
        {
            get
            {
				lock (mPadlock)
                {
                    if (mInstance == null)
                    {
						throw new Exception("Connection is not initialization!");
                    }
                    return mInstance;
                }
            }
        }

		public string GetConnectionString()
		{
			return mInstance.mConnectionString;
		}

		public static void Create(string connectionString)
        {
			if (mInstance != null)
            {
                throw new Exception("Object already created");
            }
			mInstance = new ConnectionRRP(connectionString);             
        } 
    }
}
