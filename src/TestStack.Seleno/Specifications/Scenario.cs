using System;

namespace TestStack.Seleno.Specifications
{
    public abstract class Scenario<T> //: IScenario<T>, IScenarioMetadata where T : class
    {
        public T SUT { get; set; }
        protected IDataHelper Database { get; private set; }
        protected IContainer Container { get; private set; }

        public abstract Type Story { get; }
        public abstract int ScenarioNumber { get; }
        public abstract string ScenarioTitle { get; }

        public Scenario()
        {
            InitialiseSystemUnderTest();
            Database = Container.Resolve<IDataHelper>();
            Database.ResetDatabase();
        }

        //public virtual void BddifyMe()
        //{
        //    string scenarioTitle = string.Format("Scenario {0}: {1}", ScenarioNumber.ToString("00"), ScenarioTitle);
        //    this.Bddify(scenarioTitle);
        //}

        protected virtual void InitialiseSystemUnderTest()
        {
            SUT = Container.Resolve<T>();
        }

    }
}
