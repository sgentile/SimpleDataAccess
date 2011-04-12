using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    /// <summary>
    /// Class defining unit of work to encapsulate the connection object
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// the current database connection
        /// </summary>
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class
        /// </summary>
        /// <param name="connectionName">the connection name</param>
        public UnitOfWork(string connectionName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            this.connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Finalizes an instance of the UnitOfWork class
        /// </summary>
        ~UnitOfWork()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the current connection
        /// </summary>
        public IDbConnection Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// creating a IDbCommand from the current connection object
        /// </summary>
        /// <returns>a new command object</returns>
        public IDbCommand CreateCommand()
        {
            return this.connection.CreateCommand();
        }

        /// <summary>
        /// opens the underlying database connection
        /// </summary>
        public virtual void Open()
        {
            this.connection.Open();
        }

        /// <summary>
        /// closes the underlying database connection
        /// </summary>
        public virtual void Close()
        {
            this.connection.Close();
        }

        /// <summary>
        /// Disposes the UnitOfWork and underlying connection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// virtual implementation of IDisposable
        /// </summary>
        /// <param name="disposing">flag to indicate if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ////dispose managed resources here
                if (this.connection != null)
                {
                    this.connection.Dispose();
                }
            }
        }
    }
}