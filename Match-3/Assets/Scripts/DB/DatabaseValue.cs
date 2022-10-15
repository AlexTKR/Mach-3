using System;
using Controllers;
using LiteDB;

namespace DB
{
    public class SavedValue<TValue>
    {
        public string Id { get; set; }
        public TValue Value { get; set; }
    }

    public interface ISaveValue<T>
    {
        void Save(T value);
        T Value { get; }
    }

    public class DatabaseValue<T> : ISaveValue<T>
    {
        private LiteCollection<SavedValue<T>> _collection;
        private SavedValue<T> _value;

        public T Value => _value.Value;

        public DatabaseValue(IDatabase idb, string key, T defaultValue = default)
        {
            _collection = idb.GetCollection<SavedValue<T>>();
            if (!_collection.Exists(value => value.Id == key))
            {
                CreateNew(key, defaultValue);
                return;
            }

            try
            {
                _value = _collection.FindOne(value => value.Id == key);
            }
            catch (System.Exception e)
            {
                CreateNew(key, defaultValue);
            }
        }

        private void CreateNew(string key, T defaultValue)
        {
            _value = new SavedValue<T>() { Id = key, Value = defaultValue };
            _collection.Upsert(_value);
        }

        public void Save(T value)
        {
            _value.Value = value;
            _collection.Upsert(_value);
        }
    }
}

