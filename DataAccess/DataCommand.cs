using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    /// <summary>
    /// Class defining a database command
    /// </summary>
    /// <typeparam name="T">the entity to be returned from command</typeparam>
    public abstract class DataCommand<T> where T : new()
    {
        /// <summary>
        /// the current database reader
        /// </summary>
        private IDataReader reader;

        /// <summary>
        /// the current database command object
        /// </summary>
        private IDbCommand command;

        /// <summary>
        /// a collection of parameters used by the command object
        /// </summary>
        private Collection<IDataParameter> parameterCollection;

        /// <summary>
        /// Gets  the current mapper to be used for the command execute
        /// </summary>
        public abstract MapperBase<T> Mapper { get; }

        /// <summary>
        /// Gets the current command text
        /// </summary>
        public abstract string CommandText { get; }

        /// <summary>
        /// Gets the current command type
        /// </summary>
        public abstract CommandType CommandType { get; }

        /// <summary>
        /// Gets a collection of parameters used by the command object
        /// </summary>
        public Collection<IDataParameter> ParameterCollection
        {
            get { return this.parameterCollection ?? (this.parameterCollection = new Collection<IDataParameter>()); }
        }

        /// <summary>
        /// Executes the query and performs the mapping
        /// </summary>
        /// <param name="unitOfWork">the unit of work handles the connection</param>
        /// <returns>a collection of entities</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "per design")]
        public Collection<T> Execute(IUnitOfWork unitOfWork)
        {
            Collection<T> collection = new Collection<T>();

            this.command = unitOfWork.CreateCommand();
            this.command.Connection = unitOfWork.Connection;
            this.command.CommandText = this.CommandText;
            this.command.CommandType = this.CommandType;

            if (this.ParameterCollection != null)
            {
                foreach (IDataParameter param in this.ParameterCollection)
                {
                    this.command.Parameters.Add(param);
                }
            }

            try
            {
                using (this.reader = this.ExecuteReader())
                {
                    try
                    {
                        MapperBase<T> mapperBase = this.Mapper;
                        collection = mapperBase.MapAll(this.reader);
                        return collection;
                    }
                    catch
                    {
                        throw;

                        // NOTE:
                        // consider handling exeption here
                        // instead of re-throwing
                        // if graceful recovery can be accomplished
                    }
                    finally
                    {
                        this.reader.Close();
                    }
                }
            }
            catch
            {
                throw;

                // NOTE:
                // consider handling exeption here instead of re-throwing
                // if graceful recovery can be accomplished
            }
        }

        /// <summary>
        /// Executes the command object ExecuteReader method
        /// </summary>
        /// <returns>the current datareader</returns>
        public virtual IDataReader ExecuteReader()
        {
            return this.command.ExecuteReader();
        }

        /// <summary>
        /// Add parameters to be used by the command object
        /// </summary>
        /// <param name="parameterName">the name of the parameter</param>
        /// <param name="parameterValue">the value of the parameter</param>
        /// <returns>an instance of the DataCommand</returns>
        public DataCommand<T> AddParameters(string parameterName, object parameterValue)
        {
            this.ParameterCollection.Add(new SqlParameter(parameterName, parameterValue));
            return this;
        }
    }
}