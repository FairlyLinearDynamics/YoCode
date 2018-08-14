﻿using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;
using Moq;

namespace YoCode_XUnit
{
    public class DuplicationCheckTests
    {
        private readonly IPathManager fakeDir;
        private readonly IDupFinder fakeDupFinder;

        private readonly DuplicationCheck dupCheck;

        public DuplicationCheckTests()
        {
            var mockDir = new Mock<IPathManager>();
            var mockDupFinder = new Mock<IDupFinder>();

            var fakeOriginal = @"\fake\original\dir";
            var fakeModified = @"\fake\modified\dir";

            var fakeOriginalCodeScore = "<CodebaseCost>50 <TotalDuplicatesCost>50";
            var fakeModifiedCodeScore = "<CodebaseCost>10 <TotalDuplicatesCost>10";

            var fileNameChecked = "UnitConverterWebApp.sln";

            mockDir.Setup(w => w.OriginalTestDirPath).Returns(fakeOriginal);
            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(), Path.Combine(fakeOriginal, fileNameChecked)))
                .Returns(SetUpFeatureEvidence(fakeOriginalCodeScore));

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(), Path.Combine(fakeModified, fileNameChecked)))
                .Returns(SetUpFeatureEvidence(fakeModifiedCodeScore));

            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;

            dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder);
        }

        private FeatureEvidence SetUpFeatureEvidence(string outputToBeSet)
        {
            return new FeatureEvidence()
            {
                Output = outputToBeSet,
            };
        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented_TRUE()
        {
            dupCheck.DuplicationEvidence.FeatureImplemented.Should().BeTrue();
        }
    }
}
