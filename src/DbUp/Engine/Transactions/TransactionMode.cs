using System;

namespace DbUp.Engine.Transactions
{
    /// <summary>
    /// The transaction strategy to use
    /// </summary>
    public enum TransactionMode
    {
        /// <summary>
        /// Run creates a new connection for each script, without a transaction
        /// </summary>
        NoTransaction,

        /// <summary>
        /// DbUp will run using a single transaction for the whole upgrade operation
        /// </summary>
        SingleTransaction,
        
        /// <summary>
        /// DbUp will create a new connection and transaction per script
        /// </summary>
        TransactionPerScript,

        /// <summary>
        /// DbUp will use a convention based strategy to determine whether the script will run in a transaction per script
        /// </summary>
        TransactionByConvention
    }
}