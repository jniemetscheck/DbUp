using DbUp.Engine.Output;
using System;
using System.Collections.Generic;
using System.Data;

namespace DbUp.Engine.Transactions
{
    class TransactionByConventionStrategry : ITransactionStrategy
    {
        private IDbConnection connection;
        private readonly string _excludeScriptsFromTransactionThatContainPhrase;

        public TransactionByConventionStrategry(string excludeScriptsFromTransactionThatContainPhrase)
        {
            _excludeScriptsFromTransactionThatContainPhrase = excludeScriptsFromTransactionThatContainPhrase;
        }

        public TransactionByConventionStrategry()
        {
            _excludeScriptsFromTransactionThatContainPhrase = null;
        }

        public void Execute(Action<Func<IDbCommand>> action)
        {
            var command = connection.CreateCommand();

            if (!string.IsNullOrEmpty(_excludeScriptsFromTransactionThatContainPhrase) && command.CommandText.Contains(_excludeScriptsFromTransactionThatContainPhrase))
            {
                action(() => connection.CreateCommand());
            }
            else
            {
                using (var transaction = connection.BeginTransaction())
                {
                    action(() =>
                    {
                        command.Transaction = transaction;
                        return command;
                    });
                    transaction.Commit();
                }
            }
        }

        public T Execute<T>(Func<Func<IDbCommand>, T> actionWithResult)
        {
            var command = connection.CreateCommand();

            if (command.CommandText.Contains(_excludeScriptsFromTransactionThatContainPhrase))
            {
                return actionWithResult(() => connection.CreateCommand());
            }
            else
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = actionWithResult(() =>
                    {
                        command.Transaction = transaction;
                        return command;
                    });
                    transaction.Commit();
                    return result;
                }
            }
        }

        public void Initialise(IDbConnection dbConnection, IUpgradeLog upgradeLog, List<SqlScript> executedScripts)
        {
            connection = dbConnection;
        }

        public void Dispose() { }
    }
}
