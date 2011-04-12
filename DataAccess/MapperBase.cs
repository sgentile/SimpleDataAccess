using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace DataAccess
{
    /// <summary>
    /// Class defining a mapping from an IDataReader to entity 
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public abstract class MapperBase<T> where T : new()
    {
        /// <summary>
        /// this will use reflection, but if your objects aren't convention, or need special mapping, you can override
        /// </summary>
        /// <param name="record">an instance of IDataRecord</param>
        /// <returns>the current entity</returns>
        public virtual T Map(IDataRecord record)
        {
            var instance = new T();

            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i < record.FieldCount; i++)
            {
                string fieldName = record.GetName(i);

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == fieldName)
                    {
                        property.SetValue(instance, record[i], null);
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Maps each entity returned using Map to return a collection of entities
        /// </summary>
        /// <param name="reader">an instance of an IDataReader</param>
        /// <returns>the collection of mapped entities</returns>
        public Collection<T> MapAll(IDataReader reader)
        {
            Collection<T> collection = new Collection<T>();

            while (reader.Read())
            {
                try
                {
                    collection.Add(this.Map(reader));
                }
                catch
                {
                    throw;

                    // NOTE:
                    // consider handling exeption here instead of re-throwing
                    // if graceful recovery can be accomplished
                }
            }

            return collection;
        }
    }
}