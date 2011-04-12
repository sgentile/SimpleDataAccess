using System;
using System.Data;

namespace DataAccess
{
    /// <summary>
    /// Interface defining the IUnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the current database connection object
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Gets creates a IDbCommand from the current connection object
        /// </summary>
        /// <returns>a new instance of the command object</returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// Gets opens the current database connection
        /// </summary>
        void Open();

        /// <summary>
        /// Gets closes the current database connection
        /// </summary>
        void Close();
    }
}