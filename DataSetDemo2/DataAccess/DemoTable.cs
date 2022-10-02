using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetDemo2.DataAccess {
    internal class DemoTable {

        private static SqlConnection CONNECTION;
        private static SqlDataAdapter DATA_ADAPTER;
        private static string DATABASE_TABLE_NAME = "dbo.DemoTable";
        public static string DATASET_TABLE_NAME = "DemoTable";




        public static void FillDataSet(DataSet dataSet) {
            SqlDataAdapter adapter = GetOrCreateDataAdapter();
            SqlConnection connection = GetOrCreateConnection();
            connection.Open();
            adapter.Fill(dataSet, DATASET_TABLE_NAME);
            connection.Close();

            DataTable demoTable = dataSet.Tables[DATASET_TABLE_NAME];
            demoTable.PrimaryKey = new DataColumn[1] { demoTable.Columns["id"] };
            demoTable.Columns["id"].AutoIncrementSeed = 0;
            demoTable.Columns["id"].AutoIncrementStep = -1;
            demoTable.Columns["id"].ReadOnly = true;
            demoTable.Columns["aRelationshipId"].AllowDBNull = true;
            demoTable.Columns["aDateTime"].ReadOnly = true;
            demoTable.Columns["aDateTime"].AllowDBNull = true;
        }

        public static void UpdateDataSet(DataSet dataSet) {
            SqlDataAdapter adapter = GetOrCreateDataAdapter();
            SqlConnection connection = GetOrCreateConnection();
            DataTable demoTable = dataSet.Tables[DATASET_TABLE_NAME];
            connection.Open();
            adapter.Update(demoTable);
            connection.Close();
            demoTable.AcceptChanges();
        }



        #region private methods

        private static SqlConnection GetOrCreateConnection() {
            if (DemoTable.CONNECTION == null) {
                DemoTable.CONNECTION = new SqlConnection("Server=.\\SQL2019EXPRESS; Integrated security=true; Database=db_demo1;");
            }
            return DemoTable.CONNECTION;
        }

        private static SqlDataAdapter GetOrCreateDataAdapter() {
            SqlConnection conn = DemoTable.GetOrCreateConnection();

            if (DemoTable.DATA_ADAPTER == null) {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                SqlCommand selectCommand = new SqlCommand($"SELECT * FROM {DATABASE_TABLE_NAME};", conn);

                SqlCommand insertCommand = new SqlCommand($"INSERT INTO {DATABASE_TABLE_NAME} " +
                    $"(aStringColumn, aRelationshipId, aDateTime) " +
                    $"VALUES (@aStringColumn, @aRelationshipId, @aDateTime);" +
                    $"SELECT * FROM {DATABASE_TABLE_NAME} WHERE id = SCOPE_IDENTITY();", conn);
                insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
                insertCommand.Parameters.Add("@aStringColumn", SqlDbType.NVarChar, 50, "aStringColumn");
                insertCommand.Parameters.Add("@aRelationshipId", SqlDbType.Int, 4, "aRelationshipId");
                insertCommand.Parameters.Add("@aDateTime", SqlDbType.DateTime);

                SqlCommand updateCommand = new SqlCommand($"UPDATE {DATABASE_TABLE_NAME} SET " +
                    $"aStringColumn = @aStringColumn," +
                    $"aRelationshipId = @aRelationshipId" +
                    $" WHERE (" +
                    $"id = @id AND " +
                    $"aStringColumn = @oldStringValue AND " +
                    $"aRelationshipId = @oldRelationshipId" +
                    $");", conn);

                updateCommand.Parameters.Add("@aStringColumn", SqlDbType.NVarChar, 50, "aStringColumn");
                updateCommand.Parameters.Add("@aRelationshipId", SqlDbType.Int, 4, "aRelationshipId");
                updateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");
                updateCommand.Parameters.Add("@oldStringValue", SqlDbType.NVarChar, 50, "aStringColumn").SourceVersion = DataRowVersion.Original;
                updateCommand.Parameters.Add("@oldRelationshipId", SqlDbType.Int, 4, "aRelationshipId").SourceVersion = DataRowVersion.Original;

                SqlCommand deleteCommand = new SqlCommand($"DELETE FROM {DATABASE_TABLE_NAME} WHERE id = @id;", conn);
                deleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

                adapter.SelectCommand = selectCommand;
                adapter.InsertCommand = insertCommand;
                adapter.UpdateCommand = updateCommand;
                adapter.DeleteCommand = deleteCommand;

                adapter.RowUpdating += new SqlRowUpdatingEventHandler(OnRowUpdating);
                adapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);

                DemoTable.DATA_ADAPTER = adapter;
            }

            return DemoTable.DATA_ADAPTER;
        }

        private static void OnRowUpdating(object sender, SqlRowUpdatingEventArgs args) {
            if (args.StatementType == StatementType.Insert) {
                args.Command.Parameters["@aDateTime"].Value = DateTime.Now;
            }
        }

        private static void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args) {
            if (args.StatementType == StatementType.Insert) {
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        #endregion

    }
}
