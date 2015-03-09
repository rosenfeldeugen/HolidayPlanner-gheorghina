using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner.EventsSystem
{
    public interface IEventSystem
    {
        /// <summary>
        /// Subscribes to a channel in an async fashion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel">The channel.</param>
        /// <param name="channelPattern">The channel pattern.</param>
        /// <param name="handlerAction">The handler action.</param>
        /// <returns></returns>
        Task SubscribeAsync<T>(string channel, ChannelPattern channelPattern, Action<string, T> handlerAction);

        /// <summary>
        /// Unsubscribes from the given channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="channelPattern">The channel pattern.</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string channel, ChannelPattern channelPattern);

        /// <summary>
        /// Unsubscribes all asynchronous.
        /// </summary>
        /// <returns></returns>
        Task UnsubscribeAllAsync();

        /// <summary>
        /// Publishes async a given message on a channel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel">The channel.</param>
        /// <param name="channelPattern">The channel pattern.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task<long> PublishAsync<T>(string channel, ChannelPattern channelPattern, T message);
    }
}
