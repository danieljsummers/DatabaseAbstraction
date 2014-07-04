namespace DatabaseAbstraction.Tests
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Unit Tests for the MockDatabaseService class
    /// </summary>
    [TestClass]
    public class MockDatabaseServiceTest
    {
        #region Tests

        /// <summary>
        /// Test the constructor
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_Constructor_Success()
        {
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));
            Assert.IsNotNull(data, "The Mock Database Service should not have been null");

            // We need the database queries so we can subtract them from the count
            var databaseQueries = new Dictionary<string, DatabaseQuery>();
            new DatabaseQueryProvider().Queries(databaseQueries);

            Assert.AreEqual(4, data.GetQueries().Count - databaseQueries.Count,
                "There should have been 4 queries loaded");
        }

        /// <summary>
        /// Test the Select() and AssertPerformedSelect() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_SelectAndAssertPerformedSelect_Success()
        {
            // Set up some test data
            var result1 = new StubResultSet("col1", "col2")
                .AddRow(1, "Unit")
                .AddRow(2, "Tests")
                .AddRow(3, "Rock");

            var result2 = new StubResultSet("col1", "col2")
                .AddRow(1, "Yes")
                .AddRow(2, "They")
                .AddRow(3, "Do");
            
            var result3 = new StubResultSet("col1", "col2")
                .AddRow(1, "Find Those Bugs")
                .AddRow(2, "Before")
                .AddRow(3, "They Find You");

            // Initialize the service
            var data = new MockDatabaseService(new StubDataReader(result1, result2, result3), typeof(TestQueryProvider));
            var model = new TestModel { Col1 = 13 };

            // Check for expected results
            using (var reader = data.Select("test.select"))
            {
                Assert.IsNotNull(reader, "No-Parameter Select should not have returned null");
                Assert.IsTrue(reader.Read(), "No-Parameter Results should not have been empty");
                Assert.AreEqual("Unit", reader[1], "No-Parameter Select returned unexpected results");
            }

            using (var reader = data.Select("test.select", model))
            {
                Assert.IsNotNull(reader, "Database Model Select should not have returned null");
                Assert.IsTrue(reader.Read(), "Database Model results should not have been empty");
                Assert.AreEqual("Yes", reader[1], "Database Model Select returned unexpected results");
            }

            using (var reader = data.Select("test.select", model.Parameters()))
            {
                Assert.IsNotNull(reader, "Parameter Dictionary Select should not have returned null");
                Assert.IsTrue(reader.Read(), "Parameter Dictionary Select results should not have been empty");
                Assert.AreEqual("Find Those Bugs", reader[1], "Parameter Dictionary Select returned unexpected results");
            }

            using (var reader = data.Select("test.select"))
            {
                Assert.IsNotNull(reader, "Empty Select should not have returned null");
                Assert.IsFalse(reader.Read(), "Empty Select results should have been empty");
            }

            // Did we instrument correctly?
            data.AssertPerformed("test.select");
            data.AssertPerformedSelect("test.select");
            data.AssertPerformedSelect("test.select", 4);
            data.AssertPerformedSelect("test.select", model.Parameters());
            data.AssertPerformedSelect("test.select", 2, model.Parameters());
        }

        /// <summary>
        /// Test the query type checking of the Select() method
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_Select_QueryTypeException()
        {
            try
            {
                new MockDatabaseService(new StubDataReader(), typeof(TestQueryProvider)).Select("test.insert");
                Assert.Fail("Calling Select() on an insert statement should have failed");
            }
            catch (NotSupportedException exception)
            {
                Assert.AreEqual("Query test.insert is not a select statement", exception.Message,
                    "Unexpected exception message");
            }
        }

        /// <summary>
        /// Test the Insert() and AssertPerformedInsert() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_InsertAndAssertPerformedInsert_Success()
        {
            // Initialize the service
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));
            var model = new TestModel { Col1 = 4, Col2 = "UnitTest" };

            // Run the test
            data.Insert("test.insert", model);

            model.Col2 = "TestUnit";
            data.Insert("test.insert", model.Parameters());

            // Check the results
            data.AssertPerformed("test.insert");
            data.AssertPerformedInsert("test.insert");
            data.AssertPerformedInsert("test.insert", 2);
            data.AssertPerformedInsert("test.insert", model.Parameters());

            model.Col2 = "UnitTest";
            data.AssertPerformedInsert("test.insert", 1, model.Parameters());
        }

        /// <summary>
        /// Test the query type checking of the Insert() method
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_Insert_QueryTypeException()
        {
            try
            {
                new MockDatabaseService(null, typeof(TestQueryProvider))
                    .Insert("test.update", new ParameterDictionary());
                Assert.Fail("Calling Insert() on an update statement should have failed");
            }
            catch (NotSupportedException exception)
            {
                Assert.AreEqual("Query test.update is not an insert statement", exception.Message,
                    "Unexpected exception message");
            }
        }

        /// <summary>
        /// Test the Update() and AssertPerformedUpdate() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_UpdateAndAssertPerformedUpdate_Success()
        {
            // Initialize the service
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));
            var model = new TestModel { Col1 = 5, Col2 = "UnitTest" };

            // Run the test
            data.Update("test.update", model);

            model.Col1 = 7;
            model.Col2 = "TestUnit";
            data.Update("test.update", model.Parameters());

            // Check the results
            data.AssertPerformed("test.update");
            data.AssertPerformedUpdate("test.update");
            data.AssertPerformedUpdate("test.update", 2);
            data.AssertPerformedUpdate("test.update", model.Parameters());

            model.Col1 = 5;
            model.Col2 = "UnitTest";
            data.AssertPerformedUpdate("test.update", 1, model.Parameters());
        }

        /// <summary>
        /// Test the query type checking of the Update() method
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_Update_QueryTypeException()
        {
            try
            {
                new MockDatabaseService(null, typeof(TestQueryProvider))
                    .Update("test.delete", new ParameterDictionary());
                Assert.Fail("Calling Update() on a delete statement should have failed");
            }
            catch (NotSupportedException exception)
            {
                Assert.AreEqual("Query test.delete is not an update statement", exception.Message,
                    "Unexpected exception message");
            }
        }

        /// <summary>
        /// Test the Delete() and AssertPerformedDelete() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_DeleteAndAssertPerformedDelete_Success()
        {
            // Initialize the service
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));
            var model = new TestModel { Col1 = 24, Col2 = "UnitTest" };

            // Run the test
            data.Delete("test.delete", model);

            model.Col1 = 25;
            model.Col2 = "TestUnit";
            data.Delete("test.delete", model.Parameters());

            // Check the results
            data.AssertPerformed("test.delete");
            data.AssertPerformedDelete("test.delete");
            data.AssertPerformedDelete("test.delete", 2);
            data.AssertPerformedDelete("test.delete", model.Parameters());

            model.Col1 = 24;
            model.Col2 = "UnitTest";
            data.AssertPerformedDelete("test.delete", 1, model.Parameters());
        }

        /// <summary>
        /// Test the query type checking of the Delete() method
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_Delete_QueryTypeException()
        {
            try
            {
                new MockDatabaseService(null, typeof(TestQueryProvider))
                    .Delete("test.select", new ParameterDictionary());
                Assert.Fail("Calling Delete() on a select statement should have failed");
            }
            catch (NotSupportedException exception)
            {
                Assert.AreEqual("Query test.select is not a delete statement", exception.Message,
                    "Unexpected exception message");
            }
        }

        /// <summary>
        /// Test the Sequence() and AssertPerformedSequence() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_SequenceAndAssertPerformedSequence_Success()
        {
            var data = new MockDatabaseService(null);
            data.Sequence("unit_test_sequence");

            data.AssertPerformed("unit_test_sequence");
            data.AssertPerformedSequence("unit_test_sequence");
            data.AssertPerformedSequence("unit_test_sequence", 1);
        }

        /// <summary>
        /// Test the Identity() and AssertPerformedIdentity() methods
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_IdentityAndAssertPerformedIdentity_Success()
        {
            var data = new MockDatabaseService(null);
            data.LastIdentity();
            data.LastIdentity();
            data.LastIdentity();

            data.AssertPerformedIdentity();
            data.AssertPerformedIdentity(3);
        }

        /// <summary>
        /// Test a call for a query that is not in the library
        /// </summary>
        [TestMethod]
        public void MockDatabaseService_QueryNotFound_Failure()
        {
            try
            {
                new MockDatabaseService(null, typeof(TestQueryProvider)).Select("invalid.query");
                Assert.Fail("Invalid Query should have thrown an exception");
            }
            catch (KeyNotFoundException exception)
            {
                Assert.AreEqual("Unable to find query invalid.query", exception.Message,
                    "Unexpected exception message");
            }
        }

        #endregion

        #region Support Classes

        /// <summary>
        /// A few queries to use for testing
        /// </summary>
        private class TestQueryProvider : IDatabaseQueryProvider
        {
            public string Prefix
            {
                get { return "test."; }
                set { throw new NotImplementedException("Leave my prefix alone!"); }
            }

            public void Queries(IDictionary<string, DatabaseQuery> queryLibrary)
            {
                queryLibrary.Add(Prefix + "select", Select());
                queryLibrary.Add(Prefix + "insert", Insert());
                queryLibrary.Add(Prefix + "update", Update());
                queryLibrary.Add(Prefix + "delete", Delete());
            }

            private DatabaseQuery Select()
            {
                return new DatabaseQuery { SQL = "SELECT col1, col2 from tbl" };
            }

            private DatabaseQuery Insert()
            {
                var query = new DatabaseQuery { SQL = "INSERT INTO tbl (col1, col2) VALUES (@col1, @col2)" };
                query.Parameters.Add("col1", DbType.Int32);
                query.Parameters.Add("col2", DbType.String);

                return query;
            }

            private DatabaseQuery Update()
            {
                var query = new DatabaseQuery { SQL = "UPDATE tbl SET col2 = @val WHERE col1 = @col1" };
                query.Parameters.Add("col1", DbType.Int32);
                query.Parameters.Add("col2", DbType.String);

                return query;
            }

            private DatabaseQuery Delete()
            {
                var query = new DatabaseQuery { SQL = "DELETE FROM tbl WHERE col1 = @col1" };
                query.Parameters.Add("col1", DbType.Int32);

                return query;
            }
        }

        /// <summary>
        /// A database model to use for testing
        /// </summary>
        private class TestModel : IParameterProvider
        {
            public int Col1 { get; set; }
            public string Col2 { get; set; }

            public IDictionary<string, object> Parameters()
            {
                var parameters = new Dictionary<string, object>();

                parameters.Add("col1", Col1);
                parameters.Add("col2", Col2);

                return parameters;
            }
        }

        #endregion

    }
}