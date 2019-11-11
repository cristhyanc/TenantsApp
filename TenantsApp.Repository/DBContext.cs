﻿using SQLite;
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
        string dbFileName = "TenantsAppDbV1.db";
        

        public DBContext()
        {
            try
            {
                GetConnection();
                _connection.CreateTable<Place>();
                _connection.CreateTable<Rent>();
                _connection.CreateTable<Tenant>();
                _connection.CreateTable<ScheduleRent >();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dbFileName);

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