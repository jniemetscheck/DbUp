using System;
using System.Collections.Generic;
using DbUp.Engine;

namespace DbUp.Builder
{
    /// <summary>
    /// Builds a UpgradeEngine by accepting a list of callbacks to execute. For custom configuration, you should 
    /// implement extension methods on top of this class.
    /// </summary>
    public class UpgradeEngineBuilder
    {
        private readonly List<Action<UpgradeConfiguration>> callbacks = new List<Action<UpgradeConfiguration>>();
        /// <summary>
        /// If this phrase is found the script will NOT be used in a transaction
        /// </summary>
        public string ExcludeScriptsFromTransactionThatContainPhrase;

        /// <summary>
        /// Adds a callback that will be run to configure the upgrader when Build is called.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(Action<UpgradeConfiguration> configuration)
        {
            Configure(configuration, null);
        }

        /// <summary>
        /// Adds a callback that will be run to configure the upgrader when Build is called.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="excludeScriptsFromTransactionThatContainPhrase">If this phrase is found the script will NOT be used in a transaction</param>
        public void Configure(Action<UpgradeConfiguration> configuration, string excludeScriptsFromTransactionThatContainPhrase)
        {
            callbacks.Add(configuration);
            ExcludeScriptsFromTransactionThatContainPhrase = excludeScriptsFromTransactionThatContainPhrase;
        }

        /// <summary>
        /// Creates an UpgradeEngine based on this configuration.
        /// </summary>
        /// <returns></returns>
        public UpgradeEngine Build()
        {
            var config = new UpgradeConfiguration();
            foreach (var callback in callbacks)
            {
                callback(config);
            }

            config.Validate();
            
            return new UpgradeEngine(config);
        }
    }
}