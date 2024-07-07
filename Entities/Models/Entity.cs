using Entities.LinkModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Entities.Models
{
    public  class Entity : DynamicObject, IDictionary<string, object?>
    {
        private readonly IDictionary<string, object> _expando;

        public Entity()
        {
            _expando = new ExpandoObject();
        }

        public object? this[string key] { get => _expando[key]; set => _expando[key] = value; }

        public ICollection<string> Keys => _expando.Keys;

        public ICollection<object?> Values => _expando.Values;

        public int Count => _expando.Count;

        public bool IsReadOnly => _expando.IsReadOnly;

        public void Add(string key, object? value)
        {
            _expando.Add(key, value);   
        }

        public void Add(KeyValuePair<string, object?> item)
        {
            _expando.Add(item);
        }

        public void Clear()
        {
            _expando?.Clear();
        }

        public bool Contains(KeyValuePair<string, object?> item)
        {
           return _expando.Contains(item);  
        }

        public bool ContainsKey(string key)
        {
           return _expando.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        {
            _expando.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return _expando.GetEnumerator();
        }

        public bool Remove(string key)
        {
           return _expando.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object?> item)
        {
           return _expando.Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        {
            return _expando.Remove(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }

        private void WriteLinksToXml(string key,object value,XmlWriter writer)
        {
            writer.WriteStartElement(key);
            if(value.GetType() == typeof(List<Link>))
            {
                foreach (var val in value as List<Link> )
                {
                    writer.WriteStartElement(nameof(Link));
                    WriteLinksToXml(nameof(val.Href),val.Href, writer);
                    WriteLinksToXml(nameof(val.Method),val.Method, writer);
                    WriteLinksToXml(nameof(val.Rel),val.Rel, writer);
                    writer.WriteEndElement();

                }
            }
            else 
            {
                
                writer.WriteString(value.ToString());
            
            }
            writer.WriteEndElement();
        }
    }
}
