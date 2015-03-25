using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner.EventsSystem
{
    public class EventSystem : IEventSystem
    {
        private ISubscriber redisSubscriber;
        private readonly BinarySerializer binarySerializer;
       
        //CR: are this regions helpful?
        //CR: usually regions are used when the class is not clear and when it has to many responsibilities
        //CR: here is not case

        #region Constructor


        //CR: comments not useful. They add no value to this code.

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSystem"/> class.
        /// </summary>
        public EventSystem() : this(new BinarySerializer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSystem"/> class.
        /// </summary>
        /// <param name="binarySerializer">The serializer.</param>
        public EventSystem(BinarySerializer binarySerializer)
        {
            this.binarySerializer = binarySerializer;

            redisSubscriber = RedisConnection.Connection.GetSubscriber();
        }

        #endregion

        #region IEventSystem Implementation

        /// <see cref="IEventSystem"/>
        public Task SubscribeAsync<T>(string channel, ChannelPattern channelPattern, Action<string, T> handlerAction)
        {
            LogExtensions.Log(string.Format("channel: {0}, handlerAction: {1}, channelPattern: {2} ", channel, handlerAction, channelPattern));

            var redisChannel = GetRedisChannel(channel, channelPattern);

            //CR: I'm lost down there ;)
            return EnsureConnectionOpen(() =>
                redisSubscriber.SubscribeAsync(redisChannel,
                (responseChannel, responseValue) =>
                {
                    try
                    {
                        handlerAction(responseChannel.ToString(), binarySerializer.Deserialize<T>(responseValue));
                    }
                    catch (Exception ex)
                    {
                        LogExtensions.Log(string.Format("Error trying to call subscriber: {0}", ex));
                    }
                }));
        }             

        /// <see cref="IEventSystem"/>
        public Task UnsubscribeAsync(string channel, ChannelPattern channelPattern)
        {
            LogExtensions.Log(string.Format("channel: {0}, channelPattern: {1}", channel, channelPattern));

            var redisChannel = GetRedisChannel(channel, channelPattern);
            return EnsureConnectionOpen(() => redisSubscriber.UnsubscribeAsync(redisChannel));
        }

        /// <see cref="IEventSystem"/>
        public Task UnsubscribeAllAsync()
        {
            return EnsureConnectionOpen(() => redisSubscriber.UnsubscribeAllAsync());
        }

        /// <see cref="IEventSystem"/>
        public Task<long> PublishAsync<T>(string channel, ChannelPattern channelPattern, T message)
        {
            LogExtensions.Log(string.Format("channel: {0}, channelPattern: {1}, message: {2}", channel, channelPattern, message));

            var redisChannel = GetRedisChannel(channel, channelPattern);

            return EnsureConnectionOpen(() => redisSubscriber.PublishAsync(redisChannel, binarySerializer.Serialize(message)));
        }

        #endregion

        #region Private Methods

        private RedisChannel GetRedisChannel(string channel, ChannelPattern channelPattern)
        {
            var redisChannelPattern = (RedisChannel.PatternMode)Enum.Parse(typeof(RedisChannel.PatternMode), channelPattern.ToString());

            return new RedisChannel(channel, redisChannelPattern);
        }

        /// <summary>
        /// Ensures the cache connection is open.
        /// </summary>
        /// <param name="action">The action.</param>
        private void EnsureConnectionOpen(Action action)
        {
            try
            {
                if (TryInitializeConnection())
                {
                    action();
                }
            }         
            catch (Exception exception)
            {
                LogExtensions.Log(string.Format("Exception: {0} {1}", exception.Message, exception.StackTrace));
                throw;
            }
        }


        //CR: this is not used

        /// <summary>
        /// Ensures the cache connection is open.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        private T EnsureConnectionOpen<T>(Func<T> func)
        {
            try
            {
                if (TryInitializeConnection())
                {
                    return func();
                }
            }          
            catch (Exception exception)
            {
                LogExtensions.Log(string.Format("Exception: {0} {1}", exception.Message, exception.StackTrace));
                throw;
            }

            throw new NotImplementedException();
        }

        private bool TryInitializeConnection()
        {
            if (!RedisConnection.IsOpen)
            {
                return false;
            }

            InitializationAction();

            return true;
        }

        private Action InitializationAction()
        {
            return new Action(() => { });
        }

        #endregion
    }
}
