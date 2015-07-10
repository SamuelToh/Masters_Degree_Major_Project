using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using QUT.Bio.BioPatMLDataRepository.Logic;
using QUT.Bio.BioPatMLDataRepository.DataContract;
using System.IO;

namespace BioPatMLEditor.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BioPatMLDataService
    {
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        [OperationContract]
        public List<DefinitionPatternInfo> GetALLDefPatternInfo()
        {
            List<DefinitionPatternInfo> patterns = new List<DefinitionPatternInfo>();

         
            foreach (DefinitionInfo def in RepositoryManager.GetAllDefinitionInfo())
            {
                patterns.Add(new DefinitionPatternInfo()
                    {
                        DefinitionName = def.DefinitionName,
                        MainPatternId = def.MainPatternId,
                        MainPatternName = def.MainPatternName,
                        MainPatternType = def.MainPatternType
                    });
            }
            
            return patterns;
        }

        [OperationContract]
        public String GetPatternDetailInfo(DefinitionInfo def)
        {
            return RepositoryManager.GetPatternXML(def);
        }
    }

    [DataContract]
    public class DefinitionPatternInfo
    {
        [DataMember]
        public String DefinitionName { get; set; }
        [DataMember]
        public String MainPatternName { get; set; }
        [DataMember]
        public String MainPatternId { get; set; }
        [DataMember]
        public String MainPatternType { get; set; }
    }
}
