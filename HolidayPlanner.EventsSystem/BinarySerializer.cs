using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HolidayPlanner.EventsSystem
{
    public class BinarySerializer
    {
        //CR: The comments here are not useful


        /// <summary>
        /// Serializes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public byte[] Serialize<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, entity);
                byte[] objectDataAsStream = memoryStream.ToArray();
                
                return objectDataAsStream;
            }
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            try
            {
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream(stream))
                {
                    var result = (T)binaryFormatter.Deserialize(memoryStream);

                    return result;
                }
            }
            catch(Exception ex)
            {
                LogExtensions.Log(string.Format("Deserialization failure.", ex));
                throw;
            }
        }
    }
}
