using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayPlanner.EventsSystem.Properties;

namespace HolidayPlanner.EventsSystem
{
    internal class RedisConnection
    {
        private static readonly ConfigurationOptions configurationOptions = new ConfigurationOptions
        {
            EndPoints = { Settings.Default.MasterConnection},
            AllowAdmin = true,
            Password = Settings.Default.CachePassword
        };

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public static bool IsOpen
        {
            get
            {
                return lazyConnection != null && lazyConnection.Value.IsConnected;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public static ConnectionMultiplexer Connection
        {
            get { return lazyConnection.Value; }
        }

        /// <summary>
        /// Closes the specified allow commands to complete.
        /// </summary>
        /// <param name="allowCommandsToComplete">if set to <c>true</c> [allow commands to complete].</param>
        public static void Close(bool allowCommandsToComplete = true)
        {
            if (IsOpen)
            {
                lazyConnection.Value.Close(allowCommandsToComplete);
            }
        }
    }
}
