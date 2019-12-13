using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Repository
{
    public class DBContext : IDisposable
    {
        private SQLiteConnection _connection;

        private bool _isOnTransaction=false;
        
        

        public DBContext()
        {
            try
            {
                GetConnection();
                InitTables();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitTables()
        {
            _connection.CreateTable<Place>();
            _connection.CreateTable<Rent>();
            _connection.CreateTable<Tenant>();
            _connection.CreateTable<Bill>();
            _connection.CreateTable<SchedulePayment>();
        }


        public void CloseConnection()
        {
            _connection.Close();
            _connection = null;

        }

        public void RestartConnection()
        {
            if(_connection!=null)
            {
                _connection.Close();
                _connection = null;
            }
           
            GetConnection();
            InitTables();
        }

        public SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                var databasePath = TenantsApp.Shared.Helper.DBFilePath;
                _connection = new SQLiteConnection(databasePath);
                

            }
            return _connection;
        }

        public void BeginTransaction()
        {
            if (!_isOnTransaction)
            {
                _connection.BeginTransaction();
                _isOnTransaction = true;
            }
        }

        public void CommitTransaction()
        {
            if (this._connection != null)
            {
                this._connection.Commit();
                _isOnTransaction = false;
            }

        }

        public void RollbackTransaction()
        {
            _connection.Rollback();
            _isOnTransaction = false;
        }

        public void RollbackTransaction(string savePoint)
        {
            _connection.RollbackTo(savePoint);
            _isOnTransaction = false;
        }



        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
