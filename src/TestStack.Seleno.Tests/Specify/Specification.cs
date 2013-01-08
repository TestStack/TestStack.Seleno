using System;
using System.Collections.Generic;
using System.Linq;
using AutofacContrib.NSubstitute;
using Humanizer;
using NUnit.Framework;
using TestStack.BDDfy;

namespace TestStack.Seleno.Tests.Specify
{
    [TestFixture]
    public abstract class Specification 
    {
        protected AutoSubstitute _autoSubstitute = new AutoSubstitute();
        


        [Test]
        public virtual void Run()
        {
            string title = BuildTitle();
            this.BDDfy(title, Category);
        }

        protected virtual string BuildTitle()
        {
            return Title ?? GetType().Name.Humanize(LetterCasing.Title);
        }

        public virtual TService Fake<TService>()
            where TService : class
        {
            return _autoSubstitute.ResolveAndSubstituteFor<TService>();
        }

        public virtual TService Fake<TService>(IEnumerable<Type> implementedTypes)
            where TService : class
        {
            var types = implementedTypes.ToList();
            types.Insert(0,typeof(TService));
            return (TService)_autoSubstitute.SubstituteFor(types);
        }

      

        // BDDfy methods
        public virtual void EstablishContext() { }
        public virtual void Setup() { }
        public virtual void TearDown() { }

        public virtual Type Story { get { return GetType(); } }
        public virtual string Title { get; set; }
        public string Category { get; set; }
    }
}