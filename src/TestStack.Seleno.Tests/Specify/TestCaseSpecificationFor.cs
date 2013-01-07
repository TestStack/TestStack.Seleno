using System;
using Humanizer;
using TestStack.BDDfy;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class TestCaseSpecificationFor<T> : ISpecification
    {
        public T SUT { get; set; }

        public virtual Type Story
        {
            get { return typeof(T); }
        }

        public virtual void Run()
        {
            string title = BuildTitle();
            this.BDDfy(title, Category);
        }

        protected virtual string BuildTitle()
        {
            return Title ?? GetType().Name.Humanize(LetterCasing.Title);
        }

        // BDDfy methods
        public virtual void EstablishContext() { }
        public virtual void Setup() { }
        public virtual void TearDown() { }

        public virtual string Title { get; set; }
        public string Category { get; set; }
    }
}