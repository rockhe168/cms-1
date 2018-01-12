﻿using System;
using System.Data;
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using SiteServer.Plugin.Models;

namespace BaiRong.Core.Data
{
    public class MySql : DbHelper, IDbHelper
    {
        #region Overrides
        /// <summary>
        /// Returns an array of SqlParameters of the specified size
        /// </summary>
        /// <param name="size">size of the array</param>
        /// <returns>The array of SqlParameters</returns>
        protected override IDataParameter[] GetDataParameters(int size)
        {
            return new MySqlParameter[size];
        }


        /// <summary>
        /// Returns a SqlConnection object for the given connection string
        /// </summary>
        /// <param name="connectionString">The connection string to be used to create the connection</param>
        /// <returns>A SqlConnection object</returns>
        public override IDbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public IDbCommand GetCommand()
        {
            return new MySqlCommand();
        }


        /// <summary>
        /// Returns a SqlDataAdapter object
        /// </summary>
        /// <returns>The SqlDataAdapter</returns>
        public override IDbDataAdapter GetDataAdapter()
        {
            return new MySqlDataAdapter();
        }


        /// <summary>
        /// Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
        /// </summary>
        /// <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
        public override void DeriveParameters(IDbCommand cmd)
        {
            bool mustCloseConnection = false;


            if (!(cmd is MySqlCommand))
                throw new ArgumentException("The command provided is not a SqlCommand instance.", "cmd");


            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
                mustCloseConnection = true;
            }


            MySqlCommandBuilder.DeriveParameters((MySqlCommand)cmd);


            if (mustCloseConnection)
            {
                cmd.Connection.Close();
            }
        }


        /// <summary>
        /// Returns a SqlParameter object
        /// </summary>
        /// <returns>The SqlParameter object</returns>
        public override IDataParameter GetParameter()
        {
            return new MySqlParameter();
        }


        /// <summary>
        /// Detach the IDataParameters from the command object, so they can be used again.
        /// </summary>
        /// <param name="command">command object to clear</param>
        protected override void ClearCommand(IDbCommand command)
        {
            // HACK: There is a problem here, the output parameter values are fletched 
            // when the reader is closed, so if the parameters are detached from the command
            // then the IDataReader can磘 set its values. 
            // When this happen, the parameters can磘 be used again in other command.
            bool canClear = true;


            foreach (IDataParameter commandParameter in command.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;


            }
            if (canClear)
            {
                command.Parameters.Clear();
            }
        }


        /// <summary>
        /// This cleans up the parameter syntax for an SQL Server call.  This was split out from PrepareCommand so that it could be called independently.
        /// </summary>
        /// <param name="command">An IDbCommand object containing the CommandText to clean.</param>
        public override void CleanParameterSyntax(IDbCommand command)
        {
            // do nothing for SQL
        }


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the provided SqlConnection. 
        /// </summary>
        /// <example>
        /// <code>
        /// XmlReader r = helper.ExecuteXmlReader(command);
        /// </code></example>
        /// <param name="command">The IDbCommand to execute</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        public override XmlReader ExecuteXmlReader(IDbCommand command)
        {
            bool mustCloseConnection = false;


            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
                mustCloseConnection = true;
            }


            CleanParameterSyntax(command);
            MySqlDataAdapter da = new MySqlDataAdapter((MySqlCommand)command);
            DataSet ds = new DataSet();


            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);


            StringReader stream = new StringReader(ds.GetXml());
            if (mustCloseConnection)
            {
                command.Connection.Close();
            }


            return new XmlTextReader(stream);
        }


        /// <summary>
        /// Provider specific code to set up the updating/ed event handlers used by UpdateDataset
        /// </summary>
        /// <param name="dataAdapter">DataAdapter to attach the event handlers to</param>
        /// <param name="rowUpdatingHandler">The handler to be called when a row is updating</param>
        /// <param name="rowUpdatedHandler">The handler to be called when a row is updated</param>
        protected override void AddUpdateEventHandlers(IDbDataAdapter dataAdapter, RowUpdatingHandler rowUpdatingHandler, RowUpdatedHandler rowUpdatedHandler)
        {
            if (rowUpdatingHandler != null)
            {
                MRowUpdating = rowUpdatingHandler;
                ((MySqlDataAdapter)dataAdapter).RowUpdating += RowUpdating;
            }


            if (rowUpdatedHandler != null)
            {
                MRowUpdated = rowUpdatedHandler;
                ((MySqlDataAdapter)dataAdapter).RowUpdated += RowUpdated;
            }
        }


        /// <summary>
        /// Handles the RowUpdating event
        /// </summary>
        /// <param name="obj">The object that published the event</param>
        /// <param name="e">The SqlRowUpdatingEventArgs</param>
        protected void RowUpdating(object obj, MySqlRowUpdatingEventArgs e)
        {
            base.RowUpdating(obj, e);
        }


        /// <summary>
        /// Handles the RowUpdated event
        /// </summary>
        /// <param name="obj">The object that published the event</param>
        /// <param name="e">The SqlRowUpdatedEventArgs</param>
        protected void RowUpdated(object obj, MySqlRowUpdatedEventArgs e)
        {
            base.RowUpdated(obj, e);
        }


        /// <summary>
        /// Handle any provider-specific issues with BLOBs here by "washing" the IDataParameter and returning a new one that is set up appropriately for the provider.
        /// </summary>
        /// <param name="connection">The IDbConnection to use in cleansing the parameter</param>
        /// <param name="p">The parameter before cleansing</param>
        /// <returns>The parameter after it's been cleansed.</returns>
        protected override IDataParameter GetBlobParameter(IDbConnection connection, IDataParameter p)
        {
            // do nothing special for BLOBs...as far as we know now.
            return p;
        }

        #endregion

        public string GetString(IDataReader rdr, int i)
        {
            if (i < 0 || i >= rdr.FieldCount) return string.Empty;
            return rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
        }

        public bool GetBoolean(IDataReader rdr, int i)
        {
            if (i < 0 || i >= rdr.FieldCount) return false;
            return !rdr.IsDBNull(i) && rdr.GetBoolean(i);
        }

        public int GetInt(IDataReader rdr, int i)
        {
            if (i < 0 || i >= rdr.FieldCount) return 0;
            return rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
        }

        public decimal GetDecimal(IDataReader rdr, int i)
        {
            if (i < 0 || i >= rdr.FieldCount) return 0;
            return rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
        }

        public DateTime GetDateTime(IDataReader rdr, int i)
        {
            if (i < 0 || i >= rdr.FieldCount) return DateTime.MinValue;
            return rdr.IsDBNull(i) ? DateTime.MinValue : rdr.GetDateTime(i);
        }

        public string GetString(IDataReader rdr, string name)
        {
            var i = rdr.GetOrdinal(name);
            return GetString(rdr, i);
        }

        public bool GetBoolean(IDataReader rdr, string name)
        {
            var i = rdr.GetOrdinal(name);
            return GetBoolean(rdr, i);
        }

        public int GetInt(IDataReader rdr, string name)
        {
            var i = rdr.GetOrdinal(name);
            return GetInt(rdr, i);
        }

        public decimal GetDecimal(IDataReader rdr, string name)
        {
            var i = rdr.GetOrdinal(name);
            return GetDecimal(rdr, i);
        }

        public DateTime GetDateTime(IDataReader rdr, string name)
        {
            var i = rdr.GetOrdinal(name);
            return GetDateTime(rdr, i);
        }
    }
}
