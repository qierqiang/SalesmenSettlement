using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class LocalProfile
    {
        private string _fileName;

        public LocalProfile(string fileName)
        {
            _fileName = fileName;
        }

        public void Save(object o)
        {
            string content = JsonConvert.SerializeObject(o);
            File.WriteAllText(_fileName, content);
        }

        public T Load<T>(T targetObjectexmaple)
        {
            return JsonConvert.DeserializeAnonymousType(File.ReadAllText(_fileName), targetObjectexmaple);
        }
    }
}
