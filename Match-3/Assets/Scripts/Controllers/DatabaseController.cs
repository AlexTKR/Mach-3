using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public interface IDatabase
    {
        LiteCollection<T> GetCollection<T>();
    }

    public class DatabaseController : IDatabase
    {
        private static readonly string _path =
            Path.Combine(Application.persistentDataPath, "Database.db");

        private LiteDatabase _db;
        private Dictionary<Type, object> _collections;

        public DatabaseController()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            BsonMapper.Global.IncludeFields = true;
            _db = new LiteDatabase(_path);
            _collections = new Dictionary<Type, object>();
        }

        public LiteCollection<T> GetCollection<T>()
        {
            var type = typeof(T);
            if (_collections.ContainsKey(type))
                return (LiteCollection<T>)_collections[type];

            var newCollection =
                _db.GetCollection<T>(type.GetGenericArguments()[0].ToString().Replace(".", string.Empty));
            _collections[type] = newCollection;
            return newCollection;
        }

#if UNITY_EDITOR
        [MenuItem("Database/DeleteDatabase")]
        private static void DeleteDatabase()
        {
            File.Delete(_path);
        }
#endif
    }
}