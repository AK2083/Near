using Near;
using NearTest.Model;
using System.Reflection;

namespace NearTest
{
    [TestClass]
    public class UnitTest1
    {
        public static IEnumerable<object[]> ValidData
        {
            get
            {
                return new[]
                {
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBefore).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2018, 1, 1)
                        },
                        new DummyClassBefore()
                    },
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBefore).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2018, 4, 1)
                        },
                        new DummyClassBefore()
                    },
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBefore).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2018, 12, 31)
                        },
                        new DummyClassBefore()
                    },
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBetween).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2023, 5, 1)
                        },
                        new DummyClassBetween()
                    },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ValidData))]
        public void GivenObjectsWithValidity_WhenLookForWorkflowByDate_ThenGetCorrectWorkflow(WorkflowAssignerData data, IDummyData dummy)
        {
            var obj = WorkflowAssigner.GetWorkflowObjectByDate(data);
            var CurrentWorkflow = Activator.CreateInstance(obj);

            Assert.IsTrue(CurrentWorkflow?.GetType() == dummy.GetType());
        }

        public static IEnumerable<object[]> InValidData
        {
            get
            {
                return new[]
                {
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBefore).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2017, 10, 1)
                        },
                    },
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBefore).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2019, 1, 1)
                        },
                    },
                    new object[] {
                        new WorkflowAssignerData
                        {
                            AssemblyToLoad = Assembly.GetExecutingAssembly(),
                            NamespaceToLoad = typeof(DummyClassBetween).Namespace ?? "NearTest.Model",
                            WorkflowDate = new DateTime(2026, 1, 1)
                        },
                    },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(InValidData))]
        public void GivenObjectsWithValidity_WhenLookForWorkflowByDate_ThenGetNothing(WorkflowAssignerData data)
        {
            var obj = WorkflowAssigner.GetWorkflowObjectByDate(data);
            Assert.IsNull(obj);
        }
    }
}