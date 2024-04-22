using Dapper;
using LuxeIQ.Common;
using LuxeIQ.Data;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LuxeIQ.Repositories
{
    public class ServiceRepository : IServiceRepository, IDisposable
    {
        public ServiceRepository()
        {
        }


        public static int GetRecordsByFilterCount(List<TableColumns> tableColumns, string table_name, string dbschema_name, string searchable = "")
        {
            int lcount = 0;
            try
            {
                using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                {
                    StringBuilder sb = new StringBuilder();

                    //Only numeric type values 00.0000 to 00.00

                    sb.Append(string.Format("SELECT count(*) FROM \"{0}\".\"{1}\"", dbschema_name, table_name));

                    if (!String.IsNullOrEmpty(searchable))
                    {
                        sb.Append(" WHERE ");
                        int i = 0;
                        foreach (var c in tableColumns)
                        {
                            if (c.name.ToUpper() != "SFT_POSITION_ID")
                            {
                                if (i == 0)
                                {
                                    sb.Append(string.Format("\"" + c.name + "\" LIKE  '%{0}%'", searchable));
                                }
                                if (i > 0)
                                {
                                    sb.Append(string.Format(" OR \"" + c.name + "\" LIKE  '%{0}%'", searchable));
                                }
                            }
                            i++;
                        }
                    }
                    sb.Append(";");
                    var dbResult = connectionFactory.DbConnection.Query<dynamic>(sb.ToString());
                    if (dbResult != null && dbResult.Count() > 0)
                    {
                        lcount = connectionFactory.DbConnection.ExecuteScalar<int>(sb.ToString());
                    }
                    //fetchColumns = null;
                    sb = null;
                }
            }
            catch
            {
                throw;
            }
            return lcount;
        }

        public static IList<IDictionary<string, object>> GetRecordsByFilterWithSortAndSearch(List<TableColumns> tableColumns, string table_name, string dbschema_name, int cPage, int pageSize, Int64 SFT_POSITION_ID = 0, string sortable = "", string sortDir = "", string searchable = "")
        {
            IList<IDictionary<string, object>> pgRows = null;
            try
            {
                using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                {
                    int offSet = cPage == 1 ? 0 : (cPage - 1);
                    StringBuilder sb = new StringBuilder();

                    //Only numeric type values 00.0000 to 00.00

                    if (tableColumns != null && tableColumns.Count > 0)
                    {
                        var fetchColumns = (from c in tableColumns
                                            select $"{(c.fieldType.ToLower().Equals("numeric") ? $"CASE WHEN length(\"{c.name}\"::text)<20 THEN \"{c.name}\" ELSE trunc(\"{c.name}\",20) END as \"{c.name}\"" : $"\"{c.name}\"{(c.fieldType.ToLower().Contains("numeric(") ? "::text" : c.fieldType.ToLower().Contains("bit(1)") ? "::int" : "")}")}").ToArray();
                        sb.Append(string.Format("SELECT " + string.Join(",", fetchColumns.Select(c => c).ToArray()) + " FROM \"{0}\".\"{1}\"", dbschema_name, table_name));
                    }
                    else
                    {
                        sb.Append(string.Format("SELECT * FROM \"{0}\".\"{1}\"", dbschema_name, table_name));
                    }
                   
                    if (!String.IsNullOrEmpty(searchable))
                    {
                        if (SFT_POSITION_ID == 0)
                        {
                            sb.Append(" WHERE ");
                        }
                        int i = 0;
                        foreach (var c in tableColumns)
                        {
                            if (c.name.ToUpper() != "SFT_POSITION_ID")
                            {
                                if (i == 0)
                                {
                                    sb.Append(string.Format("\"" + c.name + "\" LIKE  '%{0}%'", searchable));
                                }
                                if (i > 0)
                                {
                                    sb.Append(string.Format(" OR \"" + c.name + "\" LIKE  '%{0}%'", searchable));
                                }
                            }
                            i++;
                        }

                    }
                    if (!String.IsNullOrEmpty(sortable) && !String.IsNullOrEmpty(sortDir))
                    {
                        sb.Append(string.Format(" ORDER BY \"" + tableColumns[Convert.ToInt32(sortable)].name.ToString() + "\" {0}", sortDir.ToUpper()));
                    }
                    if (pageSize > 0)
                    {
                        sb.Append(string.Format(" LIMIT {0}", pageSize));
                        sb.Append(string.Format(" OFFSET {0};", offSet));
                    }
                    var dbResult = connectionFactory.DbConnection.Query<dynamic>(sb.ToString());
                    if (dbResult != null && dbResult.Count() > 0)
                    {
                        pgRows = dbResult.Cast<IDictionary<string, object>>().ToList();
                    }
                    //fetchColumns = null;
                    sb = null;
                }
            }
            catch
            {
                throw;
            }
            return pgRows;
        }

        public static IList<IDictionary<string, object>> GetRecordsByFilter(List<TableColumns> tableColumns, string table_name, string dbschema_name, int cPage, int pageSize, string SFT_POSITION_ID = "")
        {
            IList<IDictionary<string, object>> pgRows = null;
            try
            {
                using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                {
                    int offSet = cPage == 1 ? 0 : (cPage - 1);
                    StringBuilder sb = new StringBuilder();

                    //Only numeric type values 00.0000 to 00.00

                    if (tableColumns != null && tableColumns.Count > 0)
                    {
                        var fetchColumns = (from c in tableColumns
                                            select $"{(c.fieldType.ToLower().Equals("numeric") ? $"CASE WHEN length(\"{c.name}\"::text)<20 THEN \"{c.name}\" ELSE trunc(\"{c.name}\",20) END as \"{c.name}\"" : $"\"{c.name}\"{(c.fieldType.ToLower().Contains("numeric(") ? "::text" : c.fieldType.ToLower().Contains("bit(1)") ? "::int" : "")}")}").ToArray();
                        sb.Append(string.Format("SELECT " + string.Join(",", fetchColumns.Select(c => c).ToArray()) + " FROM \"{0}\".\"{1}\"", dbschema_name, table_name));
                    }
                    else
                    {
                        sb.Append(string.Format("SELECT * FROM \"{0}\".\"{1}\"", dbschema_name, table_name));
                    }
                    if (SFT_POSITION_ID != "")
                    {
                        DatabaseTableColumns column = new DatabaseTableColumns();
                        List<DatabaseTableColumns> ldatabaseTableColumns = ServiceRepository.GetPGDatabaseTableColumns(table_name, "products");
                        if (ldatabaseTableColumns != null && ldatabaseTableColumns.Count > 0)
                        {
                            column = ldatabaseTableColumns.Where(p => p.name.ToLower().Contains("article")).FirstOrDefault();
                        }
                        sb.Append(string.Format(" WHERE \"{0}\"='{1}';", column.name, SFT_POSITION_ID));
                    }
                    if (pageSize > 0)
                    {
                        sb.Append(string.Format(" LIMIT {0}", pageSize));
                        sb.Append(string.Format(" OFFSET {0};", offSet));
                    }
                    var dbResult = connectionFactory.DbConnection.Query<dynamic>(sb.ToString());
                    if (dbResult != null && dbResult.Count() > 0)
                    {
                        pgRows = dbResult.Cast<IDictionary<string, object>>().ToList();
                    }
                    //fetchColumns = null;
                    sb = null;
                }
            }
            catch
            {
                throw;
            }
            return pgRows;
        }

        public static async Task<int> BulkInsertForProducts(IList<IDictionary<string, object>> items, List<TableColumns> lcolumns, string schema_Name, string table_name)
        {
            IList<IDictionary<string, object>> pItems = null;

            if (items.Count > 0)
            {
                pItems = new List<IDictionary<string, object>>();
                var fetchColumns = (from c in lcolumns
                                    select c.name).ToArray();
                if (fetchColumns.Length > 0)
                {
                    foreach (var x in items)
                    {
                        //var lrow = x;
                        var lkeys = fetchColumns.Except(x.Keys).ToList();
                        if (lkeys.Count > 0)
                        {
                            var pItem = new Dictionary<string, object>();
                            foreach (var m in fetchColumns)
                            {
                                if (x.ContainsKey(m))
                                {
                                    pItem.Add(m, x[m]);
                                }
                                else
                                {
                                    if (m == "manufacturerId")
                                    {
                                        pItem.Add(m, lcolumns[0].defaultValue);
                                    }
                                    else if (m == "wholesalerId")
                                    {
                                        pItem.Add(m, lcolumns[0].defaultValue);
                                    }
                                    else
                                    {
                                        pItem.Add(m, string.Empty);
                                    }
                                }
                            }
                            pItems.Add(pItem);
                        }
                        else
                        {
                            pItems.Add(x);
                        }
                    }
                }
            }
            //Remove special chars in table name
            var primaryKeys = lcolumns.Where(c => c.isPrimaryKey == true).Select(c => c.name).ToArray();
            var nonPrimaryKeys = lcolumns.Where(c => c.isPrimaryKey != true).Select(c => c.name).ToArray();
            {
                StringBuilder sb = new StringBuilder();
                var properties = pItems[0].Keys;
                int rowCount = 0;
                sb.Append("WITH temp AS  (");
                sb.Append("INSERT INTO ");
                sb.Append(string.Format("\"{0}\".\"{1}\"", schema_Name, table_name));
                sb.Append(string.Format("({0}) VALUES ", string.Join(",", properties.Select(x => "\"" + x + "\"").ToArray())));
                sb.Append(string.Join(",", pItems.Select(c => string.Format(" ({0}) ", string.Join(", ", c.Values.Select(v => (string.IsNullOrEmpty(v.ToString()) ? "NULL" : "'" + v.ToString().Trim().Replace("'", "''") + "'")).ToArray()))).ToArray()));

                if (primaryKeys.Count() > 0)
                {
                    //do update if conflict with primary key
                    sb.Append(" ON CONFLICT (" + string.Join(",", primaryKeys.Select(c => "\"" + c + "\"").ToArray()) + ")");
                    if (nonPrimaryKeys != null && nonPrimaryKeys.Count() > 0)
                        sb.Append(string.Format(" DO UPDATE SET {0}", string.Join(",", nonPrimaryKeys.Select(x => "\"" + x + "\"=EXCLUDED.\"" + x + "\"").ToArray())));
                    else
                        sb.Append(" DO NOTHING");
                }
                sb.Append(" RETURNING xmax) SELECT COUNT(*) AS total_rows,SUM(CASE WHEN xmax::text = '0' THEN 1 ELSE 0 END) AS inserted_rows,SUM(CASE WHEN xmax::text <> '0' THEN 1 ELSE 0 END) AS updated_rows FROM temp;");

                //Create new connection
                using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                {
                    var pgQueryStatus = await connectionFactory.DbConnection.QueryFirstOrDefaultAsync<PGQueryStatus>(sb.ToString());
                    if (!pgQueryStatus.IsNull())
                    {
                        //rowCount = pgQueryStatus.inserted_rows;
                        rowCount = pgQueryStatus.total_rows;
                    }
                }
                sb = null;
                return rowCount;
            }

            return 0;
        }

        public static async Task<int> BulkInsert(IList<Dictionary<string, object>> items, List<TableColumns> lcolumns, string schema_Name, string table_name)
        {
            IList<Dictionary<string, object>> pItems = null;

            if (items.Count > 0)
            {
                pItems = new List<Dictionary<string, object>>();
                var fetchColumns = (from c in lcolumns
                                    select c.name).ToArray();
                if (fetchColumns.Length > 0)
                {
                    foreach (var x in items)
                    {
                        //var lrow = x;
                        var lkeys = fetchColumns.Except(x.Keys).ToList();
                        if (lkeys.Count > 0)
                        {
                            var pItem = new Dictionary<string, object>();
                            foreach (var m in fetchColumns)
                            {
                                if (x.ContainsKey(m))
                                {
                                    pItem.Add(m, x[m]);
                                }
                                else
                                {
                                    if (m == "manufacturerId")
                                    {
                                        pItem.Add(m, lcolumns[0].defaultValue);
                                    }
                                    else if (m == "wholesalerId")
                                    {
                                        pItem.Add(m, lcolumns[0].defaultValue);
                                    }
                                    else
                                    {
                                        pItem.Add(m, string.Empty);
                                    }
                                }
                            }
                            pItems.Add(pItem);
                        }
                        else
                        {
                            pItems.Add(x);
                        }
                    }
                }
            }
            //Remove special chars in table name
            var primaryKeys = lcolumns.Where(c => c.isPrimaryKey == true).Select(c => c.name).ToArray();
            var nonPrimaryKeys = lcolumns.Where(c => c.isPrimaryKey != true).Select(c => c.name).ToArray();
            {
                StringBuilder sb = new StringBuilder();
                var properties = pItems[0].Keys;
                int rowCount = 0;
                sb.Append("WITH temp AS  (");
                sb.Append("INSERT INTO ");
                sb.Append(string.Format("\"{0}\".\"{1}\"", schema_Name, table_name));
                sb.Append(string.Format("({0}) VALUES ", string.Join(",", properties.Select(x => "\"" + x + "\"").ToArray())));
                sb.Append(string.Join(",", pItems.Select(c => string.Format(" ({0}) ", string.Join(", ", c.Values.Select(v => (string.IsNullOrEmpty(v.ToString()) ? "NULL" : "'" + v.ToString().Trim().Replace("'", "''") + "'")).ToArray()))).ToArray()));

                if (primaryKeys.Count() > 0)
                {
                    //do update if conflict with primary key
                    sb.Append(" ON CONFLICT (" + string.Join(",", primaryKeys.Select(c => "\"" + c + "\"").ToArray()) + ")");
                    if (nonPrimaryKeys != null && nonPrimaryKeys.Count() > 0)
                        sb.Append(string.Format(" DO UPDATE SET {0}", string.Join(",", nonPrimaryKeys.Select(x => "\"" + x + "\"=EXCLUDED.\"" + x + "\"").ToArray())));
                    else
                        sb.Append(" DO NOTHING");
                }
                sb.Append(" RETURNING xmax) SELECT COUNT(*) AS total_rows,SUM(CASE WHEN xmax::text = '0' THEN 1 ELSE 0 END) AS inserted_rows,SUM(CASE WHEN xmax::text <> '0' THEN 1 ELSE 0 END) AS updated_rows FROM temp;");

                //Create new connection
                using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                {
                    var pgQueryStatus = await connectionFactory.DbConnection.QueryFirstOrDefaultAsync<PGQueryStatus>(sb.ToString());
                    if (!pgQueryStatus.IsNull())
                    {
                        //rowCount = pgQueryStatus.inserted_rows;
                        rowCount = pgQueryStatus.total_rows;
                    }
                }
                sb = null;
                return rowCount;
            }

            return 0;
        }

        public static int DropPostgresTable(string table_name)
        {
            try
            {
                //Check columns are there or not
                if (!string.IsNullOrEmpty(table_name))
                {
                    var lmatches = LuxeIQ.Common.Utilities.ValidSpecialChars(table_name);
                    if (lmatches != null && lmatches.Count > 0)
                    {
                        return -1;
                    }
                    //Create new connection to sync database

                    using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" DROP TABLE ");
                        sb.Append(string.Format("\"{0}\".\"{1}\"", "products", table_name));
                        sb.Append(";");
                        sb.Append(" DELETE FROM ");
                        sb.Append(string.Format("\"{0}\".\"{1}\"", "public", "Products"));
                        sb.Append(" WHERE \"tableName\"='" + table_name + "';");
                        //Excute create table script
                        connectionFactory.DbConnection.Execute(sb.ToString());
                        sb = null;
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("get PG Records Count By Name Error:{0}", ex.Message);
                return -1;
            }
            return -1;
        }
        public static int CreatePostgresTable(List<TableColumns> tableColumns, string table_name)
        {
            try
            {
                if (tableColumns != null && tableColumns.Count > 0)
                {
                    //Check columns are there or not
                    if (!string.IsNullOrEmpty(table_name))
                    {
                        var lmatches = LuxeIQ.Common.Utilities.ValidSpecialChars(table_name);
                        if (lmatches != null && lmatches.Count > 0)
                        {
                            return -1;
                        }
                        //Create new connection to sync database

                        using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" CREATE TABLE ");
                            sb.Append(string.Format("\"{0}\".\"{1}\"", "products", table_name));
                            sb.Append("(");

                            List<string> primaryKeys = new List<string>();
                            //Assign column data type
                            for (int i = 0; i < tableColumns.Count(); i++)
                            {
                                var dataType = string.Empty;
                                switch (tableColumns[i].fieldType.ToLower())
                                {
                                    case "int64":
                                    case "tinyint":
                                    case "smallint":
                                    case "number":
                                    case "integer":
                                    case "int":
                                        dataType = "int";
                                        break;
                                    case "bigint":
                                        dataType = "bigint";
                                        break;
                                    case "decimal":
                                    case "numeric":
                                    case "bigdecimal":
                                    case "bignumeric":
                                    case "float64":
                                        dataType = "numeric";
                                        break;
                                    case "emailaddress":
                                    case "phone":
                                    case "locale":
                                    case "string":
                                        dataType = "text";
                                        break;
                                    case "date":
                                    case "datetime":
                                    case "date_time":
                                        dataType = "timestamp without time zone";
                                        break;
                                    case "bool":
                                        dataType = "boolean";
                                        break;
                                    default:
                                        dataType = tableColumns[i].fieldType.ToLower();
                                        break;
                                }

                                if (tableColumns[i].maxLength > 0)
                                {
                                    if (dataType == "text")
                                    {
                                        sb.Append(string.Format("{0} character varying({1})", "\"" + tableColumns[i].name + "\"", tableColumns[i].maxLength));
                                    }
                                    else if (dataType == "numeric")
                                    {

                                        if (tableColumns[i].maxLength > 0 && tableColumns[i].precision > 0)
                                        {
                                            sb.Append(string.Format("{0} numeric({1},{2})", "\"" + tableColumns[i].name + "\"", tableColumns[i].maxLength, tableColumns[i].precision));
                                        }
                                        else if (tableColumns[i].maxLength > 0)
                                        {
                                            sb.Append(string.Format("{0} numeric({1})", "\"" + tableColumns[i].name + "\"", tableColumns[i].maxLength));
                                        }
                                        else
                                        {
                                            sb.Append(string.Format("{0} numeric", "\"" + tableColumns[i].name + "\""));
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append(string.Format("{0} {1}", "\"" + tableColumns[i].name + "\"", dataType));
                                }

                                if (tableColumns[i].isPrimaryKey == true)
                                {
                                    sb.Append(" NOT NULL");
                                    primaryKeys.Add(tableColumns[i].name);
                                }
                                if (tableColumns[i].isRequired == true)
                                {
                                    sb.Append(" NOT NULL");
                                }

                                if ((i + 1) < tableColumns.Count())
                                {
                                    sb.Append(", ");
                                }
                            }
                            //adding bigserial
                            //if (tableColumns.Count() > 1)
                            //{
                            //    sb.Append(", ");
                            //    sb.Append(string.Format("{0} {1}", "\"SFT_POSITION_ID\"", "BIGSERIAL"));
                            //}
                            if (primaryKeys.Count() > 0)
                            {
                                sb.Append(", constraint " + string.Format("pk_{0}", Utilities.RemoveSpecialCharsForPGConstraints(table_name)) + " primary key (" + string.Join(",", primaryKeys.Select(x => "\"" + x + "\"").ToArray()) + ")");
                            }
                            sb.Append(");");

                            //Excute create table script
                            connectionFactory.DbConnection.Execute(sb.ToString());
                            sb = null;
                            return 1;
                        }

                    }
                }
                else
                {
                    throw new ArgumentNullException("Sync database url is null. The param name: syncDefaultDatabaseUrl");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("get PG Records Count By Name Error:{0}", ex.Message);
                return -1;
            }

            return -1;
        }

        public static int isTableExist(string schema_name, string table_name)
        {
            var recordCount = 0;
            try
            {

                if (!string.IsNullOrEmpty(table_name))
                {

                    using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                    {
                        recordCount = connectionFactory.DbConnection.ExecuteScalar<int>("SELECT count(*) FROM pg_class c LEFT JOIN pg_namespace n ON n.oid = c.relnamespace where LOWER(n.nspname)=LOWER('" + schema_name + "') AND LOWER(c.relname)=LOWER('" + table_name + "') AND (c.relkind = 'r' OR c.relkind = 'v')");
                    }
                }

            }
            catch
            {
                throw;
            }
            return recordCount;
        }

        public static List<DatabaseTableColumns> GetPGDatabaseTableColumns(string tableName, string dbSchema)
        {
            List<DatabaseTableColumns> databaseTableColumns = null;
            try
            {
                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(dbSchema))
                {
                    //Create new connection to sync database
                    using (ConnectionFactory connectionFactory = new ConnectionFactory(Constants.connectionString))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT n.nspname as dbSchema,c.relname As tableName,f.attname AS name,f.attnotnull AS isRequired, pg_catalog.format_type(f.atttypid, f.atttypmod) AS fieldType,CASE WHEN p.contype = 'p' THEN 1 ELSE 0 END AS isPrimaryKey,CASE WHEN p.contype = 'u' THEN 1 ELSE 0 END AS isUniqueKey,CASE WHEN f.atthasdef = 't' THEN pg_get_expr(d.adbin, d.adrelid) END AS defaultValue FROM pg_attribute f");
                        sb.Append(" JOIN pg_class c ON c.oid = f.attrelid JOIN pg_type t ON t.oid = f.atttypid");
                        sb.Append(" LEFT JOIN pg_attrdef d ON d.adrelid = c.oid AND d.adnum = f.attnum");
                        sb.Append(" LEFT JOIN pg_namespace n ON n.oid = c.relnamespace");
                        sb.Append(" LEFT JOIN (select * from pg_constraint pc where pc.contype in ('p','u')) p ON p.conrelid = c.oid AND f.attnum = ANY(p.conkey)");
                        sb.Append(" WHERE c.relkind = 'r'::char AND f.attnum > 0 AND n.nspname <> 'information_schema' AND n.nspname <> 'pg_catalog' AND c.relname <> '__EFMigrationsHistory'");
                        sb.Append(" AND n.nspname = '" + dbSchema + "' AND c.relname='" + tableName + "'");
                        //Excute create script
                        databaseTableColumns = connectionFactory.DbConnection.Query<DatabaseTableColumns>(sb.ToString()).ToList();

                        sb.Clear();
                        sb.Length = 0;
                        sb = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:{0}", ex.Message);
            }
            return databaseTableColumns;
        }

        public void Dispose()
        {

        }
    }
}
