using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;

namespace MquterELab_UITest
{
    [TestClass]
    public class TestBioWMLResultParser
    {
        [TestMethod]
        public void TestParseResultSet()
        {
            string xmlContent = "<?xml version='1.0' encoding='utf-16'?>" +
                                    "<BioWFML>" +
                                    "  <ExecutionResult>" +
                                    "    <Collection>" +
                                    "      <Item>" +
                                    "        <Property name='Similarity'>1</Property>" +
                                    "        <Property name='MatchPattern'>QUT.Bio.BioPatML.Patterns.Motif</Property>" +
                                    "        <Property name='Matches'>5</Property>" +
                                    "        <Property name='Impact'>0.9</Property>" +
                                    "        <Property name='SubMatchNumber'>0</Property>" +
                                    "        <Property name='Length'>5</Property>" +
                                    "        <Property name='Strand'>1</Property>" +
                                    "        <Property name='Start'>1321</Property>" +
                                    "        <Property name='End'>1325</Property>" +
                                    "        <Property name='CenterPosition'>1323</Property>" +
                                    "      </Item>" +
                                    "      <Item>" +
                                    "        <Property name='Similarity'>1</Property>" +
                                    "       <Property name='MatchPattern'>QUT.Bio.BioPatML.Patterns.Motif</Property>" +
                                    "        <Property name='Matches'>5</Property>" +
                                    "        <Property name='Impact'>0.9</Property>" +
                                    "        <Property name='SubMatchNumber'>0</Property>" +
                                    "        <Property name='Length'>5</Property>" +
                                    "        <Property name='Strand'>1</Property>" +
                                    "        <Property name='Start'>15038</Property>" +
                                    "        <Property name='End'>15042</Property>" +
                                    "        <Property name='CenterPosition'>15040</Property>" +
                                    "      </Item>" +
                                    "    </Collection>" +
                                    "  </ExecutionResult>" +
                                    "</BioWFML>";
            BioWFMLResultManager resultManager = new BioWFMLResultManager();
            ResultCollections collection = resultManager.ReadBioWFML(xmlContent);
        }
    }
}
