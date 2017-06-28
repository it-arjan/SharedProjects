using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyData
{
    public enum MyDbType { EtfDb, ApiDbNancy, ApiDbWebApi };
    public class DataFactory
    {
        private MyDbType _dbType;
        public DataFactory(MyDbType type)
        {
            _dbType = type;
        }

        public IData Db(string baseUrl="optional", string oauthToken = "optional")
        {
            if (_dbType == MyDbType.EtfDb) return new Etf.EntityFrameworkDb();
            else return new NancyApi.NancyApiDb(baseUrl, oauthToken);
        }

        public IDataSetup DbSetup()
        {
            return new Etf.EtfSetup();
        }
    }
}
