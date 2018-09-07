using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YoCode
{
    internal class SolutionFileExistsCheck : ICheck
    {
        private readonly IPathManager pathManager;

        public SolutionFileExistsCheck(ICheckConfig config)
        {
            pathManager = config.PathManager;
            SolutionFileEvidence.Feature = Feature.SolutionFileExists;
        }

        private FeatureEvidence SolutionFileEvidence { get; } = new FeatureEvidence();

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                var files = pathManager.GetFilesInDirectory(pathManager.ModifiedTestDirPath, FileTypes.sln, SearchOption.TopDirectoryOnly);

                if (files.Any())
                {
                    SolutionFileEvidence.SetPassed(new SimpleEvidenceBuilder("Solution file found"));
                }
                else
                {
                    SolutionFileEvidence.SetFailed(new SimpleEvidenceBuilder("Solution file not found"));
                }
                return new List<FeatureEvidence> { SolutionFileEvidence };
            });
        }
    }
}
