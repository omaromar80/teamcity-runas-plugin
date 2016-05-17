﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace JetBrains.runAs.IntegrationTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Show log info")]
    public partial class ShowLogInfoFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ShowLog.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Show log info", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User sees header info when in the debug log mode")]
        public virtual void UserSeesHeaderInfoWhenInTheDebugLogMode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User sees header info when in the debug log mode", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
 testRunner.Given("I have appended the file command.cmd by the line echo", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.And("I\'ve added the argument -u:RunAsTestUser", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 6
 testRunner.And("I\'ve added the argument -p:aaa", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("I\'ve added the argument -l:debug", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("I\'ve added the argument command.cmd", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.When("I run RunAs tool", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table1.AddRow(new string[] {
                        "JetBrains RunAs"});
            table1.AddRow(new string[] {
                        "Settings:"});
#line 11
 testRunner.And("the output should contain:", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User sees header info when in the debug log mode and error")]
        public virtual void UserSeesHeaderInfoWhenInTheDebugLogModeAndError()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User sees header info when in the debug log mode and error", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
 testRunner.Given("I have appended the file command.cmd by the line echo", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.And("I\'ve added the argument -u:SomeAsTestUser", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.And("I\'ve added the argument -p:SomePassword", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.And("I\'ve added the argument -l:debug", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.And("I\'ve added the argument command.cmd", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.When("I run RunAs tool", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table2.AddRow(new string[] {
                        "JetBrains RunAs"});
            table2.AddRow(new string[] {
                        "Settings:"});
#line 24
 testRunner.And("the output should contain:", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
