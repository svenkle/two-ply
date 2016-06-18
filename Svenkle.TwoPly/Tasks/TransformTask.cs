using System;
using System.IO.Abstractions;
using Microsoft.Build.Framework;
using Svenkle.TwoPly.Services;
using Svenkle.TwoPly.Services.Interfaces;

namespace Svenkle.TwoPly.Tasks
{
    public class Transform : Microsoft.Build.Utilities.Task
    {
        private readonly IXmlTransformService _xmlTransformService;

        public Transform()
        {
            _xmlTransformService = new XmlTransformService(new FileSystem());
        }

        public Transform(IXmlTransformService xmlTransformService)
        {
            _xmlTransformService = xmlTransformService;
        }

        public override bool Execute()
        {
            try
            {
                _xmlTransformService.Transform(SourceFile, TransformFile, DestinationFile);
                return true;
            }
            catch (Exception exception)
            {
                Log.LogErrorFromException(exception);
                return false;
            }
        }

        [Required]
        public string SourceFile { get; set; }

        [Required]
        public string TransformFile { get; set; }

        [Required]
        public string DestinationFile { get; set; }
    }
}
