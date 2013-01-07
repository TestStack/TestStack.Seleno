using System;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;

namespace TestStack.Seleno.Tests.Specify
{
    public class SpecStoryMetaDataScanner : IStoryMetaDataScanner
    {
        public virtual StoryMetaData Scan(object testObject, Type explicityStoryType = null)
        {
            var specification = testObject as ISpecification;
            if (specification == null)
                return null;

            var story = new StoryAttribute() {Title = specification.Story.Name};
            return new StoryMetaData(specification.Story, story);
        }
    }
}