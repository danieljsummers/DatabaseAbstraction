namespace DatabaseAbstraction.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils.UnitTest;
    using NUnit.Framework;

    /// <summary>
    /// Unit Tests for the MockDatabaseService class
    /// </summary>
    [TestFixture]
    public class MockDatabaseServiceTest
    {
        #region Tests

        /// <summary>
        /// Test the constructor
        /// </summary>
        [Test]
        public void Constructor()
        {
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));
            Assert.IsNotNull(data, "The Mock Database Service should not have been null");

            // We need the database queries so we can subtract them from the count
            var databaseQueries = new Dictionary<string, DatabaseQuery>();
            new DatabaseQueryProvider().Queries(databaseQueries);

            Assert.AreEqual(4, data.Queries.Count - databaseQueries.Count, "There should have been 4 queries loaded");
        }

        /// <summary>
        /// Test the Select() and AssertPerformedSelect() methods
        /// </summary>
        [Test]
        public void SelectAndAssertPerformedSelect()
        {
            // Set up some test data
            var result1 = new StubResultSet("col1", "col2");
            result1.AddRow(1, "Unit");
            result1.AddRow(2, "Tests");
            result1.AddRow(3, "Rock");

            var result2 = new StubResultSet("col1", "col2");
            result2.AddRow(1, "Yes");
            result2.AddRow(2, "They");
            result2.AddRow(3, "Do");
            
            var result3 = new StubResultSet("col1", "col2");
            result3.AddRow(1, "Find Those Bugs");
            result3.AddRow(2, "Before");
            result3.AddRow(3, "They Find You");

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

            // Validate type checking
            try
            {
                data.Select("test.insert");
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
        [Test]
        public void InsertAndAssertPerformedInsert()
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

            // Validate type checking
            try
            {
                data.Insert("test.update", new Dictionary<string, object>());
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
        [Test]
        public void UpdateAndAssertPerformedUpdate()
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

            // Validate type checking
            try
            {
                data.Update("test.delete", new Dictionary<string, object>());
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
        [Test]
        public void DeleteAndAssertPerformedDelete()
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

            // Validate type checking
            try
            {
                data.Delete("test.select", new Dictionary<string, object>());
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
        [Test]
        public void SequenceAndAssertPerformedSequence()
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
        [Test]
        public void IdentityAndAssertPerformedIdentity()
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
        [Test]
        public void QueryNotFound()
        {
            var data = new MockDatabaseService(null, typeof(TestQueryProvider));

            try
            {
                data.Select("invalid.query");
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