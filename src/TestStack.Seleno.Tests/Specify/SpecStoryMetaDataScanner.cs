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

            string specificationTitle = CreateSpecificationTitle(specification);
            var story = new StoryAttribute() {Title = specificationTitle};
            return new StoryMetaData(specification.Story, story);
        }

        private string CreateSpecificationTitle(ISpecification specification)
        {
            string suffix = "Specification";
            string title = specification.Story.Name;
            if (title.EndsWith(suffix))
                title = title.Remove(title.Length - suffix.Length, suffix.Length);
            return title;
        }
    }
}
